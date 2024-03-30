using System;
using System.Runtime.InteropServices;

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

        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Label = null;

        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Description = null;

        [MarshalAs(UnmanagedType.I1)]
        public bool Synchronizable = false;

        [MarshalAs(UnmanagedType.I1)]
        public bool UseDataProtectionKeychain = false;
    }
}
