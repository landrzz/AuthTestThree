using System;
using System.Threading.Tasks;
using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Diagnostics;
using AuthTestThree.Services;

namespace AuthTestThree.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {

        public LoginPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDeviceService deviceService) 
            : base(navigationService, pageDialogService, deviceService)
        {
            LoginCommand = new DelegateCommand(OnLoginWorkedCommandExecute);
        }

        public DelegateCommand LoginCommand { get; }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            //if (authenticated)
            //{
            //    await _navigationService.NavigateAsync("/NavigationPage/TodoPage");
            //}
        }

        private async void OnLoginWorkedCommandExecute()
        {
            try
            {
                var loginservice = new LoginService();
                var result = await loginservice.LoginAsync();
                if (result)
                {
                    await _navigationService.NavigateAsync("/NavigationPage/TodoList");
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Didn't Work", "something went wrong", "OK");
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
