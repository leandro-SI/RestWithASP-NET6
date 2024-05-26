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

            var userResponse = _context.Users.FirstOrDefault(u => u.UserName == user.UserName && u.Password == pass.ToString());

            return userResponse;
        }

        public User ValidateCredentials(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName);
        }

        public User RefreshUserInfo(User user)
        {
            if (!_context.Users.Any(u => u.Id.Equals(user.Id)))
                return null;

            var result = _context.Users.SingleOrDefault(p => p.Id.Equals(user.Id));

            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
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
