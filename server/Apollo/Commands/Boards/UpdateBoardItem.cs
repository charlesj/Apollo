﻿using System;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Boards
{
    public class UpdateBoardItem : AuthenticatedCommand
    {
        private readonly IBoardDataService dataService;

        public int board_id { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public DateTime completed_at { get; set; }

        public UpdateBoardItem(ILoginService loginService, IBoardDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await dataService.AddItem(new BoardItem
            {
                board_id = board_id,
                title = title,
                link = link,
                description = description,
                completed_at = completed_at
            });
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(title));
        }
    }
}
