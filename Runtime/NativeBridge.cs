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
        public static extern bool KeyValueStoreAppleKeychain_HasKey(
            [In] AppleKeychainKeyValueStore kvs,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string key
        );

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_SetBool(
            [In] AppleKeychainKeyValueStore kvs,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string key,
            [MarshalAs(UnmanagedType.Bool)] bool value
        );

        [DllImport(LibraryName)]
        public static extern bool KeyValueStoreAppleKeychain_TryGetBool(
            [In] AppleKeychainKeyValueStore kvs,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string key,
            [MarshalAs(UnmanagedType.Bool)] out bool value
        );
    }
}
