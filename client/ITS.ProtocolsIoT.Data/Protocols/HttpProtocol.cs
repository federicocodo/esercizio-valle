using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ITS.ProtocolsIoT.Data
{
    public class HttpProtocol : IProtocol
    {
        private string endpoint;
        private HttpWebRequest httpWebRequest;

        public HttpProtocol(string endpoint)
        {
            this.endpoint = endpoint;
        }

        public void Publish(string topic, string message)
        {
            throw new NotImplementedException();
        }

        public void Send(string data)
        {
            httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            Console.Out.WriteLine(httpResponse.StatusCode);

            httpResponse.Close();
        }

        public void Subscribe(string data)
        {
            throw new NotImplementedException();
        }
    }
}
