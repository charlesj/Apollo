using System;
using System.Text;

namespace Apollo.Utilities
{
    public interface IBase64Converter
    {
        byte[] Decode(string encoded);
        string DecodeString(string encoded);
        string Encode(byte[] bytes);
        string Encode(string message);
    }

    public class Base64Converter : IBase64Converter
    {
        public byte[] Decode(string encoded)
        {
            return Convert.FromBase64String(encoded);
        }

        public string DecodeString(string encoded)
        {
            var bytes = this.Decode(encoded);
            return Encoding.UTF8.GetString(bytes);
        }


        public string Encode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public string Encode(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            return Encode(bytes);
        }
    }
}