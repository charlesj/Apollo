using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Apollo.IntegrationTests
{
    public class CommandTests
    {
        const string ApolloEndPoint = "http://192.168.142.10/api";

        private string loginToken = "01857904912d480eaecd55ac43549f78";

        [Fact]
        public void EnsureLoginTokenSet()
        {
            Assert.NotEmpty(loginToken);
        }
        
        [Theory]
        [MemberData(nameof(Commands))]
        public void TestCommand(string commandName, object parameters, bool expectSuccess)
        {           
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(ApolloEndPoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var paramsJobject = JObject.FromObject(parameters);
                paramsJobject["token"] = loginToken;

                var payload = new JObject();
                payload["id"] = "1";
                payload["method"] = commandName;
                payload["params"] = paramsJobject;

                var json = payload.ToString();
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            HttpWebResponse httpResponse;
            try
            {
                httpResponse = httpWebRequest.GetResponse() as HttpWebResponse;
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
            return data;
        }
    }
}
