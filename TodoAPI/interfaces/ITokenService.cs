using TodoAPI.Models;

namespace TodoAPI.interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
