using _3DPrintLib.BambuLab.Waiters;
using FluentFTP;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Formatter;
using Newtonsoft.Json;
using PrintLib;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;

namespace _3DPrintLib.BambuLab
{
    public class BambuLabPrinter : IPrinter
    {

        public Logger<BambuLabPrinter> _Logger;
        public MqttClientFactory _MqttClientFactory;
        public BambuLabOptions _options;

        public BambuLabPrinter(Logger<BambuLabPrinter> logger, MqttClientFactory mqttClientFactory,BambuLabOptions options)
        {
            _Logger = logger;
            _MqttClientFactory = mqttClientFactory;
        }

        public async Task StartAsync(string fileName)
        {
            using IMqttClient client = _MqttClientFactory.CreateMqttClient();
         
            await client.ConnectAsync(CreateClientOptions());

            if (client.IsConnected)
            {
                var msg = new
                {
                    print = new
                    {
                        command = "start",
                        sequence_id = 0,
                        param = new
                        {
                            file_name = fileName
                        }
                    }
                };

                string json = JsonConvert.SerializeObject(msg);

                await client.PublishStringAsync("device/" + _options.SerialNunber + "/request", json);

                await client.DisconnectAsync();
            }
        }

        public async Task PauseAsync()
        {
            using IMqttClient client = _MqttClientFactory.CreateMqttClient();

            await client.ConnectAsync(CreateClientOptions());

            if (client.IsConnected)
            {
                var msg = new
                {
                    print = new
                    {
                        command = "pause",
                        sequence_id = 0,
                        param = ""
                    }
                };

                string json = JsonConvert.SerializeObject(msg);

                await client.PublishStringAsync("device/" + _options.SerialNunber + "/request", json);

                await client.DisconnectAsync();
            }
        }

        public async Task StopAsync()
        {
            using IMqttClient client = _MqttClientFactory.CreateMqttClient();

            await client.ConnectAsync(CreateClientOptions());

            if (client.IsConnected)
            {
                var msg = new
                {
                    print = new
                    {
                        command = "stop",
                        sequence_id = 0,
                        param = ""
                    }
                };

                string json = JsonConvert.SerializeObject(msg);

                await client.PublishStringAsync("device/" + _options.SerialNunber + "/request", json);

                await client.DisconnectAsync();
            }
        }

        public async Task ContinueAsync()
        {
            using IMqttClient client = _MqttClientFactory.CreateMqttClient();

            await client.ConnectAsync(CreateClientOptions());

            if (client.IsConnected)
            {
                var msg = new
                {
                    print = new
                    {
                        command = "start",
                        sequence_id = 0,
                        param = ""
                    }
                };

                string json = JsonConvert.SerializeObject(msg);

                await client.PublishStringAsync("device/" + _options.SerialNunber + "/request", json);

                await client.DisconnectAsync();
            }
        }

        public string GetIdentifier()
        {
            using SHA256 sha = SHA256.Create();

            byte[] data = Encoding.UTF8.GetBytes($"{_options.SerialNunber}:{_options.PrinterIP}");

            byte[] identifier = sha.ComputeHash(data);

            return Convert.ToBase64String(identifier);
        }

        public Task<JobStatus> GetJobStatusAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PrinterStatus> GetStatusAsync()
        {
            using IMqttClient client = _MqttClientFactory.CreateMqttClient();

            SingleMessageClientWaiter waiter = new SingleMessageClientWaiter(client);

            await client.SubscribeAsync("device/" + _options.SerialNunber);
            await client.ConnectAsync(CreateClientOptions());

            if (client.IsConnected)
            {
                var msg = await waiter.WaitForMessage();

                await client.DisconnectAsync();
            }

            throw new NotImplementedException();
        }

        public async Task UploadAsync(string FilePath)
        {
            FileInfo fileinfo = new FileInfo(FilePath);

            FtpConfig config = new FtpConfig()
            {
                ValidateAnyCertificate = true,
                EncryptionMode = FtpEncryptionMode.Explicit,
            };

            using AsyncFtpClient client = new AsyncFtpClient(_options.PrinterIP,"bblp",_options.AccessCode, 990, config);

            await client.AutoConnect();

            if (client.IsConnected || client.IsAuthenticated)
            {
                await client.UploadFile(FilePath, fileinfo.Name);
            }
        }

        private MqttClientOptions CreateClientOptions()
        {
            byte[] Passcode = Encoding.UTF8.GetBytes(_options.AccessCode);

            MqttClientTlsOptions tlsoptions = new MqttClientTlsOptionsBuilder()
                                              .UseTls()
                                              .WithAllowUntrustedCertificates()
                                              .Build();

            MqttClientOptions options = new MqttClientOptionsBuilder()
                                        .WithTcpServer(_options.PrinterIP)
                                        .WithProtocolVersion(MqttProtocolVersion.V311)
                                        .WithCleanSession()
                                        .WithClientId("PrintProxyService")
                                        .WithTlsOptions(tlsoptions)
                                        .Build();
            return options;
        }

    }
}
