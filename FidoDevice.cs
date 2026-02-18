using System;

namespace Fido2Authenticator
{
    public sealed unsafe class FidoDevice : IDisposable
    {
        private fido_dev_t* _native;

        static FidoDevice()
        {
            Init.Call();
        }

        public FidoDevice()
        {
            _native = Native.fido_dev_new();
            if (_native == null)
                throw new OutOfMemoryException();
        }

        ~FidoDevice()
        {
            ReleaseUnmanagedResources();
        }

        public static explicit operator fido_dev_t*(FidoDevice dev)
        {
            return dev._native;
        }

        public void Close() => Native.fido_dev_close(_native).Check();
        public void GetAssert(FidoAssertion assert, string pin) => Native.fido_dev_get_assert(_native, (fido_assert_t*)assert, pin).Check();
        public void Open(string path) => Native.fido_dev_open(_native, path).Check();

        private void ReleaseUnmanagedResources()
        {
            var native = _native;
            Native.fido_dev_free(&native);
            _native = null;
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }
    }
}
