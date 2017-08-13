using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", $"ApolloCLI/{Apollo.Version}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var paramsJobject = JObject.FromObject(parameters);
            paramsJobject["token"] = options.LoginToken;

            var payload = new JObject();
            payload["id"] = "1";
            payload["method"] = command;
            payload["params"] = paramsJobject;

            var json = payload.ToString();
            if (options.ShowRequest)
                System.Console.WriteLine(json);

            HttpResponseMessage serverResult = null;

            try
            {
                serverResult = await client.PostAsync(options.Endpoint, new StringContent(json));
                var parsed =
                    JsonConvert.DeserializeObject<ServerResponse>(await serverResult.Content.ReadAsStringAsync());

                if (parsed.error != null || options.FullResults)
                {
                    Console.Write(parsed.ToJson());
                }
                else if (parsed.result.Result != null && !options.SuppressOutput)
                {
                    Console.Write(((object) parsed.result.Result).ToJson());
                }

                return parsed.result;
            }
            catch (Exception exception)
            {
                Console.Red($"Error communicating to server: {exception.Message}");
                Console.Write("=======================================================================");
                Console.Write($"{exception}");
                Console.Write("=======================================================================");
                Console.Write(serverResult?.ToString());
            }

            return null;
        }
    }
}
