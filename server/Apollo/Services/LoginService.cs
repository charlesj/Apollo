using System;
using System.Threading.Tasks;
using Apollo.Utilities;

namespace Apollo.Services
{
    public interface ILoginService
    {
        Task<string> Authenticate(string password);

        Task<bool> ValidateToken(string token);
    }

    public class LoginService : ILoginService
    {
        private readonly IConfiguration configuration;
        private readonly IPasswordHasher passwordHasher;
        private string activeToken;

        public LoginService(IConfiguration configuration, IPasswordHasher passwordHasher)
        {
            this.configuration = configuration;
            this.passwordHasher = passwordHasher;
        }

        public Task<string> Authenticate(string password)
        {
            var passwordHash = this.configuration.LoginPasswordHash();
            if (!this.passwordHasher.CheckHash(passwordHash, password))
            {
                return Task.FromResult((string) null);
            }

            this.activeToken = Guid.NewGuid().ToString("N");
            return Task.FromResult(this.activeToken);
        }

        public Task<bool> ValidateToken(string token)
        {
            return Task.FromResult(this.activeToken == token);
        }
    }
}