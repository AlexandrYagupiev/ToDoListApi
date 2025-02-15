using System.Security.Claims;
using System.Threading.Tasks;
using ToDoListAPI.Models;

namespace ToDoListAPI.Services
{
    public interface IAuthenticationService
    {
        Task<string> Authenticate(User user);
        ClaimsPrincipal GetUserClaims(string token);
    }
}