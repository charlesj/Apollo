﻿using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Checklists
{
    public class GetChecklists : AuthenticatedCommand
    {
        private readonly IChecklistsDataService checklistsDataService;

        public GetChecklists(ILoginService loginService, IChecklistsDataService checklistsDataService) : base(loginService)
        {
            this.checklistsDataService = checklistsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            return CommandResult.CreateSuccessResult(await checklistsDataService.GetChecklists());
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}