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
            _mutableDictionary = NativeBridge.KeyValueStoreAppleKeychain_AllocDictionary();
        }

        ~AppleKeychainKeyValueStore()
        {
            Dispose();
        }

        public void Dispose()
        {
            NativeBridge.KeyValueStoreAppleKeychain_ReleaseDictionary(_mutableDictionary);
            _mutableDictionary = IntPtr.Zero;
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
            NativeBridge.KeyValueStoreAppleKeychain_SetDouble(_mutableDictionary, key, value);
        }

        public void SetFloat(string key, float value)
        {
            NativeBridge.KeyValueStoreAppleKeychain_SetFloat(_mutableDictionary, key, value);
        }

        public void SetInt(string key, int value)
        {
            NativeBridge.KeyValueStoreAppleKeychain_SetInt(_mutableDictionary, key, value);
        }

        public void SetLong(string key, long value)
        {
            NativeBridge.KeyValueStoreAppleKeychain_SetLong(_mutableDictionary, key, value);
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
            return NativeBridge.KeyValueStoreAppleKeychain_TryGetDouble(_mutableDictionary, key, out value);
        }

        public bool TryGetFloat(string key, out float value)
        {
            return NativeBridge.KeyValueStoreAppleKeychain_TryGetFloat(_mutableDictionary, key, out value);
        }

        public bool TryGetInt(string key, out int value)
        {
            return NativeBridge.KeyValueStoreAppleKeychain_TryGetInt(_mutableDictionary, key, out value);
        }

        public bool TryGetLong(string key, out long value)
        {
            return NativeBridge.KeyValueStoreAppleKeychain_TryGetLong(_mutableDictionary, key, out value);
        }

        public bool TryGetString(string key, out string value)
        {
            throw new System.NotImplementedException();
        }

        public void Load()
        {
            NativeBridge.KeyValueStoreAppleKeychain_ReleaseDictionary(_mutableDictionary);
            _mutableDictionary = NativeBridge.KeyValueStoreAppleKeychain_Load(this);
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