using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Apollo.CLI
{
    public class ServerCommandInvoker
    {
        private ConsoleNotShitty Console = new ConsoleNotShitty();

        public async Task<ServerCommandResult> Execute(string command, object parameters, CommandLineOptions options)
        {
            var request = WebRequest.Create(options.Endpoint);
            request.ContentType = "application/json";
            request.Method = "POST";
            //httpWebRequest.UserAgent = "ApolloCLI/1.0";

            using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync()))
            {
                var paramsJobject = JObject.FromObject(parameters);
                paramsJobject["token"] = options.LoginToken;

                var payload = new JObject();
                payload["id"] = "1";
                payload["method"] = command;
                payload["params"] = paramsJobject;

                var json = payload.ToString();
                if (options.ShowRequest)
                    System.Console.WriteLine(json);
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            HttpWebResponse httpResponse;
            try
            {
                httpResponse = (HttpWebResponse) await request.GetResponseAsync();
            }
            catch (WebException we)
            {
                httpResponse = we.Response as HttpWebResponse;
                if (httpResponse == null)
                    throw;
            }

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                try
                {
                    var parsed = JsonConvert.DeserializeObject<ServerResponse>(result);

                    if (parsed.error != null || options.FullResults)
                    {
                        Console.Write(result);
                    }
                    else if (parsed.result.Result != null && !options.SuppressOutput)
                    {
                        Console.Write(((object)parsed.result.Result).ToJson());
                    }

                    return parsed.result;
                }
                catch (Exception exception)
                {
                    Console.Red($"Error communicating to server: {exception.Message}");
                    Console.Write("=======================================================================");
                    Console.Write($"{exception}");
                    Console.Write("=======================================================================");
                    Console.Write(result);
                }

                return null;
            }
        }
    }
}
