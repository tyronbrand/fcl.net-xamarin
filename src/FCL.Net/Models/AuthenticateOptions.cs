using System.Net.Http;

namespace FCL.Net.Models
{
    public class AuthenticateParams
    {
        public AuthnResponse AuthnResponse { get; set; }
        public string RedirectUri { get; set; }
        public double TimerIntervalMs { get; set; } = 1000;
        public int TimerTimeoutMs { get; set; } = 240000;
        public FclOptions Options { get; set; }
        public HttpClient HttpClient { get; set; }
    }
}
