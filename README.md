# Fido2Authenticator
Fido2 authentication for Windows desktop applications

Based on https://github.com/borrrden/Fido2Net

Needs `Fido2.dll` file from https://github.com/Yubico/libfido2 in executable (bin) folder

=== Example of use

  $ string authorizationData = Fido2Manager.GetAuthorizationData<Fido2Options, Fido2AuthorizationData>(challengeData, "https://login.ingbank.pl", createAuthorizationDataMethod);

=== How to find the structure of `Options` and `AuthorizationData`
Look at Http request and response in the browser
let `challengeData` come from response, `authorizationData` from request
e.g.
  $ string challengeData = "eyJjaGFsbGVuZ2UiOiJVdzBqT3RyeUhBdTBPLXBqcm1LeDNJZjE3NFhRS2xsa2I0Z2dpeUNQSE9FIiwiaGludHMiOltdLCJycElkIjoibG9naW4uaW5nYmFuay5wbCIsImFsbG93Q3JlZGVudGlhbHMiOlt7InR5cGUiOiJwdWJsaWMta2V5IiwiaWQiOiI2aGxFM0EwdXJlb2RXSXozTzFuZDN0MmprN0lqTnR3X1NPX2JSNjV5TlhSTFJVWWFmd3hFV1NTc2lwSWVmM3Axb29EU0tCM0RtSG5QVXl6ZUI0bTZxQSIsInRyYW5zcG9ydHMiOlsidXNiIl19XSwiZXh0ZW5zaW9ucyI6e319";
  $ string authorizationData = "eyJ0eXBlIjoicHVibGljLWtleSIsImlkIjoiNmhsRTNBMHVyZW9kV0l6M08xbmQzdDJqazdJak50d19TT19iUjY1eU5YUkxSVVlhZnd4RVdTU3NpcEllZjNwMW9vRFNLQjNEbUhuUFV5emVCNG02cUEiLCJyYXdJZCI6IjZobEUzQTB1cmVvZFdJejNPMW5kM3Qyams3SWpOdHdfU09fYlI2NXlOWFJMUlVZYWZ3eEVXU1NzaXBJZWYzcDFvb0RTS0IzRG1IblBVeXplQjRtNnFBIiwicmVzcG9uc2UiOnsiY2xpZW50RGF0YUpTT04iOiJleUowZVhCbElqb2lkMlZpWVhWMGFHNHVaMlYwSWl3aVkyaGhiR3hsYm1kbElqb2lWWGN3YWs5MGNubElRWFV3VHkxd2FuSnRTM2d6U1dZeE56UllVVXRzYkd0aU5HZG5hWGxEVUVoUFJTSXNJbTl5YVdkcGJpSTZJbWgwZEhCek9pOHZiRzluYVc0dWFXNW5ZbUZ1YXk1d2JDSXNJbU55YjNOelQzSnBaMmx1SWpwbVlXeHpaWDAiLCJhdXRoZW50aWNhdG9yRGF0YSI6IlRQM1dyRl9KRXQtX3huZXhjMGIzeTNSRENxN3l2Z0RlLXNCNUs5cWVqeHBGQUFBQUJ3Iiwic2lnbmF0dXJlIjoiZWdZQ3lRQ253dXhNajR6ckZIOXNpQU13UG4tMDRZbFpaRFEyWTBoMzdCWjN2OG5aWGdJaEE3WktOcVJYXy1CTXdabVZKTW5DVmptRWQ0NHVmanA2bEM2N2h3VEt5WFpXIiwidXNlckhhbmRsZSI6bnVsbH0sImNsaWVudEV4dGVuc2lvblJlc3VsdHMiOnsiYXBwaWQiOm51bGwsImFwcGlkRXhjbHVkZSI6bnVsbCwiY3JlZFByb3BzIjpudWxsfX0=";

Make sure `challengeData` doesn't have breaklines
  $ string challengeDataMain = challengeData.Replace("\n", String.Empty);

  $ string optionsJson = Encoding.UTF8.GetString(Convert.FromBase64String(challengeDataMain));

Create `Options` based on `optionsJson`

  $ string authorizationDataJson = Encoding.UTF8.GetString(Convert.FromBase64String(authorizationData));

Create `AuthorizationData` based on `authorizationDataJson`
In `createAuthorizationDataMethod` fill `AuthorizationData` data from parameters. In this case there are:
* `type` as constant value
* `id` from `id` parameter
* `rawId` from `id` parameter
* `response.authenticatorData` from `authenticatorData` parameter
* `response.clientDataJSON` from `clientDataJSON` parameter
* `response.signature` from `signature` parameter
There can be also cases when `authorization` needs to be filled from options parameter

Check if fields like `authenticatorData`, `clientDataJSON`, `signature` from `authorizationDataJson` contain
* '-' and '_': set `Options.EncodeResult => true;`
* '+' and '/' (this type is characterized also by '=' at the end): set `Options.EncodeResult => false;`

Check if Options.Challenge value contains
'-' and '_': set `Options.EncodeChallenge => false;`
'+' and '/' (this type is characterized also by '=' at the end): set `Options.EncodeChallenge => true;`

set `Options.Challenge => challenge;`
set `Options.AllowCredentials => allowCredentials;`
set `Options.RelyingParty => rpId;`
Depending on the structure these values can also come for example from `data.publicKey.challenge`, `data.publicKey.allowCredentials`, `data.publicKey.rpId`

The final declaration would be like that

  $ [DataContract] public class Fido2Options : Options
  $ {
  $     [DataMember] public string challenge { get; set; }
  $     [DataMember] public List<string> hints { get; set; }
  $     [DataMember] public string rpId { get; set; }
  $     [DataMember] public List<OptionsAllowCredentials> allowCredentials { get; set; }
  $     [DataMember] public OptionsExtensions extensions { get; set; }
  $      
  $     public override bool EncodeChallenge => false;
  $     public override string Challenge => challenge;
  $     public override bool EncodeResult => true;
  $     public override IEnumerable<OptionsAllowCredentials> AllowCredentials => allowCredentials;
  $     public override string RelyingParty => rpId;
  $ }
  $ 
  $ [DataContract] public class OptionsExtensions
  $ {
  $ }
  $ 
  $ [DataContract] public class Fido2AuthorizationData : AuthorizationData
  $ {
  $     [DataMember] public string type { get; set; }
  $     [DataMember] public string id { get; set; }
  $     [DataMember] public string rawId { get; set; }
  $     [DataMember] public AuthorizationDataResponse response { get; set; }
  $     [DataMember] public AuthorizationDataClientExtensionResults clientExtensionResults { get; set; }
  $ }
  $ 
  $ [DataContract] public class AuthorizationDataResponse
  $ {
  $     [DataMember] public string clientDataJSON { get; set; }
  $     [DataMember] public string authenticatorData { get; set; }
  $     [DataMember] public string signature { get; set; }
  $     [DataMember] public string userHandle { get; set; }
  $ }
  $ 
  $ [DataContract] public class AuthorizationDataClientExtensionResults
  $ {
  $     [DataMember] public string appid { get; set; }
  $     [DataMember] public string appidExclude { get; set; }
  $     [DataMember] public string credProps { get; set; }
  $ }
  $ 
  $ Func<Fido2Options, string, string, string, string, Fido2AuthorizationData> createAuthorizationDataMethod = (Fido2Options options, string authenticatorData, string clientDataJSON, string signature, string id) =>
  $ {
  $     Fido2AuthorizationData authorization = new Fido2AuthorizationData();
  $     authorization.type = "public-key";
  $     authorization.id = id;
  $     authorization.rawId = id;
  $     authorization.response = new AuthorizationDataResponse()
  $     {
  $         authenticatorData = authenticatorData,
  $         clientDataJSON = clientDataJSON,
  $         signature = signature,
  $         userHandle = null,
  $     };
  $     authorization.clientExtensionResults = new AuthorizationDataClientExtensionResults()
  $     {
  $         appid = null,
  $         appidExclude = null,
  $         credProps = null,
  $     };
  $     return authorization;
  $ }
