using Fido2Authenticator.Enums;

namespace Fido2Authenticator
{
    internal static class Init
    {
        private static bool _called;

        public static void Call()
        {
            if (!_called)
            {
                _called = true;
                Native.fido_init((int)FidoFlags.Debug);
            }
        }
    }
}
