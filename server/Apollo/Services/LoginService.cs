using System;
using System.Threading.Tasks;

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
        private string activeToken;

        public LoginService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<string> Authenticate(string password)
        {
            var expectedPassword = this.configuration.LoginPassword();
            if (password != expectedPassword)
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