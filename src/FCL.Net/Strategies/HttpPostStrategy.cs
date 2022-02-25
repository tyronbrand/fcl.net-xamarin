using FCL.Net.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FCL.Net.Strategy
{
    public class HttpPostStrategy
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly FclOptions _options;

        public HttpPostStrategy(FclOptions options)
        {
            _options = options;
        }

        public async Task<FclAuthServiceResponse> ExecHttpPostAsync()
        {
            var json = JsonConvert.SerializeObject(new AuthnRequest
            {
                F_Type = "Service",
                F_Vsn = "1.0.0",
                Type = FCLServiceType.Authn,
                Method = "HTTP/POST",
                Endpoint = _options.AuthnUri
            });

            var data = new StringContent(json, Encoding.UTF8, "application/json");           
            var response = await client.PostAsync(_options.AuthnUri, data);
            var authnResponse = JsonConvert.DeserializeObject<AuthnResponse>(response.Content.ReadAsStringAsync().Result);

            var authParams = new AuthenticateParams
            {
                AuthnResponse = authnResponse,
                RedirectUri = _options.RedirectUri,
                Options = _options,
                HttpClient = client
            };

            return await _options.Browser.AuthenticateAsync(authParams);
        }
    }
}
