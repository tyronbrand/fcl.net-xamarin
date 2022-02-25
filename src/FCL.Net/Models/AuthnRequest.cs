namespace FCL.Net.Models
{
    public class AuthnRequest : RequestBase
    {
        public FclServiceType Type { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
    }
}
