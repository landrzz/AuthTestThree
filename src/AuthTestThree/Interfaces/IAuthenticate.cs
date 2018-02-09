using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace AuthTestThree.Interfaces
{
    public interface IAuthenticate
    {
        Task<Tuple<bool, MobileServiceUser>> AuthenticateAsync();

        Task<Tuple<bool, MobileServiceUser>> LogoutAsync();
    }
}
