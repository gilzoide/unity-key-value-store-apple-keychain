#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_TVOS || UNITY_VISIONOS
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Gilzoide.KeyValueStore.AppleKeychain
{
    [StructLayout(LayoutKind.Sequential)]
    public class AppleKeychainKeyValueStore : ISavableKeyValueStore, IDisposable
    {
        [field: MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Account { get; set; } = null;

        [field: MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Service { get; set; } = Application.identifier;

        [field: MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Label { get; set; } = null;

        [field: MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Description { get; set; } = null;

        [field: MarshalAs(UnmanagedType.Bool)]
        public bool IsSynchronizable { get; set; } = false;

        [field: MarshalAs(UnmanagedType.Bool)]
        private readonly bool _useDataProtectionKeychain = !Application.isEditor;

        private IntPtr _mutableDictionary = IntPtr.Zero;

        public AppleKeychainKeyValueStore()
        {
            NativeBridge.KeyValueStoreAppleKeychain_AllocDictionary(out _mutableDictionary);
        }

        public void Dispose()
        {
            NativeBridge.KeyValueStoreAppleKeychain_ReleaseDictionary(ref _mutableDictionary);
        }

        public void DeleteAll()
        {
            NativeBridge.KeyValueStoreAppleKeychain_ClearDictionary(_mutableDictionary);
        }

        public void DeleteKey(string key)
        {
            NativeBridge.KeyValueStoreAppleKeychain_DeleteKey(_mutableDictionary, key);
        }

        public bool HasKey(string key)
        {
            return NativeBridge.KeyValueStoreAppleKeychain_HasKey(_mutableDictionary, key);
        }

        public void SetBool(string key, bool value)
        {
            NativeBridge.KeyValueStoreAppleKeychain_SetBool(_mutableDictionary, key, value);
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
            return NativeBridge.KeyValueStoreAppleKeychain_TryGetBool(_mutableDictionary, key, out value);
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

        public void Load()
        {
            NativeBridge.KeyValueStoreAppleKeychain_Load(this, out _mutableDictionary);
        }

        public void Save()
        {
            NativeBridge.KeyValueStoreAppleKeychain_Save(this);
        }

        public void DeleteKeychain()
        {
            NativeBridge.KeyValueStoreAppleKeychain_DeleteKeychain(this);
        }
    }
}
#endif