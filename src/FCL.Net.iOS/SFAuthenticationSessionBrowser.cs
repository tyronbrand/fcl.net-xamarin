using FCL.Net.Models;
using FCL.Net.Xamarin.Shared;
using Foundation;
using SafariServices;
using System.Threading.Tasks;

namespace FCL.Net.iOS
{
    public class SFAuthenticationSessionBrowser : IOSBrowserBase
    {
        protected override Task<FclAuthServiceResponse> Launch(AuthenticateParams authenticateParams)
        {
            return Start(authenticateParams);
        }

        internal static async Task<FclAuthServiceResponse> Start(AuthenticateParams authenticateParams)
        {
            var task = new TaskCompletionSource<FclAuthServiceResponse>();
            var fclPoller = new FclPoller(authenticateParams, task);
            var uri = Fcl.BuildUrl(authenticateParams.AuthnResponse.Local.Endpoint, authenticateParams.AuthnResponse.Local.Params, authenticateParams.Options.Location);

            SFAuthenticationSession sfWebAuthenticationSession = null;
            sfWebAuthenticationSession = new SFAuthenticationSession(
                new NSUrl(uri),
                null,
                (callbackUrl, error) =>
                {
                    if (error.Code == (long)SFAuthenticationError.CanceledLogin)
                    {
                        task.SetResult(new FclAuthServiceResponse
                        {
                            ResultType = ResultType.UserCancel
                        });
                    }
                    else
                    {
                        task.SetResult(new FclAuthServiceResponse
                        {
                            ResultType = ResultType.UnknownError
                        });
                    }
                });

            sfWebAuthenticationSession.Start();
            fclPoller.Start();
            
            var result = await task.Task;
            fclPoller.Stop();
            sfWebAuthenticationSession.Dispose();

            return result;
        }
    }
}