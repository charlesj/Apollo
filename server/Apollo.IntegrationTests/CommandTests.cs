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

        private string loginToken = "9f0b556ece8e4c4ab9ba19fc92a52cf1";

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
            data.Add("getLogEntries", new object(), true);
            data.Add("getAllActiveLoginSessions", new object(), true);
            data.Add("addMetric", new {name="testName", category="testCategory", value=1.0f}, true);
            data.Add("getMetrics", new {name="testname"}, true);
            data.Add("getMetrics", new object(), true);
            data.Add("getMetrics", new {category="testCategory"}, true);
            data.Add("getMetrics", new {category="testCategory", name="testname"}, true);
            data.Add("saveBookmark", new {title="title", link="link", description="description", tags=new[]{"tag1", "tag2"}}, true);
            data.Add("saveBookmark", new
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
            data.Add("SaveBoard", new { board = new {title="test board"}}, true);
            data.Add("SaveBoard", new { board = new {title="test", load_order=0, id=1}}, true);
            data.Add("GetBoards", new {}, true);
            data.Add("GetBoardItems", new {board_id = 1}, true);
            data.Add("SaveBoardItem", new {title="test", link="", description="", board_id=1}, true);
            data.Add("SaveBoardItem", new {title="test update", link="", description="", id=1}, false);
            data.Add("SaveBoardItem", new {title="test update", link="", description="", id=1, board_id=1}, true);
            data.Add("DeleteBoard", new {id = 3}, true);
            data.Add("DeleteBoardItem", new {id = 3}, true);
            data.Add("UpsertChecklist", new {checklist = new
            {
                name="test checklist",
                type="daily",
                description="yes this is description"
            }}, true);
            data.Add("UpsertChecklist", new {checklist = new
            {
                id=1,
                name="test checklist",
                type="daily",
                description="yes this is description"
            }}, true);
            data.Add("UpsertChecklistItem", new
            {
                Item = new
                {
                    checklist_id=1,
                    name="first item",
                    type="required",
                    description="i am describe"
                }
            }, true);
            data.Add("UpsertChecklistItem", new
            {
                Item = new
                {
                    id=1,
                    checklist_id=1,
                    name="first item",
                    type="required",
                    description="i am describe"
                }
            }, true);
            data.Add("GetChecklists", new {}, true);
            data.Add("GetChecklistItems", new {id=1}, true);
            data.Add("DeleteChecklist", new {id=1}, true);
            data.Add("DeleteChecklistItem", new {id=1}, true);
            data.Add("UpsertChecklistCompletion", new
            {
                item=new
                {
                    checklist_id=1,
                    notes=string.Empty
                }
            }, true );
            data.Add("UpsertChecklistCompletion", new
            {
                item=new
                {
                    id=1,
                    checklist_id=1,
                    notes=string.Empty
                }
            }, true );
            data.Add("GetChecklistCompletions", new {id=1}, true);
            data.Add("DeleteChecklistCompletion", new {id=1}, true);
            data.Add("GetChecklistCompletionItems", new {id=1}, true);
            data.Add("DeleteChecklistCompletionItem", new {id=1}, true);
            data.Add("UpsertChecklistCompletionItem", new
            {
                item= new
                {
                   checklist_completion_id=1,
                   checklist_item_id=1,
                   completed=1
                }
            }, true);
            data.Add("UpsertChecklistCompletionItem", new
            {
                item= new
                {
                    id=1,
                    checklist_completion_id=1,
                    checklist_item_id=1,
                    completed=1
                }
            }, true);
            data.Add("GetChecklistItemHistory", new { }, false);
            data.Add("GetChecklistItemHistory", new {checklist_item_id=1 }, true);

            data.Add("AddCompletedChecklist", new
            {
                checklist_id=1,
                notes="This was completed",
                items = new[]
                {
                    new {checklist_item_id=1, completed=1},
                    new {checklist_item_id=2, completed=0}
                }
            }, true);

            //data.Add("GetChecklistCompletion", new {completed_checklist_id = 1}, true);

            data.Add("UpsertGoal", new { Goal= new { Slug="testGoal" } }, true);
            data.Add("GetGoal", new { Id="goal:testGoal" }, true);
            data.Add("UpsertGoal", new { Goal= new { Slug="deletableGoal" } }, true);
            data.Add("DeleteGoal", new { Id="goal:deletableGoal" }, true);
            data.Add("GetGoals", new { }, true);
            return data;
        }
    }
}
