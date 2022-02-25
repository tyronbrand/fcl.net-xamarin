using FCL.Net.Models;
using FCL.Net.Strategy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCL.Net
{
    public class Fcl
    {        
        public FlowUser CurrentUser;
        private readonly HttpPostStrategy _httpPostStrategy;

        public Fcl(FclOptions options)
        {
            _httpPostStrategy = new HttpPostStrategy(options);
        }

        public void Unauthenticate()
        {
            CurrentUser = null;
        }

        public async Task Reauthenticate()
        {
            Unauthenticate();
            await AuthenticateAsync();
        }

        public async Task<FclAuthServiceResponse> AuthenticateAsync()
        {
            var authResult = await _httpPostStrategy.ExecHttpPostAsync();

            if (authResult.ResultType == ResultType.Success)
                BuildUser(authResult.AuthnResponse);

            return authResult;
        }

        public static string BuildUrl(Uri uri, Dictionary<string, object> parameters = null, string location = null)
        {
            var paramList = new List<string>();

            if (parameters != null && !string.IsNullOrWhiteSpace(location))
                paramList.Add($"l6n={location}");
            
            foreach (var parameter in parameters)
                paramList.Add($"{parameter.Key}={parameter.Value}");

            return $"{uri}?{string.Join("&", paramList)}";
        }

        private void BuildUser(AuthnResponse authnResponse)
        {
            CurrentUser = new FlowUser
            {
                Addr = authnResponse.Data.Addr,
                LoggedIn = true,
                Services = authnResponse.Data.Services,
            };
        }
    }
}
