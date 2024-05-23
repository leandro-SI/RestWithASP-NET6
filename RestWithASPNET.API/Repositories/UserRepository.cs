using RestWithASPNET.API.Data;
using RestWithASPNET.API.models;
using RestWithASPNET.API.models.Dtos;
using RestWithASPNET.API.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace RestWithASPNET.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ProjetoContext _context;

        public UserRepository(ProjetoContext context)
        {
            _context = context;
        }

        public User ValidateCredentials(UserDTO user)
        {
            var pass = ComputeHash(user.Password, SHA256.Create());
            return _context.Users.FirstOrDefault(u =>
            (u.UserName == user.UserName) && (u.Password == user.Password));
        }

        private object ComputeHash(string password, HashAlgorithm algorithm)
        {
            byte[] ínputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = algorithm.ComputeHash(ínputBytes);

            var builder = new StringBuilder();

            foreach(var item in hashBytes)
            {
                builder.Append(item.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
