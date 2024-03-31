#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_TVOS || UNITY_VISIONOS
using System;

namespace Gilzoide.KeyValueStore.AppleKeychain.Internal
{
    public struct CFMutableDictionaryRef : IDisposable
    {
        private IntPtr _nativeHandle;

        public static CFMutableDictionaryRef Alloc()
        {
            return NativeBridge.KeyValueStoreAppleKeychain_AllocDictionary();
        }

        public void Dispose()
        {
            if (_nativeHandle != IntPtr.Zero)
            {
                NativeBridge.CFRelease(_nativeHandle);
                _nativeHandle = IntPtr.Zero;
            }
        }

        public void DeleteAll()
        {
            NativeBridge.KeyValueStoreAppleKeychain_ClearDictionary(this);
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
            NativeBridge.KeyValueStoreAppleKeychain_SetBytes(this, key, value, value.Length);
        }

        public void SetDouble(string key, double value)
        {
            NativeBridge.KeyValueStoreAppleKeychain_SetDouble(this, key, value);
        }

        public void SetFloat(string key, float value)
        {
            NativeBridge.KeyValueStoreAppleKeychain_SetFloat(this, key, value);
        }

        public void SetInt(string key, int value)
        {
            NativeBridge.KeyValueStoreAppleKeychain_SetInt(this, key, value);
        }

        public void SetLong(string key, long value)
        {
            NativeBridge.KeyValueStoreAppleKeychain_SetLong(this, key, value);
        }

        public void SetString(string key, string value)
        {
            NativeBridge.KeyValueStoreAppleKeychain_SetString(this, key, value);
        }

        public bool TryGetBool(string key, out bool value)
        {
            return NativeBridge.KeyValueStoreAppleKeychain_TryGetBool(this, key, out value);
        }

        public bool TryGetBytes(string key, out byte[] value)
        {
            if (NativeBridge.KeyValueStoreAppleKeychain_TryGetBytes(this, key, out CFDataRef cfdata))
            {
                using (cfdata)
                {
                    value = cfdata.GetBytes();
                    return true;
                }
            }
            else
            {
                value = null;
                return false;
            }
        }

        public bool TryGetDouble(string key, out double value)
        {
            return NativeBridge.KeyValueStoreAppleKeychain_TryGetDouble(this, key, out value);
        }

        public bool TryGetFloat(string key, out float value)
        {
            return NativeBridge.KeyValueStoreAppleKeychain_TryGetFloat(this, key, out value);
        }

        public bool TryGetInt(string key, out int value)
        {
            return NativeBridge.KeyValueStoreAppleKeychain_TryGetInt(this, key, out value);
        }

        public bool TryGetLong(string key, out long value)
        {
            return NativeBridge.KeyValueStoreAppleKeychain_TryGetLong(this, key, out value);
        }

        public bool TryGetString(string key, out string value)
        {
            if (NativeBridge.KeyValueStoreAppleKeychain_TryGetString(this, key, out CFDataRef cfdata))
            {
                using (cfdata)
                {
                    value = cfdata.GetString();
                    return true;
                }
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
}
#endif