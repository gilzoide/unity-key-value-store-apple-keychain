#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_TVOS || UNITY_VISIONOS
using System;
using System.Runtime.InteropServices;

namespace Gilzoide.KeyValueStore.AppleKeychain.Internal
{
    public static class NativeBridge
    {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_TVOS || UNITY_VISIONOS)
        public const string LibraryName = "__Internal";
#else
        public const string LibraryName = "KeyValueStoreAppleKeychain";
#endif

        // Alloc/Release CFMutableDictionaryRef
        [DllImport(LibraryName)]
        public static extern CFMutableDictionaryRef KeyValueStoreAppleKeychain_AllocDictionary();

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_Release(IntPtr cftyperef);

        // Read CFDataRef
        [DllImport(LibraryName)]
        public static extern IntPtr KeyValueStoreAppleKeychain_DataGetBytePtr(CFDataRef cfdata);

        [DllImport(LibraryName)]
        public static extern int KeyValueStoreAppleKeychain_DataGetLength(CFDataRef cfdata);

        // Read/Write internal NSMutableDictionary
        [DllImport(LibraryName)]
        public static extern void KeyValueStoreAppleKeychain_ClearDictionary(CFMutableDictionaryRef mutableDictionary);

        [DllImport(LibraryName)]
        public static extern void KeyValueStoreAppleKeychain_DeleteKey(CFMutableDictionaryRef mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_HasKey(CFMutableDictionaryRef mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key);

        [DllImport(LibraryName)]
        public static extern void KeyValueStoreAppleKeychain_SetBool(CFMutableDictionaryRef mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, [MarshalAs(UnmanagedType.Bool)] bool value);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_TryGetBool(CFMutableDictionaryRef mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, [MarshalAs(UnmanagedType.Bool)] out bool value);

        [DllImport(LibraryName)]
        public static extern void KeyValueStoreAppleKeychain_SetInt(CFMutableDictionaryRef mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, int value);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_TryGetInt(CFMutableDictionaryRef mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, out int value);

        [DllImport(LibraryName)]
        public static extern void KeyValueStoreAppleKeychain_SetLong(CFMutableDictionaryRef mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, long value);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_TryGetLong(CFMutableDictionaryRef mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, out long value);

        [DllImport(LibraryName)]
        public static extern void KeyValueStoreAppleKeychain_SetFloat(CFMutableDictionaryRef mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, float value);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_TryGetFloat(CFMutableDictionaryRef mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, out float value);

        [DllImport(LibraryName)]
        public static extern void KeyValueStoreAppleKeychain_SetDouble(CFMutableDictionaryRef mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, double value);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_TryGetDouble(CFMutableDictionaryRef mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, out double value);

        [DllImport(LibraryName)]
        public static extern unsafe void KeyValueStoreAppleKeychain_SetData(CFMutableDictionaryRef mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, void* bytes, int length);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_TryGetData(CFMutableDictionaryRef mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, out CFDataRef cfdata);

        // Read/Write Keychain
        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_Save([In] GenericPasswordKeychainAttributes kvs, CFMutableDictionaryRef dictionary);

        [DllImport(LibraryName)]
        public static extern CFMutableDictionaryRef KeyValueStoreAppleKeychain_Load([In] GenericPasswordKeychainAttributes kvs);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_DeleteKeychain([In] GenericPasswordKeychainAttributes kvs);
    }
}
#endif