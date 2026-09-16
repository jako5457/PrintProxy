using MQTTnet;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace _3DPrintLib.BambuLab.Waiters
{
    internal class SingleMessageClientWaiter
    {

        private readonly IMqttClient _Client;

        public SingleMessageClientWaiter(IMqttClient client)
        {
            _Client = client;
            _Client.ApplicationMessageReceivedAsync += MessageRecievedAsync;
        }

        public string ReciverTopic { get; set; } = string.Empty;

        private byte[]? data = null;

        public async Task MessageRecievedAsync(MqttApplicationMessageReceivedEventArgs args)
        {
            if (data != null)
            {
                _Client.ApplicationMessageReceivedAsync -= MessageRecievedAsync;
            }

            ReciverTopic = args.ApplicationMessage.Topic;
            data = args.ApplicationMessage.Payload.ToArray();
        }

        public async Task<byte[]> WaitForMessage(int MaxRetries = 10)
        {
            int retries = 0;
            while(data == null) {
                await Task.Delay(Random.Shared.Next(500, 2000));
                retries++;

                if (retries >= MaxRetries)
                {
                   return new byte[0];
                }
            }

            return data;
        }
    }
}
