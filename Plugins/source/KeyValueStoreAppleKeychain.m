#import <Foundation/Foundation.h>
#import <Security/Security.h>

#import "IUnityInterface.h"
#import "IUnityLog.h"

static IUnityLog *logger;

static void debugLogFormat(NSString* fmt, ...) {
    va_list va;
    va_start(va, fmt);
    NSString* msg = [[NSString alloc] initWithFormat:fmt arguments:va];
    va_end(va);
    UNITY_LOG(logger, msg.UTF8String);
}

static void logSecError(OSStatus result) {
    switch (result) {
        case errSecSuccess:
        case errSecItemNotFound:
            break;

        default: {
            NSString* error = CFBridgingRelease(SecCopyErrorMessageString(result, NULL));
            UNITY_LOG_ERROR(logger, error.UTF8String);
            break;
        }
    }
}

static void logNSError(NSError* error) {
    if (error) {
        UNITY_LOG_ERROR(logger, error.localizedDescription.UTF8String);
    }
}

typedef struct AppleKeychainKeyValueStore {
    const char *serviceName;
    int isSynchronizable;
    int useDataProtectionKeychain;
} AppleKeychainKeyValueStore;

static bool setData(const AppleKeychainKeyValueStore *kvs, const char *keyCstr, id data) {
    NSError* error = nil;
    NSData* archivedData = [NSKeyedArchiver archivedDataWithRootObject:data requiringSecureCoding:YES error:&error];
    if (error) {
        logNSError(error);
        return false;
    }

    NSString* service = [NSString stringWithCString:kvs->serviceName encoding:NSUTF8StringEncoding];
    NSString* key = [NSString stringWithCString:keyCstr encoding:NSUTF8StringEncoding];

    NSMutableDictionary* query = [NSMutableDictionary dictionaryWithObjectsAndKeys:
        (id)kSecClassGenericPassword, (id)kSecClass,
        service, (id)kSecAttrService,
        key, (id)kSecAttrAccount,
        nil
    ];
    if (@available(iOS 13.0, macOS 10.15, tvOS 13.0, watchOS 6.0, visionOS 1.0, *)) {
        [query setObject:@(kvs->useDataProtectionKeychain) forKey:(id)kSecUseDataProtectionKeychain];
    }

    CFTypeRef existingData;
    OSStatus result = SecItemCopyMatching((CFDictionaryRef) query, &existingData);
    switch (result) {
        case errSecSuccess:
            CFRelease(existingData);
            NSDictionary* attributesToUpdate = @{
                (id)kSecValueData: archivedData,
                (id)kSecAttrSynchronizable: @(kvs->isSynchronizable),
            };
            OSStatus result = SecItemUpdate((CFDictionaryRef) query, (CFDictionaryRef) attributesToUpdate);
            logSecError(result);
            return result == errSecSuccess;

        case errSecItemNotFound:
            if (@available(iOS 7.0, macOS 10.9, tvOS 9.0, watchOS 2.0, visionOS 1.0, *)) {
                [query setObject:@(kvs->isSynchronizable) forKey:(id)kSecAttrSynchronizable];
            }
            [query setObject:archivedData forKey:(id)kSecValueData];
            result = SecItemAdd((CFDictionaryRef) query, NULL);
            logSecError(result);
            return result == errSecSuccess;

        default:
            logSecError(result);
            return false;
    }
}

static id getData(const AppleKeychainKeyValueStore *kvs, const char *keyCstr, Class cls) {
    NSString* service = [NSString stringWithCString:kvs->serviceName encoding:NSUTF8StringEncoding];
    NSString* key = [NSString stringWithCString:keyCstr encoding:NSUTF8StringEncoding];

    NSMutableDictionary* query = [NSMutableDictionary dictionaryWithObjectsAndKeys:
        (id)kSecClassGenericPassword, (id)kSecClass,
        service, (id)kSecAttrService,
        key, (id)kSecAttrAccount,
        @YES, (id)kSecReturnData,
        nil
    ];

    CFTypeRef existingData;
    OSStatus result = SecItemCopyMatching((CFDictionaryRef) query, &existingData);
    switch (result) {
        case errSecSuccess: {
            NSData* data = CFBridgingRelease(existingData);
            NSError* error = nil;
            id value = [NSKeyedUnarchiver unarchivedObjectOfClass:cls fromData:data error:&error];
            logNSError(error);
            return value;
        }

        default:
            logSecError(result);
            return nil;
    }
}

void UNITY_INTERFACE_EXPORT UnityPluginLoad(IUnityInterfaces* unityInterfaces) {
    logger = UNITY_GET_INTERFACE(unityInterfaces, IUnityLog);
}

bool KeyValueStoreAppleKeychain_SetBool(const AppleKeychainKeyValueStore *kvs, const char *keyCstr, int value) {
    return setData(kvs, keyCstr, @(value));
}

bool KeyValueStoreAppleKeychain_TryGetBool(const AppleKeychainKeyValueStore *kvs, const char *keyCstr, int *outValue) {
    NSNumber* value = getData(kvs, keyCstr, NSNumber.class);
    if (value) {
        *outValue = value.boolValue;
        return true;
    }
    else {
        *outValue = 0;
        return false;
    }
}