using AuthenticationServices;
using FCL.Net.Models;
using FCL.Net.Xamarin.Shared;
using Foundation;
using System.Threading.Tasks;
using UIKit;

namespace FCL.Net.iOS
{
    public class ASWebAuthenticationSessionBrowser : IOSBrowserBase
    {
        protected override Task<FclAuthServiceResponse> Launch(AuthenticateParams authenticateParams)
        {
            return Start(authenticateParams);
        }

        internal static async Task<FclAuthServiceResponse> Start(AuthenticateParams authenticateParams)
        {
            var task = new TaskCompletionSource<FclAuthServiceResponse>();
            var fclPoller = new FclPoller(authenticateParams, task);
            var uriStr = Fcl.BuildUrl(authenticateParams.AuthnResponse.Local.Endpoint, authenticateParams.AuthnResponse.Local.Params, authenticateParams.Options.Location);

            ASWebAuthenticationSession asWebAuthenticationSession = null;
            asWebAuthenticationSession = new ASWebAuthenticationSession(
                new NSUrl(uriStr),
                null,
                (callbackUrl, error) =>
                {
                    if(error.Code == (long)ASWebAuthenticationSessionErrorCode.CanceledLogin)
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
            
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                asWebAuthenticationSession.PresentationContextProvider = new PresentationContextProviderToSharedKeyWindow();
                asWebAuthenticationSession.PrefersEphemeralWebBrowserSession = false;
            }
            
            asWebAuthenticationSession.Start();
            fclPoller.Start();

            var result = await task.Task;
            fclPoller.Stop();
            asWebAuthenticationSession.Dispose();

            return result;
        }

        class PresentationContextProviderToSharedKeyWindow : NSObject, IASWebAuthenticationPresentationContextProviding
        {
            public UIWindow GetPresentationAnchor(ASWebAuthenticationSession session)
            {
                return UIApplication.SharedApplication.KeyWindow;
            }
        }
    }
}