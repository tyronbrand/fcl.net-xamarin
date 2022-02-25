using FCL.Net.Models;

namespace FCL.Net.Xamarin.Shared
{
    public class ActivityMediator
    {
        private static ActivityMediator _instance;
        private ActivityMediator() { }

        public static ActivityMediator Instance => _instance ?? (_instance = new ActivityMediator());

        public delegate void MessageReceivedEventHandler(FclAuthServiceResponse fclAuthServiceResponse);

        public event MessageReceivedEventHandler ActivityMessageReceived;

        public void Send(FclAuthServiceResponse fclAuthServiceResponse)
        {
            ActivityMessageReceived?.Invoke(fclAuthServiceResponse);
        }
    }
}
