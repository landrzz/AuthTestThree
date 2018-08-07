using System;
using System.Text;
using System.Threading.Tasks;
using AuthTestThree.Interfaces;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Plugin.SecureStorage;

namespace AuthTestThree.Services
{
    public class LoginService
    {
        bool authenticated = false;
        MobileServiceUser user;

        public LoginService()
        {
        }

        public async Task<bool> LoginAsync()
        {
            const string userIdKey = ":UserId";
            const string tokenKey = ":Token";

            if (CrossSecureStorage.Current.HasKey(userIdKey)
                && CrossSecureStorage.Current.HasKey(tokenKey))
            {
                string userId = CrossSecureStorage.Current.GetValue(userIdKey);
                string token = CrossSecureStorage.Current.GetValue(tokenKey);

                //make sure token isnt expired
                if (!IsTokenExpired(token))
                {

                    TodoItemManager.DefaultManager.CurrentClient.CurrentUser = new MobileServiceUser(userId)
                    {
                        MobileServiceAuthenticationToken = token
                    };
                    return true;
                }

                // Expired; refresh it.
                try
                {
                    // Only works with Google, MSA and Azure.
                    await Xamarin.Forms.DependencyService.Get<IAuthenticate>().AuthenticateAsync();
                }
                catch
                {
                    // Failed - clear local user cache.
                    await Xamarin.Forms.DependencyService.Get<IAuthenticate>().LogoutAsync();
                }
            }


            //var worked = await Xamarin.Forms.DependencyService.Get<IAuthenticate>().AuthenticateAsync();
            //if (App.Authenticator != null)
            //{
                //var results = await App.Authenticator.AuthenticateAsync();

                var results = await Xamarin.Forms.DependencyService.Get<IAuthenticate>().AuthenticateAsync();
                authenticated = results.Item1;
                user = results.Item2;
            //}


            if (user != null)
            {
                CrossSecureStorage.Current.SetValue(userIdKey, user.UserId);
                CrossSecureStorage.Current.SetValue(tokenKey, user.MobileServiceAuthenticationToken);
				App.OwnerID = user.UserId;
            }

            return authenticated;
        }

        private bool IsTokenExpired(string token)
        {
            // No token == expired.
            if (string.IsNullOrEmpty(token))
                return true;

            // Split the string apart; we want the JSON payload.
            string[] parts = token.Split('.');
            if (parts.Length != 3)
                throw new ArgumentException("Token must consist from 3 delimited by dot parts.");

            string jwt = parts[1]
                .Replace('-', '+')  // 62nd char of encoding
                .Replace('_', '/'); // 63rd char of encoding
            switch (jwt.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: jwt += "=="; break; // Two pad chars
                case 3: jwt += "="; break;  // One pad char
                default:
                    throw new ArgumentException("Token is not a valid Base64 string.");
            }

            // Convert to a JSON string (std. Base64 decode)
            string json = Encoding.UTF8.GetString(Convert.FromBase64String(jwt));

            // Get the expiration date from the JSON object.
            var jsonObj = JObject.Parse(json);
            var exp = Convert.ToDouble(jsonObj["exp"].ToString());

            // JWT expiration is an offset from 1/1/1970 UTC
            var expire = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(exp);
            return expire < DateTime.UtcNow;
        }


    }
}
