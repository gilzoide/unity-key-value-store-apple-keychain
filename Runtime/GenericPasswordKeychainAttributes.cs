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
        public string Label = null;

        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Description = null;

        [MarshalAs(UnmanagedType.Bool)]
        public bool IsSynchronizable = false;

        [MarshalAs(UnmanagedType.Bool)]
        private readonly bool _useDataProtectionKeychain = !Application.isEditor;
    }
}
