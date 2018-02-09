using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using AuthTestThree.Interfaces;

namespace AuthTestThree.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate, IAuthenticate 
    {
        MobileServiceUser user;

        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();
            global::Rg.Plugins.Popup.Popup.Init();
            global::FFImageLoading.Forms.Touch.CachedImageRenderer.Init();
            global::FFImageLoading.ImageService.Instance.Initialize(new FFImageLoading.Config.Configuration()
            {
                Logger = new Services.DebugLogger()
            });

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            App.Init(this);

            // Code for starting up the Xamarin Test Cloud Agent
#if DEBUG
            Xamarin.Calabash.Start();
#endif
            LoadApplication(new App(new iOSInitializer()));

            return base.FinishedLaunching(uiApplication, launchOptions);
        }





        public async Task<bool> AuthenticateAsync()
        {
            bool success = false;
            try
            {
                if (user == null)
                {
                    // The authentication provider could also be Facebook, Twitter, or Microsoft
                    user = await TodoItemManager.DefaultManager.CurrentClient.LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController, MobileServiceAuthenticationProvider.Google, Helpers.AppConstants.URLScheme);
                    if (user != null)
                    {
                        var authAlert = UIAlertController.Create("Authentication", "You are now logged in " + user.UserId, UIAlertControllerStyle.Alert);
                        authAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, null));
                        UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(authAlert, true, null);
                    }
                }
                success = true;
            }
            catch (Exception ex)
            {
                var authAlert = UIAlertController.Create("Authentication failed", ex.Message, UIAlertControllerStyle.Alert);
                authAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, null));
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(authAlert, true, null);
            }
            return success;
        }

        public async Task<bool> LogoutAsync()
        {
            bool success = false;
            try
            {
                if (user != null)
                {
                    foreach (var cookie in NSHttpCookieStorage.SharedStorage.Cookies)
                    {
                        NSHttpCookieStorage.SharedStorage.DeleteCookie(cookie);
                    }
                    await TodoItemManager.DefaultManager.CurrentClient.LogoutAsync();

                    var logoutAlert = UIAlertController.Create("Authentication", "You are now logged out " + user.UserId, UIAlertControllerStyle.Alert);
                    logoutAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, null));
                    UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(logoutAlert, true, null);
                }
                user = null;
                success = true;
            }
            catch (Exception ex)
            {
                var logoutAlert = UIAlertController.Create("Logout failed", ex.Message, UIAlertControllerStyle.Alert);
                logoutAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, null));
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(logoutAlert, true, null);
            }
            return success;
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return TodoItemManager.DefaultManager.CurrentClient.ResumeWithURL(url);
        }

    }
}
