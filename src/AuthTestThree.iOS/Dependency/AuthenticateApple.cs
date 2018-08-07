using System;
using System.Threading.Tasks;
using AuthTestThree.Interfaces;
using Foundation;
using Microsoft.WindowsAzure.MobileServices;
using UIKit;
using AuthTestThree.iOS;
using Xamarin.Forms;
using Plugin.SecureStorage;
using System.Collections.Generic;

[assembly: Dependency(typeof(AuthenticateApple))]
namespace AuthTestThree.iOS
{
    partial class AuthenticateApple :  IAuthenticate 
    {
        MobileServiceUser user;
        const string UserIdKey = ":UserId";
        const string TokenKey = ":Token";

        public AuthenticateApple()
        {
        }

        public async Task<Tuple<bool, MobileServiceUser>> AuthenticateAsync()
        {
            bool success = false;
            try
            {
                if (user == null)
                {
                    // The authentication provider could also be Facebook, Twitter, or Microsoft
                    user = await TodoItemManager.DefaultManager.CurrentClient.LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController, MobileServiceAuthenticationProvider.Google,
                                                                                         Helpers.AppConstants.URLScheme); // new Dictionary<string, string> { { "access_type", "offline" } },
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
            return new Tuple<bool, MobileServiceUser>(success, user);
        }

        public async Task<Tuple<bool, MobileServiceUser>> LogoutAsync()
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

                // Delete the local cache
                CrossSecureStorage.Current.DeleteKey(UserIdKey);
                CrossSecureStorage.Current.DeleteKey(TokenKey);

                user = null;
                success = true;

            }
            catch (Exception ex)
            {
                var logoutAlert = UIAlertController.Create("Logout failed", ex.Message, UIAlertControllerStyle.Alert);
                logoutAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, null));
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(logoutAlert, true, null);
				success = false;
            }
            return new Tuple<bool, MobileServiceUser>(success, user);
        }
    }
}
