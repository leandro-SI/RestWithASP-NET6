namespace RestWithASPNET.API.models.Dtos
{
    public class TokenDTO
    {
        public bool Authenticated { get; set; }
        public string Created { get; set; }
        public string Expiration { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public TokenDTO()
        {
            
        }

        public TokenDTO(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public TokenDTO(bool authenticated, string created, string expiration, string acessToken, string refreshToken)
        {
            Authenticated = authenticated;
            Created = created;
            Expiration = expiration;
            AccessToken = acessToken;
            RefreshToken = refreshToken;
        }
    }
}
