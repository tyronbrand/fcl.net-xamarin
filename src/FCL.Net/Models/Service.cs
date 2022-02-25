using System;
using System.Collections.Generic;

namespace FCL.Net.Models
{
    public class Service : RequestBase
    {
        public FCLServiceType Type { get; set; }
        public FCLServiceMethod Method { get; set; }
        public Uri Endpoint { get; set; }
        public string Uid { get; set; }
        public string Id { get; set; }
        public Identity Identity { get; set; }
        public Provider Provider { get; set; }
        public Dictionary<string, object> Params { get; set; }
    }
}
