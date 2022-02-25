namespace FCL.Net.Models
{
    public class AuthnResponse : RequestBase
    {
        public Status Status { get; set; }
        public Service Updates { get; set; }
        public Service Local { get; set; }
        public AuthnData Data { get; set; }
        public string Reason { get; set; }
        public AuthnData CompositeSignature { get; set; }
        public Service AuthorizationUpdates { get; set; }
    }
}
