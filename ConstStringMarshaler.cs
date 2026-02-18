using System;
using System.Runtime.InteropServices;

namespace Fido2Authenticator
{
    public sealed class ConstStringMarshaler : ICustomMarshaler
    {
        private static readonly ConstStringMarshaler Instance = new ConstStringMarshaler();

        public static ICustomMarshaler GetInstance(string cookie) => Instance;

        public void CleanUpManagedData(object ManagedObj)
        {
            throw new NotImplementedException();
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
        }

        public int GetNativeDataSize()
        {
            return IntPtr.Size;
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            throw new NotImplementedException();
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            return Marshal.PtrToStringAnsi(pNativeData);
        }
    }
}
