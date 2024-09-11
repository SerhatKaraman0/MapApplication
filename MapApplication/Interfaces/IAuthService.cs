using System.Threading.Tasks;
using MapApplication.Data;
using MapApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace MapApplication.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateToken(UsersDb user);
        Task<IActionResult> AuthenticateUser(string email, string password);
        void LogoutUser(string token);
    }
}
