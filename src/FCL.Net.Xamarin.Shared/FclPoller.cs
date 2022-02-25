using FCL.Net.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;

namespace FCL.Net.Xamarin.Shared
{
    public class FclPoller
    {
        private readonly string _pollUri;
        private readonly int _timeoutMs;
        private readonly double _intervalMs;
        private readonly TaskCompletionSource<FclAuthServiceResponse> _task;
        private bool _disableTimer;
        private static HttpClient _client;
        private Timer _timer;

        public FclPoller(AuthenticateParams authenticateParams, TaskCompletionSource<FclAuthServiceResponse> task)
        {
            _task = task;
            _client = authenticateParams.HttpClient ?? new HttpClient();
            _timeoutMs = authenticateParams.TimerTimeoutMs;
            _intervalMs = authenticateParams.TimerIntervalMs;
            _pollUri = Fcl.BuildUrl(authenticateParams.AuthnResponse.Updates.Endpoint, authenticateParams.AuthnResponse.Updates.Params, authenticateParams.Options.Location);
        }

        public void Start()
        {
            _timer = new Timer()
            {
                AutoReset = true
            };
            _disableTimer = false;
            _timer.Elapsed += async (sender, e) => await PollAsync(DateTime.UtcNow);
            _timer.Interval = _intervalMs;
            _timer.Enabled = true;
        }

        public void Stop()
        {
            _disableTimer = true;
            _timer.Enabled = false;
            _timer.Dispose();
        }

        private async Task PollAsync(DateTime startTime)
        {
            try
            {
                _timer.Enabled = false;

                if (DateTime.UtcNow.Subtract(startTime).TotalMilliseconds > _timeoutMs)
                {
                    Stop();
                    _task.SetResult(new FclAuthServiceResponse
                    {
                        ResultType = ResultType.Timeout
                    });
                }

                var response = await _client.GetStringAsync(_pollUri);
                var authnResponse = JsonConvert.DeserializeObject<AuthnResponse>(response);

                if (authnResponse.Status == Status.Approved)
                {
                    _task.SetResult(
                        new FclAuthServiceResponse
                        {
                            AuthnResponse = authnResponse,
                            ResultType = ResultType.Success
                        });
                }
            }
            catch (Exception)
            {
                Stop();
                _task.SetResult(new FclAuthServiceResponse
                {
                    ResultType = ResultType.UnknownError
                });
            }
            finally
            {
                if (!_disableTimer)
                {
                    _timer.Enabled = true;
                }
            }
        }
    }
}
