using RestWithASPNET.API.Configurations;
using RestWithASPNET.API.models.Dtos;
using RestWithASPNET.API.Repositories.Interfaces;
using RestWithASPNET.API.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RestWithASPNET.API.Services
{
    public class LoginService : ILoginService
    {
        private const string DATE_FORMAT = "yyy-MM-dd HH:mm:ss";
        private TokenConfiguration _configuration;

        private IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public LoginService(TokenConfiguration configuration, IUserRepository userRepository, ITokenService tokenService)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public TokenDTO ValidateCredentials(UserDTO userCredentials)
        {
            var user = _userRepository.ValidateCredentials(userCredentials);

            if (user == null)
                return null;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.DaysToExpiry);

            DateTime createDate = DateTime.UtcNow;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

            _userRepository.RefreshUserInfo(user);

            return new TokenDTO(
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accessToken,
                refreshToken
            );
        }

        public TokenDTO ValidateCredentials(TokenDTO token)
        {
            var accessToken = token.AccessToken;
            var refreshToken = token.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiryToken(accessToken);

            var userName = principal.Identity.Name;

            var user = _userRepository.ValidateCredentials(userName);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now) 
                return null;

            accessToken = _tokenService.GenerateAccessToken(principal.Claims);
            refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            DateTime createDate = DateTime.UtcNow;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

            _userRepository.RefreshUserInfo(user);

            return new TokenDTO(
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accessToken,
                refreshToken
            );

        }
    }
}
