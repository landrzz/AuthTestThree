using System;
using System.Threading.Tasks;
using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Diagnostics;

namespace AuthTestThree.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        bool authenticated = false;

        public LoginPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDeviceService deviceService) 
            : base(navigationService, pageDialogService, deviceService)
        {
            LoginCommand = new DelegateCommand(OnLoginWorkedCommandExecute);
        }

        public DelegateCommand LoginCommand { get; }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (authenticated)
            {
                await _navigationService.NavigateAsync("/NavigationPage/TodoPage");
            }
        }

        private async void OnLoginWorkedCommandExecute()
        {
            try
            {
                if (App.Authenticator != null)
                {
                    authenticated = await App.Authenticator.AuthenticateAsync();
                }

                if (authenticated)
                {
                    await _navigationService.NavigateAsync("/NavigationPage/TodoList");
                }
            } 
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("Authentication was cancelled"))
                {
                    Debug.WriteLine("Authentication cancelled by the user");
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Authentication failed");
            }

        }
    }
}
