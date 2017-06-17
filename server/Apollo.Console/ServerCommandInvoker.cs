using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Apollo.Console
{
    public class ServerCommandInvoker
    {
        public dynamic Execute(
            string command,
            object parameters,
            SharedCommandOptions options)
        {
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(options.Endpoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.UserAgent = "ApolloCLI/1.0";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
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
                streamWriter.Close();
            }

            HttpWebResponse httpResponse;
            try
            {
                httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
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
                    var parsed = JsonConvert.DeserializeObject<dynamic>(result);

                    if (parsed.error != null || options.FullResults)
                    {
                        System.Console.WriteLine(result);
                    }
                    else if (parsed.result.Result != null)
                    {
                        System.Console.WriteLine(JsonConvert.SerializeObject(parsed.result.Result, Formatting.Indented));
                    }

                    return parsed.result;
                }
                catch (Exception exception)
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine($"Error communicating to server: {exception.Message}");
                    System.Console.ResetColor();
                    System.Console.WriteLine("=======================================================================");
                    System.Console.WriteLine($"{exception}");
                    System.Console.WriteLine("=======================================================================");
                    System.Console.WriteLine(result);
                }

                return null;
            }
        }
    }
}
