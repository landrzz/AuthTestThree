using System;
using System.Threading.Tasks;
using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Diagnostics;
using AuthTestThree.Services;
using AuthTestThree.Interfaces;
using Microsoft.WindowsAzure.MobileServices;

namespace AuthTestThree.ViewModels
{
    public class TodoListViewModel : ViewModelBase
    {
        bool Unauthenticated = false;
        MobileServiceUser user;
        public DelegateCommand LogoutCommand { get; }

        public TodoListViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDeviceService deviceService) 
            : base(navigationService, pageDialogService, deviceService)
        {
            LogoutCommand = new DelegateCommand(OnLogoutCommandExecute);
        }
       

        private async void OnLogoutCommandExecute()
        {
            var results = await Xamarin.Forms.DependencyService.Get<IAuthenticate>().LogoutAsync();
            Unauthenticated = results.Item1;
            user = results.Item2;

            if(Unauthenticated)
            {
                await _navigationService.NavigateAsync("/LoginPage");
            }
        }
    }
}
