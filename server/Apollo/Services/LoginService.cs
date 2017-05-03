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
        private readonly ILoginSessionDataService loginSessionDataService;
        private readonly IPasswordHasher passwordHasher;
        private readonly IUserSettingsService userSettingsService;

        public LoginService(ILoginSessionDataService loginSessionDataService,
                            IPasswordHasher passwordHasher,
                            IUserSettingsService userSettingsService)
        {
            this.loginSessionDataService = loginSessionDataService;
            this.passwordHasher = passwordHasher;
            this.userSettingsService = userSettingsService;
        }

        public async Task<string> Authenticate(string password)
        {
            var passwordHash = await this.userSettingsService
                    .GetSetting<string>(Constants.UserSettings.PasswordHash);
            if (!this.passwordHasher.CheckHash(passwordHash, password))
            {
                return null;
            }

            var activeToken = Guid.NewGuid().ToString("N");
            await this.loginSessionDataService.CreateSession(activeToken);
            return activeToken;
        }

        public async Task<bool> ValidateToken(string token)
        {
            var currentSessions = await this.loginSessionDataService.GetAllSessions();

            var valid = currentSessions.Any(s => s.token == token);

            if (valid)
            {
                await this.loginSessionDataService.UpdateLastSeen(token);
            }

            return valid;
        }
    }
}