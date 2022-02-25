using System;

namespace FCL.Net.Models
{
    public class FlowUser : RequestBase
    {       
        public override string F_Type { get; set; } = "User";
        public override string F_Vsn { get; set; } = "1.0.0";
        public string Addr { get; set; }
        public bool LoggedIn { get; set; }
        public string Cid { get; set; }
        public DateTime ExpiresAt { get; set; }
        public Service[] Services { get; set; }
    }
}
