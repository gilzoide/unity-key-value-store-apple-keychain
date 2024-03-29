#import <Foundation/Foundation.h>
#import <Security/Security.h>

#import "IUnityInterface.h"
#import "IUnityLog.h"

static IUnityLog *logger;

/// Log a formatted debug message to Unity's console
static void debugLogFormat(NSString* fmt, ...) {
    va_list va;
    va_start(va, fmt);
    NSString* msg = [[NSString alloc] initWithFormat:fmt arguments:va];
    va_end(va);
    UNITY_LOG(logger, msg.UTF8String);
}

/// Log OSStatus error messages to Unity's console.
/// Ignores `errSecSuccess` and `errSecItemNotFound`.
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

/// Log NSError message to Unity's console.
static void logNSError(NSError* error) {
    if (error) {
        UNITY_LOG_ERROR(logger, error.localizedDescription.UTF8String);
    }
}

/// @warning Must match the C# AppleKeychainKeyValueStore class!!!
typedef struct AppleKeychainKeyValueStore {
    const char *serviceName;
    int isSynchronizable;
    int useDataProtectionKeychain;
} AppleKeychainKeyValueStore;

static NSMutableDictionary* createBaseQuery(const AppleKeychainKeyValueStore *kvs, const char *key) {
    return [NSMutableDictionary dictionaryWithObjectsAndKeys:
        (id)kSecClassGenericPassword, (id)kSecClass,
        [NSString stringWithCString:kvs->serviceName encoding:NSUTF8StringEncoding], (id)kSecAttrService,
        [NSString stringWithCString:key encoding:NSUTF8StringEncoding], (id)kSecAttrAccount,
        nil
    ];
}

///////////////////////////////////////////////////////////
// Get/Set key-value pairs
///////////////////////////////////////////////////////////
static bool setData(const AppleKeychainKeyValueStore *kvs, const char *key, id data) {
    NSError* error = nil;
    NSData* archivedData = [NSKeyedArchiver archivedDataWithRootObject:data requiringSecureCoding:YES error:&error];
    if (error) {
        logNSError(error);
        return false;
    }

    NSMutableDictionary* query = createBaseQuery(kvs, key);
    if (@available(iOS 13.0, macOS 10.15, tvOS 13.0, watchOS 6.0, visionOS 1.0, *)) {
        [query setObject:@(kvs->useDataProtectionKeychain) forKey:(id)kSecUseDataProtectionKeychain];
    }

    OSStatus result = SecItemCopyMatching((CFDictionaryRef) query, NULL);
    switch (result) {
        case errSecSuccess: {
            NSDictionary* attributesToUpdate = @{
                (id)kSecValueData: archivedData,
                (id)kSecAttrSynchronizable: @(kvs->isSynchronizable),
            };
            OSStatus result = SecItemUpdate((CFDictionaryRef) query, (CFDictionaryRef) attributesToUpdate);
            logSecError(result);
            return result == errSecSuccess;
        }

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

static bool hasData(const AppleKeychainKeyValueStore *kvs, const char *key) {
    NSMutableDictionary* query = createBaseQuery(kvs, key);
    OSStatus result = SecItemCopyMatching((CFDictionaryRef) query, NULL);
    logSecError(result);
    return result == errSecSuccess;
}

static NSData* getData(const AppleKeychainKeyValueStore *kvs, const char *key) {
    NSMutableDictionary* query = createBaseQuery(kvs, key);
    [query setObject:@YES forKey:(id)kSecReturnData];
    CFTypeRef existingData;
    OSStatus result = SecItemCopyMatching((CFDictionaryRef) query, &existingData);
    switch (result) {
        case errSecSuccess:
            return CFBridgingRelease(existingData);

        default:
            logSecError(result);
            return nil;
    }
}

static id getTypedData(const AppleKeychainKeyValueStore *kvs, const char *key, Class cls) {
    NSData* data = getData(kvs, key);
    if (data) {
        NSError* error = nil;
        id value = [NSKeyedUnarchiver unarchivedObjectOfClass:cls fromData:data error:&error];
        logNSError(error);
        return value;
    }
    else {
        return nil;
    }
}

static bool deleteData(const AppleKeychainKeyValueStore *kvs, const char *key) {
    NSMutableDictionary* query = createBaseQuery(kvs, key);
    OSStatus result = SecItemDelete((CFDictionaryRef) query);
    logSecError(result);
    return result == errSecSuccess;
}

///////////////////////////////////////////////////////////
// Exported functions
///////////////////////////////////////////////////////////
void UNITY_INTERFACE_EXPORT UnityPluginLoad(IUnityInterfaces* unityInterfaces) {
    logger = UNITY_GET_INTERFACE(unityInterfaces, IUnityLog);
}

bool KeyValueStoreAppleKeychain_DeleteKey(const AppleKeychainKeyValueStore *kvs, const char *key) {
    return deleteData(kvs, key);
}

bool KeyValueStoreAppleKeychain_HasKey(const AppleKeychainKeyValueStore *kvs, const char *key) {
    return hasData(kvs, key);
}

bool KeyValueStoreAppleKeychain_SetBool(const AppleKeychainKeyValueStore *kvs, const char *key, int value) {
    return setData(kvs, key, @(value));
}

bool KeyValueStoreAppleKeychain_TryGetBool(const AppleKeychainKeyValueStore *kvs, const char *key, int *outValue) {
    NSNumber* value = getTypedData(kvs, key, NSNumber.class);
    if (value) {
        *outValue = value.boolValue;
        return true;
    }
    else {
        *outValue = 0;
        return false;
    }
}