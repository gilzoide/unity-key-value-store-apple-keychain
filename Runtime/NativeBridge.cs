using System;
using System.Runtime.InteropServices;

namespace Gilzoide.KeyValueStore.AppleKeychain
{
    public static class NativeBridge
    {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_TVOS || UNITY_VISIONOS)
        public const string LibraryName = "__Internal";
#else
        public const string LibraryName = "KeyValueStoreAppleKeychain";
#endif

        // Alloc/Release NSMutableDictionary
        [DllImport(LibraryName)]
        public static extern IntPtr KeyValueStoreAppleKeychain_AllocDictionary();

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_ReleaseDictionary(IntPtr mutableDictionary);

        // Read/Write internal NSMutableDictionary
        [DllImport(LibraryName)]
        public static extern void KeyValueStoreAppleKeychain_ClearDictionary(IntPtr mutableDictionary);

        [DllImport(LibraryName)]
        public static extern void KeyValueStoreAppleKeychain_DeleteKey(IntPtr mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_HasKey(IntPtr mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key);

        [DllImport(LibraryName)]
        public static extern void KeyValueStoreAppleKeychain_SetBool(IntPtr mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, [MarshalAs(UnmanagedType.Bool)] bool value);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_TryGetBool(IntPtr mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, [MarshalAs(UnmanagedType.Bool)] out bool value);

        [DllImport(LibraryName)]
        public static extern void KeyValueStoreAppleKeychain_SetInt(IntPtr mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, int value);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_TryGetInt(IntPtr mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, out int value);

        [DllImport(LibraryName)]
        public static extern void KeyValueStoreAppleKeychain_SetLong(IntPtr mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, long value);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_TryGetLong(IntPtr mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, out long value);

        [DllImport(LibraryName)]
        public static extern void KeyValueStoreAppleKeychain_SetFloat(IntPtr mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, float value);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_TryGetFloat(IntPtr mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, out float value);

        [DllImport(LibraryName)]
        public static extern void KeyValueStoreAppleKeychain_SetDouble(IntPtr mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, double value);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_TryGetDouble(IntPtr mutableDictionary, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, out double value);

        // Read/Write Keychain
        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_Save([In] AppleKeychainKeyValueStore kvs);

        [DllImport(LibraryName)]
        public static extern IntPtr KeyValueStoreAppleKeychain_Load([In] AppleKeychainKeyValueStore kvs);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_DeleteKeychain([In] AppleKeychainKeyValueStore kvs);
    }
}
