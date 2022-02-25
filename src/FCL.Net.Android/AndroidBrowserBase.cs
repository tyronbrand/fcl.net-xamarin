using Android.App;
using Android.Content;
using FCL.Net.Models;
using FCL.Net.Xamarin.Shared;
using Plugin.CurrentActivity;
using System;
using System.Threading.Tasks;
using Uri = Android.Net.Uri;

namespace FCL.Net.Android
{
    public abstract class AndroidBrowserBase : IBrowser
    {
        protected readonly Activity _context;
        private TaskCompletionSource<FclAuthServiceResponse> _task;
        private FclPoller _fclPoller;

        public AndroidBrowserBase() : this(CrossCurrentActivity.Current.Activity) { }

        public AndroidBrowserBase(Activity context)
        {            
            _context = context;
        }

        public async Task<FclAuthServiceResponse> AuthenticateAsync(AuthenticateParams authenticateParams)
        {
            ValidateParams(authenticateParams);

            _task = new TaskCompletionSource<FclAuthServiceResponse>();
            _fclPoller = new FclPoller(authenticateParams, _task);

            ActivityMediator.Instance.ActivityMessageReceived += Callback;

            var fclAuthServiceResponse = await AuthenticateTask(authenticateParams);

            _fclPoller.Stop();

            if (fclAuthServiceResponse.ResultType == ResultType.Success)
                _context.StartActivity(new Intent(Intent.ActionView, Uri.Parse(authenticateParams.RedirectUri)));
            
            ActivityMediator.Instance.ActivityMessageReceived -= Callback;

            return fclAuthServiceResponse;
        }

        private static void ValidateParams(AuthenticateParams authenticateParams)
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
        }

        private Task<FclAuthServiceResponse> AuthenticateTask(AuthenticateParams authenticateParams)
        {
            var uri = Fcl.BuildUrl(authenticateParams.AuthnResponse.Local.Endpoint, authenticateParams.AuthnResponse.Local.Params, authenticateParams.Options.Location);
            OpenBrowser(Uri.Parse(uri));
            
            _fclPoller.Start();

            return _task.Task;
        }

        void Callback(FclAuthServiceResponse fclAuthServiceResponse)
        {
            if(!_task.Task.IsCompleted)
                _task.SetResult(fclAuthServiceResponse);
        }

        protected abstract void OpenBrowser(Uri uri);
    }
}