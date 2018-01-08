using System.Threading.Tasks;
using Apollo.Commands.Log;
using Apollo.CommandSystem;
using Apollo.Data;
using Moq;
using Xunit;

namespace Apollo.Tests.Commands.Journal
{
    public class AddJournalEntryTests : BaseUnitTest<AddLogEntry>
    {
        public AddJournalEntryTests()
        {
            Mock<IJournalDataService>()
                .Setup(m => m.CreateJournalEntry(It.IsAny<JournalEntry>()))
                .Returns(Task.FromResult(0));
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("  ", false)]
        [InlineData("    ", false)]
        [InlineData("\n", false)]
        [InlineData("\t", false)]
        [InlineData("content", true)]
        public async void ValidatesNoteContent(string note, bool expectedValid)
        {
            this.ClassUnderTest.Note = note;
            var isValid = await this.ClassUnderTest.IsValid();
            Assert.Equal(expectedValid, isValid);
        }

        [Fact]
        public async void SavesViaJournalService()
        {
            var journalDataService = Mock<IJournalDataService>();
            this.ClassUnderTest.Note = "note";

            await this.ClassUnderTest.Execute();

            journalDataService.Verify(
                x => x.CreateJournalEntry(
                    It.Is<JournalEntry>(
                        je => je.note == this.ClassUnderTest.Note)));
        }

        [Fact]
        public async void ReturnsSuccessResult()
        {
            this.ClassUnderTest.Note = "note";

            var result = await this.ClassUnderTest.Execute();

            Assert.Same(CommandResult.SuccessfulResult, result);
        }
    }
}
