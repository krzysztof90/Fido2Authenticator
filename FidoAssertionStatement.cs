using Fido2Authenticator.Enums;
using System;

namespace Fido2Authenticator
{
    public unsafe ref struct FidoAssertionStatement
    {
        public ReadOnlySpan<byte> AuthData { get; }
        public FidoAuthFlags Flags { get; }
        public ReadOnlySpan<byte> HmacSecret { get; }
        public ReadOnlySpan<byte> Id { get; }
        public ReadOnlySpan<byte> Signature { get; }
        public string UserDisplayName { get; }
        public string UserIcon { get; }
        public ReadOnlySpan<byte> UserId { get; }
        public string UserName { get; }

        internal FidoAssertionStatement(fido_assert_t* native, int index)
        {
            UIntPtr idx = (UIntPtr)index;
            //AuthData = new ReadOnlySpan<byte>(
            //    Native.fido_assert_authdata_ptr(native, idx),
            //    (int)Native.fido_assert_authdata_len(native, idx));
            AuthData = new ReadOnlySpan<byte>(
                Native.fido_assert_authdata_raw_ptr(native, idx),
                (int)Native.fido_assert_authdata_raw_len(native, idx));
            Flags = Native.fido_assert_flags(native, idx);
            HmacSecret = new ReadOnlySpan<byte>(
                Native.fido_assert_hmac_secret_ptr(native, idx),
                (int)Native.fido_assert_hmac_secret_len(native, idx));
            Id = new ReadOnlySpan<byte>(
                Native.fido_assert_id_ptr(native, idx),
                (int)Native.fido_assert_id_len(native, idx));
            Signature = new ReadOnlySpan<byte>(
                Native.fido_assert_sig_ptr(native, idx),
                (int)Native.fido_assert_sig_len(native, idx));
            UserDisplayName = Native.fido_assert_user_display_name(native, idx);
            UserIcon = Native.fido_assert_user_icon(native, idx);
            UserName = Native.fido_assert_user_name(native, idx);
            UserId = new ReadOnlySpan<byte>(
                Native.fido_assert_user_id_ptr(native, idx),
                (int)Native.fido_assert_user_id_len(native, idx));
        }
    }
}
