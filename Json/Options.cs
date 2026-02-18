using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fido2Authenticator.Json
{
    [DataContract]
    public abstract class Options
    {
        public abstract bool EncodeChallenge { get; }
        public abstract string Challenge { get; }
        public abstract bool EncodeResult { get; }
        public abstract IEnumerable<OptionsAllowCredentials> AllowCredentials { get; }
        public abstract string RelyingParty { get; }
    }

    [DataContract]
    public class OptionsAllowCredentials
    {
        [DataMember] public string id { get; set; }
        [DataMember] public string type { get; set; }
        [DataMember] public List<string> transports { get; set; }
    }
}
