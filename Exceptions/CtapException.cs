using Fido2Authenticator.Enums;
using System;

namespace Fido2Authenticator.Exceptions
{
    public sealed class CtapException : Exception
    {
        public CtapStatus Code { get; }

        public CtapException(CtapStatus err) : base($"CTAP response indicated non-success status ({err})")
        {
            Code = err;
        }
    }
}
