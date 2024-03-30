using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Gilzoide.KeyValueStore.AppleKeychain
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public class GenericPasswordKeychainAttributes
    {
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Account = null;

        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Service = null;

        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string AccessGroup = null;

        [Tooltip("User-visible label for this keychain item")]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Label = null;

        [Tooltip("User-visible string describing this kind of keychain item")]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Description = null;

        [Tooltip("Whether the keychain item in question is synchronized to other devices through iCloud")]
        [MarshalAs(UnmanagedType.I1)]
        public bool Synchronizable = false;

        [Tooltip("Set the value for this key to true when accessing a macOS keychain item that behaves like an iOS keychain item. "
            + "Affects operations only in macOS, other platforms automatically behave as if the key is set to true.")]
        [MarshalAs(UnmanagedType.I1)]
        public bool UseDataProtectionKeychain = false;
    }
}
