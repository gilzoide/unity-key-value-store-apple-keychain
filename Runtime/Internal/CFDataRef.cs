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
            if (_nativeHandle == IntPtr.Zero)
            {
                return null;
            }
            IntPtr bytes = NativeBridge.CFDataGetBytePtr(this);
            int length = checked((int) NativeBridge.CFDataGetLength(this));
            var value = new byte[length];
            Marshal.Copy(bytes, value, 0, length);
            return value;
        }

        public string GetString()
        {
            if (_nativeHandle == IntPtr.Zero)
            {
                return null;
            }
            IntPtr bytes = NativeBridge.CFDataGetBytePtr(this);
            int length = checked((int) NativeBridge.CFDataGetLength(this));
            return Marshal.PtrToStringUni(bytes, length);
        }

        public void Dispose()
        {
            if (_nativeHandle != IntPtr.Zero)
            {
                NativeBridge.CFRelease(_nativeHandle);
                _nativeHandle = IntPtr.Zero;
            }
        }
    }
}
#endif