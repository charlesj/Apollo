﻿namespace Apollo.Server
{
    public class HttpResponse
    {
        public string Body { get; set; }
        public int HttpCode { get; set; }

        public static HttpResponse BadRequest(string message = null)
        {
            return new HttpResponse {Body = message, HttpCode = 400};
        }

        public static HttpResponse NotFound(string message)
        {
            return new HttpResponse {Body = message, HttpCode = 404};
        }
    }
}