using Fido2Authenticator.Enums;
using System;

namespace Fido2Authenticator.Exceptions
{
    public sealed class FidoException : Exception
    {
        public FidoStatus Code { get; set; }

        public FidoException(FidoStatus code) : base($"FIDO2 operation failed ({code})")
        {
            Code = code;
        }
    }
}
