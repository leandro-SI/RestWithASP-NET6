using RestWithASPNET.API.models.Dtos;

namespace RestWithASPNET.API.Services.Interfaces
{
    public interface ILoginService
    {
        TokenDTO ValidateCredentials(UserDTO user);
        TokenDTO ValidateCredentials(TokenDTO token);
        bool RevokeToken(string userName);
    }
}
