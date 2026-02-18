using System.Runtime.Serialization;

namespace Fido2Authenticator.Json
{
    [DataContract]
    public class ClientData
    {
        [DataMember] public string type { get; set; }
        [DataMember] public string challenge { get; set; }
        [DataMember] public string origin { get; set; }
        //TODO some structures doesn't have this field
        [DataMember] public bool crossOrigin { get; set; }
    }
}
