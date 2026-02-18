using Fido2Authenticator.Enums;
using System;
using System.Runtime.InteropServices;

namespace Fido2Authenticator
{
    public static unsafe partial class Native
    {
        //TODO make it dynamically, now dll needs to be copied to bin
        private const string DllName = "fido2";

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fido_init(int flags);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern fido_dev_t* fido_dev_new();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_close(fido_dev_t* dev);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_get_assert(fido_dev_t* dev, fido_assert_t* assert, string pin);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_open(fido_dev_t* dev, string path);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fido_dev_free(fido_dev_t** dev_p);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern fido_assert_t* fido_assert_new();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_assert_allow_cred(fido_assert_t* assert, byte* ptr, UIntPtr len);
        
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_assert_set_clientdata(fido_assert_t* assert, byte* data, UIntPtr len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fido_assert_free(fido_assert_t** assert_p);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* fido_assert_authdata_ptr(fido_assert_t* assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_assert_authdata_len(fido_assert_t* assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* fido_assert_authdata_raw_ptr(fido_assert_t* assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_assert_authdata_raw_len(fido_assert_t* assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FidoAuthFlags fido_assert_flags(fido_assert_t* assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* fido_assert_hmac_secret_ptr(fido_assert_t* assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_assert_hmac_secret_len(fido_assert_t* assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* fido_assert_id_ptr(fido_assert_t* assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_assert_id_len(fido_assert_t* assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* fido_assert_sig_ptr(fido_assert_t* assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_assert_sig_len(fido_assert_t* assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
        public static extern string fido_assert_user_display_name(fido_assert_t* assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
        public static extern string fido_assert_user_icon(fido_assert_t* assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
        public static extern string fido_assert_user_name(fido_assert_t* assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* fido_assert_user_id_ptr(fido_assert_t* assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_assert_user_id_len(fido_assert_t* assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_assert_count(fido_assert_t* assert);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_assert_set_count(fido_assert_t* assert, UIntPtr n);
        
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
        public static extern string fido_assert_rp_id(fido_assert_t* assert);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_assert_set_rp(fido_assert_t* assert, string id);
    }

    public struct fido_assert_t
    {
    }

    public struct fido_dev_t
    {
    }
}
