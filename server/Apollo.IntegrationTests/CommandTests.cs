using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Xunit;

namespace Apollo.IntegrationTests
{
    public class CommandTests
    {
        const string ApolloEndPoint = "http://192.168.142.10/";

        [Theory]
        [MemberData(nameof(Commands))]
        public void TestCommand(string commandName, object parameters, bool expectSuccess)
        {
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(ApolloEndPoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var payload = new {id = "1", method = commandName, @params = parameters};
                var json = JsonConvert.SerializeObject(payload);
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
            return data;
        }
    }
}