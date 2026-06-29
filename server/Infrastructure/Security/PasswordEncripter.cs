using Domain.Security;
using BC = BCrypt.Net.BCrypt;

namespace Infrastructure.Security
{
    public class PasswordEncripter : IPasswordEncripter
    {
        public string Encrypt(string password)
        {
            string passwordHash = BC.HashPassword(password);

            return passwordHash;
        }

        public bool Verify(string password, string passwordHash)
        {
            return BC.Verify(password, passwordHash);
        }
    }
}
