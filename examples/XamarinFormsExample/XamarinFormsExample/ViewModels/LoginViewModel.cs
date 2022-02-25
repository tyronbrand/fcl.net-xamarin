using FCL.Net;
using FCL.Net.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinFormsExample.Models;
using XamarinFormsExample.Views;

namespace XamarinFormsExample.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {        
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);           
        }

        private async void OnLoginClicked(object wallet)
        {
            var browser = DependencyService.Get<IBrowser>();

            var options = new FclOptions
            {
                RedirectUri = "com.companyname.xamarinformsexample://",
                Browser = browser,
                AuthnUri = FclWalletProvider.GetDefaultWalletProvider(DefaultFclWalletProvider.Blocto).Endpoint.ToString(),
                Location = "https://foo.com"
            };

            var fcl = new Fcl(options);

            var response = await fcl.AuthenticateAsync();

            if(response.ResultType == ResultType.Success)
            {
                Preferences.Set(PreferenceKey.CurrentUser, JsonConvert.SerializeObject(fcl.CurrentUser));
                await Shell.Current.GoToAsync($"//{nameof(AccountPage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
    }
}
