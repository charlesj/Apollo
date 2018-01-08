﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apollo.Commands.Log;
using Apollo.Data;
using Xunit;

namespace Apollo.Tests.Commands.Journal
{
    public class GetAllJournalEntriesTests : BaseUnitTest<GetLogEntries>
    {
        [Fact]
        public async void ReturnsDataServiceResult()
        {
            IReadOnlyList<JournalEntry> expected = new List<JournalEntry>
            {
                new JournalEntry {created_at = DateTime.Now, id = 1, note = "Hello"},
                new JournalEntry {created_at = DateTime.Now, id = 2, note = "World"}
            };

            var journalService = Mock<IJournalDataService>();
            journalService.Setup(j => j.GetJournalEntries(0)).Returns(Task.FromResult(expected));
            var result = await ClassUnderTest.Execute();

            Assert.Null(result.ErrorMessage);
            var typed = Assert.IsType<List<JournalEntry>>(result.Result);

            Assert.Collection(typed, i => Assert.Same(expected[0], i), i => Assert.Same(expected[1], i));
        }

        [Fact]
        public async void BubblesExceptions()
        {
            var exception = new Exception("Expected");
            var journalService = Mock<IJournalDataService>();
            journalService.Setup(j => j.GetJournalEntries(0)).Throws(exception);
            var actual = await Assert.ThrowsAsync<Exception>(async () => await ClassUnderTest.Execute());
            Assert.Same(exception, actual);
        }
    }
}