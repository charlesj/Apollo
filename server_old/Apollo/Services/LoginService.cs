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

        Task<bool> ValidateToken(string token, string ipAddress, string userAgent);

        Task<bool> CheckPassword(string password);

        Task RevokeToken(string token);
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
            if (!await this.CheckPassword(password))
                return null;

            var activeToken = Guid.NewGuid().ToString("N");
            await this.loginSessionDataService.CreateSession(activeToken);
            return activeToken;
        }

        public async Task<bool> ValidateToken(string token, string ipAddress, string userAgent)
        {
            var currentSessions = await this.loginSessionDataService.GetAllActiveSessions();

            var valid = currentSessions.Any(s => s.token == token);

            if (valid)
            {
                await this.loginSessionDataService.UpdateLastSeen(token, ipAddress, userAgent);
            }

            return valid;
        }

        public async Task<bool> CheckPassword(string password)
        {
            var passwordHash = await this.userSettingsService
                .GetSetting<string>(Constants.UserSettings.PasswordHash);

            return this.passwordHasher.CheckHash(passwordHash, password);
        }

        public async Task RevokeToken(string token)
        {
            await this.loginSessionDataService.RevokeSession(token);
        }
    }
}