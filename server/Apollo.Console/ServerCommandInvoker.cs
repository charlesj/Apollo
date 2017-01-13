using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Apollo.Console
{
    public class ServerCommandInvoker
    {
        public void Execute(
            string command,
            object parameters,
            SharedCommandOptions options)
        {
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(options.Endpoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var payload = new JsonRpcRequest {id = "1", method = command, @params = parameters};
                var json = JsonConvert.SerializeObject(payload, Formatting.Indented);
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
                    else
                    {
                        System.Console.WriteLine(JsonConvert.SerializeObject(parsed.result.Result, Formatting.Indented));
                    }
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
            }
        }

        private class JsonRpcRequest
        {
            public string id { get; set; }
            public string method { get; set; }
            public object @params { get; set; }
        }
    }
}