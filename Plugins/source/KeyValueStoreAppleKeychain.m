#import <Foundation/Foundation.h>
#import <Security/Security.h>

#import "IUnityInterface.h"
#import "IUnityLog.h"

/// Unity logger
static IUnityLog *logger;

/// Log a formatted debug message to Unity's console
static void debugLogFormat(NSString* fmt, ...) {
    va_list va;
    va_start(va, fmt);
    if (logger) {
        NSString* msg = [[NSString alloc] initWithFormat:fmt arguments:va];
        UNITY_LOG_ERROR(logger, msg.UTF8String);
    }
    else {
        NSLogv(fmt, va);
    }
    va_end(va);
}

static void debugLogError(NSString* msg) {
    if (logger) {
        UNITY_LOG_ERROR(logger, msg.UTF8String);
    }
    else {
        NSLog(@"%@", msg);
    }
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
            debugLogError(error);
            break;
        }
    }
}

/// Log NSError message to Unity's console.
static void logNSError(NSError* error) {
    if (error) {
        debugLogError(error.debugDescription);
    }
}

static bool stringIsNullOrEmpty(const char *cStr) {
    return cStr == NULL || cStr[0] == '\0';
}

static NSString* toNSString(const char *cStr) {
    return [NSString stringWithCString:cStr encoding:NSUTF8StringEncoding];
}

/// @warning Must match the C# GenericPasswordKeychainAttributes class!!!
typedef struct GenericPasswordKeychainAttributes {
    const char *account;
    const char *service;
    const char *accessGroup;
    const char *label;
    const char *description;
    int8_t isSynchronizable;
    int8_t useDataProtectionKeychain;
} GenericPasswordKeychainAttributes;

static NSMutableDictionary* createBaseQuery(const GenericPasswordKeychainAttributes *kvs) {
    NSMutableDictionary* query = [NSMutableDictionary dictionaryWithObject:(id)kSecClassGenericPassword forKey:(id)kSecClass];
    if (!stringIsNullOrEmpty(kvs->account)) {
        [query setObject:toNSString(kvs->account) forKey:(id)kSecAttrAccount];
    }
    if (!stringIsNullOrEmpty(kvs->service)) {
        [query setObject:toNSString(kvs->service) forKey:(id)kSecAttrService];
    }
    if (!stringIsNullOrEmpty(kvs->accessGroup)) {
        [query setObject:toNSString(kvs->accessGroup) forKey:(id)kSecAttrAccessGroup];
    }
    if (@available(iOS 13.0, macOS 10.15, tvOS 13.0, watchOS 6.0, visionOS 1.0, *)) {
        [query setObject:@(kvs->useDataProtectionKeychain) forKey:(id)kSecUseDataProtectionKeychain];
    }
    return query;
}

static void fillAttributesToUpdate(const GenericPasswordKeychainAttributes *kvs, NSMutableDictionary* query, NSData* data) {
    if (!stringIsNullOrEmpty(kvs->label)) {
        [query setObject:toNSString(kvs->label) forKey:(id)kSecAttrLabel];
    }
    if (!stringIsNullOrEmpty(kvs->description)) {
        [query setObject:toNSString(kvs->description) forKey:(id)kSecAttrDescription];
    }
    if (@available(iOS 7.0, macOS 10.9, tvOS 9.0, watchOS 2.0, visionOS 1.0, *)) {
        [query setObject:@(kvs->isSynchronizable) forKey:(id)kSecAttrSynchronizable];
    }
    [query setObject:data forKey:(id)kSecValueData];
}

///////////////////////////////////////////////////////////
// Get/Set key-value pairs
///////////////////////////////////////////////////////////
static bool setKeychainData(const GenericPasswordKeychainAttributes *kvs, id data) {
    if (!data) {
        return false;
    }

    NSError* error = nil;
    NSData* archivedData = [NSKeyedArchiver archivedDataWithRootObject:data requiringSecureCoding:YES error:&error];
    if (error) {
        logNSError(error);
        return false;
    }

    NSMutableDictionary* query = createBaseQuery(kvs);
    OSStatus result = SecItemCopyMatching((CFDictionaryRef)query, NULL);
    switch (result) {
        case errSecSuccess: {
            NSMutableDictionary* attributesToUpdate = [NSMutableDictionary dictionary];
            fillAttributesToUpdate(kvs, attributesToUpdate, archivedData);
            OSStatus result = SecItemUpdate((CFDictionaryRef)query, (CFDictionaryRef)attributesToUpdate);
            logSecError(result);
            return result == errSecSuccess;
        }

        case errSecItemNotFound:
            fillAttributesToUpdate(kvs, query, archivedData);
            result = SecItemAdd((CFDictionaryRef)query, NULL);
            logSecError(result);
            return result == errSecSuccess;

        default:
            logSecError(result);
            return false;
    }
}

static NSData* getKeychainData(const GenericPasswordKeychainAttributes *kvs) {
    NSMutableDictionary* query = createBaseQuery(kvs);
    [query setObject:@YES forKey:(id)kSecReturnData];
    CFTypeRef existingData;
    OSStatus result = SecItemCopyMatching((CFDictionaryRef)query, &existingData);
    switch (result) {
        case errSecSuccess:
            return CFBridgingRelease(existingData);

        default:
            logSecError(result);
            return nil;
    }
}

static id getKeychainTypedData(const GenericPasswordKeychainAttributes *kvs, Class cls) {
    NSData* data = getKeychainData(kvs);
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

static bool deleteKeychainData(const GenericPasswordKeychainAttributes *kvs) {
    NSMutableDictionary* query = createBaseQuery(kvs);
    OSStatus result = SecItemDelete((CFDictionaryRef)query);
    logSecError(result);
    return result == errSecSuccess;
}

///////////////////////////////////////////////////////////
// Exported functions
///////////////////////////////////////////////////////////
void UNITY_INTERFACE_EXPORT UnityPluginLoad(IUnityInterfaces* unityInterfaces) {
    logger = UNITY_GET_INTERFACE(unityInterfaces, IUnityLog);
}

CFTypeRef KeyValueStoreAppleKeychain_AllocDictionary() {
    return CFBridgingRetain([NSMutableDictionary dictionary]);
}

void KeyValueStoreAppleKeychain_Release(CFTypeRef ref) {
    if (ref) {
        CFRelease(ref);
    }
}

void KeyValueStoreAppleKeychain_ClearDictionary(NSMutableDictionary* dict) {
    [dict removeAllObjects];
}

void KeyValueStoreAppleKeychain_DeleteKey(NSMutableDictionary* dict, const char *key) {
    [dict removeObjectForKey:toNSString(key)];
}

bool KeyValueStoreAppleKeychain_HasKey(NSMutableDictionary* dict, const char *key) {
    return [dict valueForKey:toNSString(key)];
}

void KeyValueStoreAppleKeychain_SetBool(NSMutableDictionary* dict, const char *key, int value) {
    [dict setObject:@(value) forKey:toNSString(key)];
}

bool KeyValueStoreAppleKeychain_TryGetBool(NSMutableDictionary* dict, const char *key, int *outValue) {
    id value = [dict valueForKey:toNSString(key)];
    if ([value isKindOfClass:NSNumber.class]) {
        *outValue = [value boolValue];
        return true;
    }
    else {
        *outValue = 0;
        return false;
    }
}

void KeyValueStoreAppleKeychain_SetInt(NSMutableDictionary* dict, const char *key, int value) {
    [dict setObject:@(value) forKey:toNSString(key)];
}

bool KeyValueStoreAppleKeychain_TryGetInt(NSMutableDictionary* dict, const char *key, int *outValue) {
    id value = [dict valueForKey:toNSString(key)];
    if ([value isKindOfClass:NSNumber.class]) {
        *outValue = [value intValue];
        return true;
    }
    else {
        *outValue = 0;
        return false;
    }
}

void KeyValueStoreAppleKeychain_SetLong(NSMutableDictionary* dict, const char *key, long value) {
    [dict setObject:@(value) forKey:toNSString(key)];
}

bool KeyValueStoreAppleKeychain_TryGetLong(NSMutableDictionary* dict, const char *key, long *outValue) {
    id value = [dict valueForKey:toNSString(key)];
    if ([value isKindOfClass:NSNumber.class]) {
        *outValue = [value longValue];
        return true;
    }
    else {
        *outValue = 0;
        return false;
    }
}

void KeyValueStoreAppleKeychain_SetFloat(NSMutableDictionary* dict, const char *key, float value) {
    [dict setObject:@(value) forKey:toNSString(key)];
}

bool KeyValueStoreAppleKeychain_TryGetFloat(NSMutableDictionary* dict, const char *key, float *outValue) {
    id value = [dict valueForKey:toNSString(key)];
    if ([value isKindOfClass:NSNumber.class]) {
        *outValue = [value floatValue];
        return true;
    }
    else {
        *outValue = 0;
        return false;
    }
}

void KeyValueStoreAppleKeychain_SetDouble(NSMutableDictionary* dict, const char *key, double value) {
    [dict setObject:@(value) forKey:toNSString(key)];
}

bool KeyValueStoreAppleKeychain_TryGetDouble(NSMutableDictionary* dict, const char *key, double *outValue) {
    id value = [dict valueForKey:toNSString(key)];
    if ([value isKindOfClass:NSNumber.class]) {
        *outValue = [value doubleValue];
        return true;
    }
    else {
        *outValue = 0;
        return false;
    }
}

void KeyValueStoreAppleKeychain_SetBytes(NSMutableDictionary* dict, const char *key, const void *bytes, int length) {
    [dict setObject:[NSData dataWithBytes:bytes length:length] forKey:toNSString(key)];
}

bool KeyValueStoreAppleKeychain_TryGetBytes(NSMutableDictionary* dict, const char *key, CFDataRef *outValue) {
    id value = [dict valueForKey:toNSString(key)];
    if ([value isKindOfClass:NSData.class]) {
        *outValue = CFBridgingRetain(value);
        return true;
    }
    else {
        *outValue = nil;
        return false;
    }
}

void KeyValueStoreAppleKeychain_SetString(NSMutableDictionary* dict, const char *key, const char *cStr) {
    [dict setObject:toNSString(cStr) forKey:toNSString(key)];
}

bool KeyValueStoreAppleKeychain_TryGetString(NSMutableDictionary* dict, const char *key, CFDataRef *outValue) {
    id value = [dict valueForKey:toNSString(key)];
    if ([value isKindOfClass:NSString.class]) {
        *outValue = CFBridgingRetain([value dataUsingEncoding:NSUTF16StringEncoding]);
        return true;
    }
    else {
        *outValue = nil;
        return false;
    }
}

const void *KeyValueStoreAppleKeychain_DataGetBytePtr(CFDataRef ref) {
    if (ref) {
        return CFDataGetBytePtr(ref);
    }
    else {
        return NULL;
    }
}

int KeyValueStoreAppleKeychain_DataGetLength(CFDataRef ref) {
    if (ref) {
        return CFDataGetLength(ref);
    }
    else {
        return 0;
    }
}

bool KeyValueStoreAppleKeychain_Save(const GenericPasswordKeychainAttributes *kvs, NSMutableDictionary* dictionary) {
    return setKeychainData(kvs, dictionary);
}

CFTypeRef KeyValueStoreAppleKeychain_Load(const GenericPasswordKeychainAttributes *kvs) {
    NSDictionary* dictionary = getKeychainTypedData(kvs, NSDictionary.class);
    return CFBridgingRetain([NSMutableDictionary dictionaryWithDictionary:dictionary]);
}

bool KeyValueStoreAppleKeychain_DeleteKeychain(const GenericPasswordKeychainAttributes *kvs) {
    return deleteKeychainData(kvs);
}