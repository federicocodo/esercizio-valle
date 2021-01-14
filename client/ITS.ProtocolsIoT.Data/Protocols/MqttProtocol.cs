using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


namespace ITS.ProtocolsIoT.Data.Protocols
{
    public class MqttProtocol : IProtocol
    {
        private static MqttClient client;
        private readonly string BrokerAddress = "127.0.0.1";

        public MqttProtocol(string clientId)
        {
            client = new MqttClient(BrokerAddress);
            client.Connect(clientId);
        }

        public void Subscribe(string topic)
        {
            if (topic != null)
            {

                client.Subscribe(new string[] { topic }, new byte[] { 2 });
                Console.WriteLine($"Ok subscribed topic: {topic}");
            }
            else
            {
                Console.WriteLine("Invalid topic.");
            }
        }

        public void Publish(string topic, string message)
        {
            if (topic != null)
            {
                client.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                Console.WriteLine($"Ok published message:{message} on topic:{topic}");
            }
            else
            {
                Console.WriteLine("Invalid topic.");
            }
        }

        public void Send(string data)
        {
            throw new NotImplementedException();
        }
    }
}
