using RestWithASPNET.API.models;
using RestWithASPNET.API.models.Dtos;

namespace RestWithASPNET.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User ValidateCredentials(UserDTO userDTO);
        User ValidateCredentials(string userName);
        User RefreshUserInfo(User user);
    }
}
