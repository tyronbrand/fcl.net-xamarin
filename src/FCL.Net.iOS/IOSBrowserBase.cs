using FCL.Net.Models;
using System;
using System.Threading.Tasks;

namespace FCL.Net.iOS
{
    public abstract class IOSBrowserBase : IBrowser
    {
        public Task<FclAuthServiceResponse> AuthenticateAsync(AuthenticateParams authenticateParams)
        {
            if (authenticateParams.AuthnResponse == null)
                throw new ArgumentException("Missing AuthnResponse", nameof(authenticateParams));

            if (authenticateParams.AuthnResponse.Local == null)
                throw new ArgumentException("Missing AuthnResponse.Local", nameof(authenticateParams));

            if (authenticateParams.AuthnResponse.Updates == null)
                throw new ArgumentException("Missing AuthnResponse.Updates", nameof(authenticateParams));

            if (string.IsNullOrWhiteSpace(authenticateParams.RedirectUri))
                throw new ArgumentException("Missing RedirectUri", nameof(authenticateParams));

            if (authenticateParams.Options == null)
                throw new ArgumentException("Missing Options", nameof(authenticateParams));

            if (authenticateParams.HttpClient == null)
                throw new ArgumentException("Missing HttpClient", nameof(authenticateParams));
                        
            return Launch(authenticateParams);
        }

        protected abstract Task<FclAuthServiceResponse> Launch(AuthenticateParams authenticateParams);
    }
}