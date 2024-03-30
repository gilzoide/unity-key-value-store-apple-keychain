#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_TVOS || UNITY_VISIONOS
using System;
using System.Runtime.InteropServices;

namespace Gilzoide.KeyValueStore.AppleKeychain.Internal
{
    public struct CFDataRef : IDisposable
    {
        private IntPtr _nativeHandle;

        public byte[] GetBytes()
        {
            IntPtr bytes = NativeBridge.KeyValueStoreAppleKeychain_DataGetBytePtr(this);
            int length = NativeBridge.KeyValueStoreAppleKeychain_DataGetLength(this);
            var value = new byte[length];
            Marshal.Copy(bytes, value, 0, length);
            return value;
        }

        public string GetString()
        {
            IntPtr bytes = NativeBridge.KeyValueStoreAppleKeychain_DataGetBytePtr(this);
            int length = NativeBridge.KeyValueStoreAppleKeychain_DataGetLength(this);
            return Marshal.PtrToStringUni(bytes, length);
        }

        public void Dispose()
        {
            NativeBridge.KeyValueStoreAppleKeychain_Release(_nativeHandle);
            _nativeHandle = IntPtr.Zero;
        }
    }
}
#endif