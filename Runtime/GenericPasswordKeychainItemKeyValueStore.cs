#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_TVOS || UNITY_VISIONOS
using System;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Gilzoide.KeyValueStore.AppleKeychain
{
    public class GenericPasswordKeychainItemKeyValueStore : ISavableKeyValueStore, IDisposable
    {
        public GenericPasswordKeychainAttributes KeychainAttributes { get; }
        public bool SaveOnDispose { get; set; } = true;

        private IntPtr _mutableDictionary = IntPtr.Zero;

        public GenericPasswordKeychainItemKeyValueStore() : this(new GenericPasswordKeychainAttributes())
        {
        }

        public GenericPasswordKeychainItemKeyValueStore(GenericPasswordKeychainAttributes attributes, bool autoload = true)
        {
            if (attributes == null)
            {
                throw new ArgumentNullException(nameof(attributes));
            }
            KeychainAttributes = attributes;
            _mutableDictionary = NativeBridge.KeyValueStoreAppleKeychain_AllocDictionary();
            if (autoload)
            {
                Load();
            }
        }

        ~GenericPasswordKeychainItemKeyValueStore()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (SaveOnDispose)
            {
                Save();
            }
            NativeBridge.KeyValueStoreAppleKeychain_Release(_mutableDictionary);
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
            unsafe
            {
                fixed (void* ptr = value)
                {
                    NativeBridge.KeyValueStoreAppleKeychain_SetData(_mutableDictionary, key, ptr, value.Length);
                }
            }
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
            unsafe
            {
                fixed (void* ptr = value)
                {
                    NativeBridge.KeyValueStoreAppleKeychain_SetData(_mutableDictionary, key, ptr, value.Length * sizeof(char));
                }
            }
        }

        public bool TryGetBool(string key, out bool value)
        {
            return NativeBridge.KeyValueStoreAppleKeychain_TryGetBool(_mutableDictionary, key, out value);
        }

        public bool TryGetBytes(string key, out byte[] value)
        {
            if (NativeBridge.KeyValueStoreAppleKeychain_TryGetData(_mutableDictionary, key, out IntPtr cfdata))
            {
                unsafe
                {
                    void* bytes = NativeBridge.KeyValueStoreAppleKeychain_DataGetBytePtr(cfdata);
                    int length = NativeBridge.KeyValueStoreAppleKeychain_DataGetLength(cfdata);
                    value = new byte[length];
                    fixed (void* valuePtr = value)
                    {
                        UnsafeUtility.MemCpy(valuePtr, bytes, length);
                    }
                }
                NativeBridge.KeyValueStoreAppleKeychain_Release(cfdata);
                return true;
            }
            else
            {
                value = null;
                return false;
            }
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
            if (NativeBridge.KeyValueStoreAppleKeychain_TryGetData(_mutableDictionary, key, out IntPtr cfdata))
            {
                unsafe
                {
                    void* bytes = NativeBridge.KeyValueStoreAppleKeychain_DataGetBytePtr(cfdata);
                    int length = NativeBridge.KeyValueStoreAppleKeychain_DataGetLength(cfdata);
                    value = Marshal.PtrToStringUni((IntPtr) bytes, length);
                }
                NativeBridge.KeyValueStoreAppleKeychain_Release(cfdata);
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        public void Load()
        {
            NativeBridge.KeyValueStoreAppleKeychain_Release(_mutableDictionary);
            _mutableDictionary = NativeBridge.KeyValueStoreAppleKeychain_Load(KeychainAttributes);
        }

        public void Save()
        {
            NativeBridge.KeyValueStoreAppleKeychain_Save(KeychainAttributes, _mutableDictionary);
        }

        public void DeleteKeychain()
        {
            NativeBridge.KeyValueStoreAppleKeychain_DeleteKeychain(KeychainAttributes);
        }
    }
}
#endif