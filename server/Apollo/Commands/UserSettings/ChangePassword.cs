using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Services;
using Apollo.Utilities;

namespace Apollo.Commands.UserSettings
{
    public class ChangePassword : AuthenticatedCommand
    {
        private readonly IPasswordHasher passwordHasher;
        private readonly IUserSettingsService userSettingsService;
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordVerification { get; set; }


        public ChangePassword(
            ILoginService loginService,
            IPasswordHasher passwordHasher,
            IUserSettingsService userSettingsService) : base(loginService)
        {
            this.passwordHasher = passwordHasher;
            this.userSettingsService = userSettingsService;
        }

        public override async Task<CommandResult> Execute()
        {
            var newHash = this.passwordHasher.GenerateHash(this.NewPassword);

            await this.userSettingsService.SetSetting<string>(Constants.UserSettings.PasswordHash, newHash);

            return CommandResult.SuccessfulResult;
        }

        public override async Task<bool> IsValid()
        {
            var passwordCheck = await this.loginService.CheckPassword(this.CurrentPassword);
            if (!passwordCheck)
                return false;

            return NewPassword == NewPasswordVerification;
        }

        public override object ExamplePayload()
        {
            return new { CurrentPassword, NewPassword, NewPasswordVerification };
        }
    }
}
