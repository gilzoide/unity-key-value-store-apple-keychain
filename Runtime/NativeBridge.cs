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

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_AllocDictionary(out IntPtr mutableDictionary);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_ReleaseDictionary(ref IntPtr mutableDictionary);

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
        public static extern bool KeyValueStoreAppleKeychain_Save([In] AppleKeychainKeyValueStore kvs);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_Load([In] AppleKeychainKeyValueStore kvs, out IntPtr mutableDictionary);

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_DeleteKeychain([In] AppleKeychainKeyValueStore kvs);
    }
}
