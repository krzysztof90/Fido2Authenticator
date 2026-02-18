using System;

namespace Fido2Authenticator
{
    public sealed unsafe class FidoAssertion : IDisposable
    {
        private fido_assert_t* _native;

        static FidoAssertion()
        {
            Init.Call();
        }

        public FidoAssertionStatement this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException();

                return new FidoAssertionStatement(_native, index);
            }
        }

        public int Count
        {
            get => (int)Native.fido_assert_count(_native);
            set => Native.fido_assert_set_count(_native, (UIntPtr)value).Check();
        }

        public string Rp
        {
            get => Native.fido_assert_rp_id(_native);
            set => Native.fido_assert_set_rp(_native, value).Check();
        }

        public FidoAssertion()
        {
            _native = Native.fido_assert_new();
            if (_native == null)
                throw new OutOfMemoryException();
        }

        ~FidoAssertion()
        {
            ReleaseUnmanagedResources();
        }

        public static explicit operator fido_assert_t*(FidoAssertion assert)
        {
            return assert._native;
        }

        public void AllowCredential(ReadOnlySpan<byte> credentialId)
        {
            fixed (byte* cred = credentialId)
            {
                Native.fido_assert_allow_cred(_native, cred, (UIntPtr)credentialId.Length).Check();
            }
        }

        public void SetClientData(ReadOnlySpan<byte> clientData)
        {
            fixed (byte* clientData_ = clientData)
            {
                Native.fido_assert_set_clientdata(_native, clientData_, (UIntPtr)clientData.Length).Check();
            }
        }

        private void ReleaseUnmanagedResources()
        {
            var native = _native;
            Native.fido_assert_free(&native);
            _native = null;
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }
    }
}
