﻿using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;
using Apollo.Utilities;

namespace Apollo.Commands.UserSettings
{
    public class ChangePassword : AuthenticatedCommand
    {
        private readonly IPasswordHasher passwordHasher;
        private readonly IUserSettignsDataService userSettingsService;
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordVerification { get; set; }


        public ChangePassword(
            ILoginService loginService,
            IPasswordHasher passwordHasher,
            IUserSettignsDataService userSettingsService) : base(loginService)
        {
            this.passwordHasher = passwordHasher;
            this.userSettingsService = userSettingsService;
        }

        public override async Task<CommandResult> Execute()
        {
            var newHash = this.passwordHasher.GenerateHash(this.NewPassword);

            var setting = new UserSetting
            {
                name = Constants.UserSettings.PasswordHash,
                value = newHash
            };

            await this.userSettingsService.UpdateSetting(setting);

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
