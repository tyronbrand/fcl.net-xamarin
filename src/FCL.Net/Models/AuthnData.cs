namespace FCL.Net.Models
{
    public class AuthnData : RequestBase
    {
        public string Addr { get; set; }        
        public Service[] Services { get; set; }
        public Service Proposer { get; set; }
        public Service[] Payer { get; set; }
        public Service[] Authorization { get; set; }
        public string Signature { get; set; }
    }
}
