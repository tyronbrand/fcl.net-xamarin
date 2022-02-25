using FCL.Net.Models;
using FCL.Net.Xamarin.Shared;
using Foundation;
using SafariServices;
using System.Threading.Tasks;
using UIKit;

namespace FCL.Net.iOS
{
    public class SFSafariViewControllerBrowser : IOSBrowserBase
    {
        private static TaskCompletionSource<FclAuthServiceResponse> _task;
        private static FclPoller _fclPoller;

        protected override Task<FclAuthServiceResponse> Launch(AuthenticateParams authenticateParams)
        {
            return Start(authenticateParams);
        }

        internal static async Task<FclAuthServiceResponse> Start(AuthenticateParams authenticateParams)
        {
            _task = new TaskCompletionSource<FclAuthServiceResponse>();
            _fclPoller = new FclPoller(authenticateParams, _task);

            var uri = Fcl.BuildUrl(authenticateParams.AuthnResponse.Local.Endpoint, authenticateParams.AuthnResponse.Local.Params, authenticateParams.Options.Location);
            var safari = new SFSafariViewController(new NSUrl(uri))
            {
                Delegate = new SafariViewControllerDelegate()
            };

            void Callback(FclAuthServiceResponse response)
            {
                _task.SetResult(response);
            }

            ActivityMediator.Instance.ActivityMessageReceived += Callback;

            var fclAuthServiceResponse = await AuthenticateTask(safari);

            _fclPoller.Stop();
            if (fclAuthServiceResponse.ResultType == ResultType.Success)
            {
                safari.DismissViewController(true, null);
                safari.Dispose();
            }

            ActivityMediator.Instance.ActivityMessageReceived -= Callback;

            return fclAuthServiceResponse;
        }

        private static Task<FclAuthServiceResponse> AuthenticateTask(SFSafariViewController safari)
        {
            FindRootController().PresentViewController(safari, true, null);
            _fclPoller.Start();

            return _task.Task;
        }        

        private static UIViewController FindRootController()
        {
            var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;
            return vc;
        }        

        class SafariViewControllerDelegate : SFSafariViewControllerDelegate
        {
            public override void DidFinish(SFSafariViewController controller)
            {
                ActivityMediator.Instance.Send(
                    new FclAuthServiceResponse
                    {
                        ResultType = ResultType.UserCancel
                    });
            }
        }
    }
}