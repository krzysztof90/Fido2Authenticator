using Fido2Authenticator.Enums;
using Fido2Authenticator.Exceptions;

namespace Fido2Authenticator
{
    internal static class Extensions
    {
        public static void Check(this int err)
        {
            if (err > (int)CtapStatus.Ok)
                throw new CtapException((CtapStatus)err);

            if (err < (int)FidoStatus.Ok)
                throw new FidoException((FidoStatus)err);
        }
    }
}
