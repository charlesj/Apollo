using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Sdk;

namespace Apollo.IntegrationTests
{
    public class CommandTests
    {
        private static int RequestCounter = 0;
        const string ApolloEndPoint = "http://192.168.142.10/api";

        private string loginToken = "8066c726d8ef4a50bcef85d2510869df";

        [Fact]
        public void EnsureLoginTokenSet()
        {
            Assert.NotEmpty(loginToken);
        }

        [Theory]
        [MemberData(nameof(Commands))]
        public async void TestCommand(string commandName, object parameters, bool expectSuccess)
        {
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(ApolloEndPoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync()))
            {
                var paramsJobject = JObject.FromObject(parameters);
                paramsJobject["token"] = loginToken;

                var payload = new JObject();
                payload["id"] = (++RequestCounter).ToString();
                payload["method"] = commandName;
                payload["params"] = paramsJobject;

                var json = payload.ToString();
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            HttpWebResponse httpResponse;
            try
            {
                httpResponse = await httpWebRequest.GetResponseAsync() as HttpWebResponse;
                Assert.NotNull(httpResponse);
                Assert.True(expectSuccess);
            }
            catch (WebException we)
            {
                httpResponse = we.Response as HttpWebResponse;
                if (httpResponse == null)
                    throw;

                Assert.False(expectSuccess);
            }

            await Task.Delay(500);
        }

        public static TheoryData<string, object, bool> Commands()
        {
            var data = new TheoryData<string, object, bool>();
            data.Add("testcommand", new object(), true);
            data.Add("BOGUS", new object(), false);
            data.Add("applicationInfo", new object(), true);
            data.Add("getAllJournalEntries", new object(), true);
            data.Add("getAllActiveLoginSessions", new object(), true);
            data.Add("addMetric", new {name="testName", category="testCategory", value=1.0f}, true);
            data.Add("getMetrics", new {name="testname"}, true);
            data.Add("getMetrics", new object(), true);
            data.Add("getMetrics", new {category="testCategory"}, true);
            data.Add("getMetrics", new {category="testCategory", name="testname"}, true);
            data.Add("addBookmark", new {title="title", link="link", description="description", tags=new[]{"tag1", "tag2"}}, true);
            data.Add("addBookmark", new
            {
                title="title",
                link="link",
                description="description",
                tags=new[]{"tag1", "tag2"},
                createdAt= DateTime.Now.AddDays(-10),
                modifiedAt = DateTime.Now.AddDays(-5)
            }, true);
            data.Add("getBookmarks", new {start=1}, true);
            data.Add("getBookmarks", new {link="link"}, true);
            data.Add("addTodoItem", new { title="Test item"}, true);
            data.Add("getTodoItems", new object(), true);
            data.Add("updateTodoItem", new { item = new {id = 1, title = "Test Item 2"}}, true);
            data.Add("updateTodoItem", new { item = new {id = 1, title = "Test Item 2", completed_at=DateTime.Now}}, true);
            data.Add("addTodoQueueItem", new { item = new {title = "Test Item 2"}}, true);
            data.Add("addTodoQueueItem", new { item = new {title = "Test Item 3", link="http://example.com", description="whatever"}}, true);
            data.Add("getTodoQueueItems", new { }, true);
            data.Add("updateTodoQueueItem", new { item = new
            {
                id=1,
                title = "Test Item 3",
                link="http://example.com",
                description="whatever",
                completed_at = DateTime.Now
            }}, true);
            data.Add("getSummaries", new { }, true);

            data.Add("getJobs", new {}, true);
            data.Add("getJobs", new {expired=true}, true);
//            data.Add("addJob",
//                new
//                {
//                    commandName = "testCommand",
//                    parameters = new object(),
//                    schedule = new {hourly = true, start = DateTime.Now}
//                }, true);
            data.Add("cancelJob", new {jobId = 1}, true);
            data.Add("getAvailableCommands", new {}, true);
            data.Add("getJobHistory", new {jobId = 1}, true);
            data.Add("UpdateCryptoPrices", new {}, true);
            data.Add("GetAssetPrice", new {symbol="btc"}, true);
            data.Add("AddNote", new {name="test note", body="yes I am note"}, true);
            data.Add("GetNotes", new{}, true);
            data.Add("GetNote", new {id = 1}, true);
            data.Add("UpdateNote", new {id = 1, name = "two", body = "I am updated note"}, true);
            data.Add("AddBoard", new {title="test"}, true);
            data.Add("UpdateBoard", new {title="test", load_order=0, id=1}, true);
            data.Add("GetBoards", new {}, true);
            data.Add("GetBoardItems", new {board_id = 1}, true);
            data.Add("AddBoardItem", new {title="test"}, true);
            data.Add("UpdateBoardItem", new {title="test update", id=1}, true);
            data.Add("DeleteBoard", new {id = 3}, true);
            data.Add("DeleteBoardItem", new {id = 3}, true);
            return data;
        }
    }
}
