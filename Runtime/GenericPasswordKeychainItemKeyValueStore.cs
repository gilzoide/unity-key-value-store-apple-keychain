#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_TVOS || UNITY_VISIONOS
using System;
using Gilzoide.KeyValueStore.AppleKeychain.Internal;

namespace Gilzoide.KeyValueStore.AppleKeychain
{
    public class GenericPasswordKeychainItemKeyValueStore : ISavableKeyValueStore, IDisposable
    {
        public GenericPasswordKeychainAttributes KeychainAttributes { get; }
        public bool SaveOnDispose { get; set; } = true;

        private CFMutableDictionaryRef _mutableDictionary;

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
            _mutableDictionary = CFMutableDictionaryRef.Alloc();
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
            _mutableDictionary.Dispose();
        }

        public void DeleteAll()
        {
            _mutableDictionary.DeleteAll();
        }

        public void DeleteKey(string key)
        {
            _mutableDictionary.DeleteKey(key);
        }

        public bool HasKey(string key)
        {
            return _mutableDictionary.HasKey(key);
        }

        public void SetBool(string key, bool value)
        {
            _mutableDictionary.SetBool(key, value);
        }

        public void SetBytes(string key, byte[] value)
        {
            _mutableDictionary.SetBytes(key, value);
        }

        public void SetDouble(string key, double value)
        {
            _mutableDictionary.SetDouble(key, value);
        }

        public void SetFloat(string key, float value)
        {
            _mutableDictionary.SetFloat(key, value);
        }

        public void SetInt(string key, int value)
        {
            _mutableDictionary.SetInt(key, value);
        }

        public void SetLong(string key, long value)
        {
            _mutableDictionary.SetLong(key, value);
        }

        public void SetString(string key, string value)
        {
            _mutableDictionary.SetString(key, value);
        }

        public bool TryGetBool(string key, out bool value)
        {
            return _mutableDictionary.TryGetBool(key, out value);
        }

        public bool TryGetBytes(string key, out byte[] value)
        {
            return _mutableDictionary.TryGetBytes(key, out value);
        }

        public bool TryGetDouble(string key, out double value)
        {
            return _mutableDictionary.TryGetDouble(key, out value);
        }

        public bool TryGetFloat(string key, out float value)
        {
            return _mutableDictionary.TryGetFloat(key, out value);
        }

        public bool TryGetInt(string key, out int value)
        {
            return _mutableDictionary.TryGetInt(key, out value);
        }

        public bool TryGetLong(string key, out long value)
        {
            return _mutableDictionary.TryGetLong(key, out value);
        }

        public bool TryGetString(string key, out string value)
        {
            return _mutableDictionary.TryGetString(key, out value);
        }

        public void Load()
        {
            _mutableDictionary.Dispose();
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