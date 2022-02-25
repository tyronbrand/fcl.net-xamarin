using FCL.Net.Models;
using System.Threading.Tasks;
using UIKit;

namespace FCL.Net.iOS
{
    public class AutoSelectBrowser : IOSBrowserBase
    {
        protected override Task<FclAuthServiceResponse> Launch(AuthenticateParams authenticateParams)
        {
            // For iOS 12+ use ASWebAuthenticationSession
            if (UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
                return ASWebAuthenticationSessionBrowser.Start(authenticateParams);

            // For iOS 11 use SFAuthenticationSession
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                return SFAuthenticationSessionBrowser.Start(authenticateParams);

            // For iOS 10 and earlier use SFSafariViewController
            return SFSafariViewControllerBrowser.Start(authenticateParams);
        }
    }
}