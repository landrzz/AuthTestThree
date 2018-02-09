using System;
using System.Threading.Tasks;

namespace AuthTestThree.Interfaces
{
    public interface IAuthenticate
    {
        Task<bool> AuthenticateAsync();

        Task<bool> LogoutAsync();
    }
}
