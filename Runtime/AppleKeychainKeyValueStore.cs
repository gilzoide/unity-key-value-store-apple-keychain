#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_TVOS || UNITY_VISIONOS
using System.Runtime.InteropServices;
using UnityEngine;

namespace Gilzoide.KeyValueStore.AppleKeychain
{
    [StructLayout(LayoutKind.Sequential)]
    public class AppleKeychainKeyValueStore : IKeyValueStore
    {
        [field: MarshalAs(UnmanagedType.LPUTF8Str)]
        public string ServiceName { get; set; }

        [field: MarshalAs(UnmanagedType.Bool)]
        public bool IsSynchronizable { get; set; }

        [field: MarshalAs(UnmanagedType.Bool)]
        private readonly bool _useDataProtectionKeychain = !Application.isEditor;

        public AppleKeychainKeyValueStore(string serviceName, bool isSynchronizable)
        {
            ServiceName = serviceName;
            IsSynchronizable = isSynchronizable;
        }

        public void DeleteAll()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteKey(string key)
        {
            NativeBridge.KeyValueStoreAppleKeychain_DeleteKey(this, key);
        }

        public bool HasKey(string key)
        {
            return NativeBridge.KeyValueStoreAppleKeychain_HasKey(this, key);
        }

        public void SetBool(string key, bool value)
        {
            NativeBridge.KeyValueStoreAppleKeychain_SetBool(this, key, value);
        }

        public void SetBytes(string key, byte[] value)
        {
            throw new System.NotImplementedException();
        }

        public void SetDouble(string key, double value)
        {
            throw new System.NotImplementedException();
        }

        public void SetFloat(string key, float value)
        {
            throw new System.NotImplementedException();
        }

        public void SetInt(string key, int value)
        {
            throw new System.NotImplementedException();
        }

        public void SetLong(string key, long value)
        {
            throw new System.NotImplementedException();
        }

        public void SetString(string key, string value)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetBool(string key, out bool value)
        {
            return NativeBridge.KeyValueStoreAppleKeychain_TryGetBool(this, key, out value);
        }

        public bool TryGetBytes(string key, out byte[] value)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetDouble(string key, out double value)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetFloat(string key, out float value)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetInt(string key, out int value)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetLong(string key, out long value)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetString(string key, out string value)
        {
            throw new System.NotImplementedException();
        }
    }
}
#endif