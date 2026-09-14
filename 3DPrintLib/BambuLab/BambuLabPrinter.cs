using _3DPrintLib.BambuLab.Dtos.PrintInfo;
using _3DPrintLib.BambuLab.Waiters;
using FluentFTP;
using FluentFTP.GnuTLS;
using FluentFTP.GnuTLS.Enums;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Formatter;
using Newtonsoft.Json;
using PrintLib;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;

namespace _3DPrintLib.BambuLab
{
    public class BambuLabPrinter : IPrinter
    {

        public ILogger<BambuLabPrinter> _Logger;
        public MqttClientFactory _MqttClientFactory;
        public BambuLabOptions _options;

        public BambuLabPrinter(ILogger<BambuLabPrinter> logger, MqttClientFactory mqttClientFactory,BambuLabOptions options)
        {
            _Logger = logger;
            _MqttClientFactory = mqttClientFactory;
            _options = options;
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
                        command = "project_file",
                        sequence_id = 0,
                        param = "",
                        project_id = 0,
                        profile_id = 0,
                        task_id = 0,
                        subtask_id = 0,
                        subtask_name = "",
                        file = fileName,
                        url = "file:///mnt/sdcard",
                        md5 = "",
                        timelapse = false,
                        bed_type = "auto",
                        bed_levelling = true,
                        flow_cali = true,
                        vibration_cali = true,
                        layer_inspect = true,
                        ams_mapping = "",
                        use_ams = true
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

        public string GetIdentifier() => _options.Identifier;

        public async Task<JobStatus> GetJobStatusAsync()
        {
            return await GetStatusAsync();
        }

        public async Task<PrinterStatus> GetStatusAsync()
        {
            using IMqttClient client = _MqttClientFactory.CreateMqttClient();

            SingleMessageClientWaiter waiter = new SingleMessageClientWaiter(client);
            
            await client.ConnectAsync(CreateClientOptions());

            if (client.IsConnected)
            {
                await client.SubscribeAsync("device/" + _options.SerialNunber + "/report");

                var PushBody = new
                {
                    pushing = new
                    {
                        command = "pushall"
                    }
                };

                string pushjson = JsonConvert.SerializeObject(PushBody);

                MqttApplicationMessage Pushmsg = new MqttApplicationMessage();
                Pushmsg.Topic = $"device/{_options.SerialNunber}";
                Pushmsg.Payload = new System.Buffers.ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(pushjson));
                Pushmsg.QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce;

                await client.PublishAsync(Pushmsg);

                var msg = await waiter.WaitForMessage();

                await client.DisconnectAsync();

                string jsondata = Encoding.UTF8.GetString(msg);

                BambuPrintInfoRoot? data = JsonConvert.DeserializeObject<BambuPrintInfoRoot>(jsondata ?? string.Empty);

                if (data != null)
                {
                    return new PrinterStatus()
                    {
                        PrinterName = _options.PrinterName,
                        FileName = data.print.SubtaskName,
                        Progress = data.print.PrintPercent,
                        Status = data.print.StatusMessage,
                        PrinterCam = data.print.IpcamInfo.RtspUrl
                    };
                }
            }

            return new PrinterStatus() { PrinterName = _options.PrinterName };
        }

        public async Task UploadAsync(string FilePath)
        {
            FileInfo fileinfo = new FileInfo(FilePath);

            FtpConfig config = new FtpConfig()
            {
                ValidateAnyCertificate = true,
                EncryptionMode = FtpEncryptionMode.Explicit,
                SslProtocols = SslProtocols.Tls12,
                CustomStream = typeof(GnuTlsStream),
                CustomStreamConfig = new GnuConfig()
                {
                    SecuritySuite = GnuSuite.Secure128,
                    SetALPNControlConnection = string.Empty,
                    SetALPNDataConnection = string.Empty
                }
            };

            using AsyncFtpClient client = new AsyncFtpClient(_options.PrinterIP,"bblp",_options.AccessCode, 990, config);

            client.LegacyLogger = (a, b) => _Logger.LogInformation(b);
            
            
            await client.AutoConnect();

            if (client.IsConnected || client.IsAuthenticated)
            {
               var status = await client.UploadFile(FilePath, fileinfo.Name);

                if (status == FtpStatus.Failed)
                {
                    _Logger.LogError("Failed to upload file");
                }
            }
        }

        private MqttClientOptions CreateClientOptions()
        {
            byte[] Passcode = Encoding.UTF8.GetBytes(_options.AccessCode);

            MqttClientTlsOptions tlsoptions = new MqttClientTlsOptionsBuilder()
                                              .UseTls()
                                              .WithAllowUntrustedCertificates()
                                              .WithCertificateValidationHandler((_) => true)
                                              .Build();

            MqttClientOptions options = new MqttClientOptionsBuilder()
                                        .WithTcpServer(_options.PrinterIP)
                                        .WithProtocolVersion(MqttProtocolVersion.V311)
                                        .WithCleanSession()
                                        .WithClientId("PrintProxyService")
                                        .WithTlsOptions(tlsoptions)
                                        .WithCredentials("bblp",_options.AccessCode)
                                        .Build();
            return options;
        }

    }
}
