using System;
using System.Linq;
using System.Threading.Tasks;
using Apollo.Data;
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
        private readonly ILoginSessionDataService loginSessionDataService;
        private readonly IPasswordHasher passwordHasher;

        public LoginService(IConfiguration configuration,
                            ILoginSessionDataService loginSessionDataService,
                            IPasswordHasher passwordHasher)
        {
            this.configuration = configuration;
            this.loginSessionDataService = loginSessionDataService;
            this.passwordHasher = passwordHasher;
        }

        public Task<string> Authenticate(string password)
        {
            var passwordHash = this.configuration.LoginPasswordHash();
            if (!this.passwordHasher.CheckHash(passwordHash, password))
            {
                return Task.FromResult((string) null);
            }

            var activeToken = Guid.NewGuid().ToString("N");
            this.loginSessionDataService.CreateSession(activeToken);
            return Task.FromResult(activeToken);
        }

        public async Task<bool> ValidateToken(string token)
        {
            var currentSessions = await this.loginSessionDataService.GetAllSessions();

            foreach (var session in currentSessions)
            {
                Console.WriteLine($"Found session {session.token}");
            }

            var valid = currentSessions.Any(s => s.token == token);

            if (valid)
            {
                await this.loginSessionDataService.UpdateLastSeen(token);
            }

            return valid;
        }
    }
}