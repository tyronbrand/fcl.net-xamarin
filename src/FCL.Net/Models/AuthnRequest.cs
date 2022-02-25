namespace FCL.Net.Models
{
    public class AuthnRequest : RequestBase
    {
        public FCLServiceType Type { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
    }
}
