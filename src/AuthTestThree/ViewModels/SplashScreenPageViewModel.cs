using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.SecureStorage;
using Prism.AppModel;
using Prism.Navigation;
using Prism.Services;

namespace AuthTestThree.ViewModels
{

    public class SplashScreenPageViewModel : ViewModelBase
    {
        public SplashScreenPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDeviceService deviceService) 
            : base(navigationService, pageDialogService, deviceService)
        {
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            // TODO: Implement any initialization logic you need here. Example would be to handle automatic user login

            // Simulated long running task. You should remove this in your app.
            await Task.Delay(2000);
            var result = TryCredentialRestore();


            if (result)
            {
                await _navigationService.NavigateAsync("NavigationPage/TodoList");
            } else 
            {
                await _navigationService.NavigateAsync("/LoginPage");
            }
        }

        public bool TryCredentialRestore()
        {
            const string userIdKey = ":UserId";
            const string tokenKey = ":Token";
            if (CrossSecureStorage.Current.HasKey(userIdKey)
                && CrossSecureStorage.Current.HasKey(tokenKey))
            {
                string userId = CrossSecureStorage.Current.GetValue(userIdKey);
                string token = CrossSecureStorage.Current.GetValue(tokenKey);
				App.OwnerID = userId;
                TodoItemManager.DefaultManager.CurrentClient.CurrentUser = new MobileServiceUser(userId)
                {
                    MobileServiceAuthenticationToken = token
                };
                return true;
            }
            return false;
        }
    }
}