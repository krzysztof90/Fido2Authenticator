using Fido2Authenticator.Exceptions;
using Fido2Authenticator.Json;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Fido2Authenticator
{
    public static class Fido2Manager
    {
        public static string GetAuthorizationData<T, Y>(string challengeData, string origin, Func<T, string, string, string, string, Y> createAuthorizationData) where T : Options where Y : AuthorizationData
        {
            Encoding encoding = Encoding.UTF8;

            string challengeDataMain = challengeData.Replace("\n", String.Empty);
            string optionsJson = encoding.GetString(Convert.FromBase64String(challengeDataMain));
            T options = JsonConvert.DeserializeObject<T>(optionsJson);

            ClientData clientData = new ClientData();
            clientData.origin = origin;
            clientData.type = "webauthn.get";
            clientData.challenge = options.EncodeChallenge ? Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(Convert.FromBase64String(options.Challenge)) : options.Challenge;
            clientData.crossOrigin = false;
            string clientDataJson = JsonConvert.SerializeObject(clientData);
            byte[] clientDataBytes = encoding.GetBytes(clientDataJson);

            byte[] authData;
            byte[] signatureData;
            byte[] idData;

            using (FidoDevice dev = new FidoDevice())
            {
                dev.Open("windows://hello");
                using (FidoAssertion assert = new FidoAssertion())
                {
                    assert.SetClientData(clientDataBytes);
                    assert.Rp = options.RelyingParty;
                    foreach (OptionsAllowCredentials allowCredential in options.AllowCredentials)
                        assert.AllowCredential(Microsoft.IdentityModel.Tokens.Base64UrlEncoder.DecodeBytes(allowCredential.id));

                    try
                    {
                        //TODO there is window with chose e.g. phone as authorization method, and warning about source url; in the browser there is right away ask for touch the key
                        //TODO window shows behind main window. If here is set breakpoint then window is in front
                        dev.GetAssert(assert, null);
                    }
                    catch (CtapException)
                    {
                        return null;
                    }

                    FidoAssertionStatement assertion = assert[0];
                    authData = assertion.AuthData.ToArray();
                    signatureData = assertion.Signature.ToArray();
                    idData = assertion.Id.ToArray();
                }
                dev.Close();
            }

            string authenticatorDataEncoded = Convert.ToBase64String(authData);
            string clientDataEncoded = Convert.ToBase64String(clientDataBytes);
            string signatureEncoded = Convert.ToBase64String(signatureData);
            string idEncoded = Convert.ToBase64String(idData);

            if (options.EncodeResult)
            {
                authenticatorDataEncoded = Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(Convert.FromBase64String(authenticatorDataEncoded));
                clientDataEncoded = Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(Convert.FromBase64String(clientDataEncoded));
                signatureEncoded = Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(Convert.FromBase64String(signatureEncoded));
                idEncoded = Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(Convert.FromBase64String(idEncoded));
            }

            //TODO abstract method to Options
            Y authorizationData = createAuthorizationData(options, authenticatorDataEncoded, clientDataEncoded, signatureEncoded, idEncoded);

            string authorizationDataJson = JsonConvert.SerializeObject(authorizationData);
            string authorizationData64 = Convert.ToBase64String(encoding.GetBytes(authorizationDataJson));
            return authorizationData64;
        }
    }
}
