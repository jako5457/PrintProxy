using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    internal class FlashforgeDetail
    {
        [JsonPropertyName("autoShutdown")]
        public string AutoShutdown { get; set; } = string.Empty;

        [JsonPropertyName("autoShutdownTime")]
        public int AutoShutdownTime { get; set; }

        [JsonPropertyName("cameraStreamUrl")]
        public string CameraStreamUrl { get; set; } = string.Empty;

        [JsonPropertyName("chamberFanSpeed")]
        public int ChamberFanSpeed { get; set; }

        [JsonPropertyName("chamberTargetTemp")]
        public int ChamberTargetTemp { get; set; }

        [JsonPropertyName("chamberTemp")]
        public int ChamberTemp { get; set; }

        [JsonPropertyName("coolingFanSpeed")]
        public int CoolingFanSpeed { get; set; }

        [JsonPropertyName("cumulativeFilament")]
        public float CumulativeFilament { get; set; }

        [JsonPropertyName("cumulativePrintTime")]
        public long CumulativePrintTime { get; set; }

        [JsonPropertyName("currentPrintSpeed")]
        public int CurrentPrintSpeed { get; set; }

        [JsonPropertyName("doorStatus")]
        public string DoorStatus { get; set; } = string.Empty;

        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; } = string.Empty;

        [JsonPropertyName("estimatedLeftLen")]
        public int EstimatedLeftLen { get; set; }

        [JsonPropertyName("estimatedLeftWeight")]
        public double EstimatedLeftWeight { get; set; }

        [JsonPropertyName("estimatedRightLen")]
        public int EstimatedRightLen { get; set; }

        [JsonPropertyName("estimatedRightWeight")]
        public double EstimatedRightWeight { get; set; }

        [JsonPropertyName("estimatedTime")]
        public double EstimatedTime { get; set; }

        [JsonPropertyName("externalFanStatus")]
        public string ExternalFanStatus { get; set; } = string.Empty;

        [JsonPropertyName("fillAmount")]
        public int FillAmount { get; set; }

        [JsonPropertyName("firmwareVersion")]
        public string FirmwareVersion { get; set; } = string.Empty;

        [JsonPropertyName("flashRegisterCode")]
        public string FlashRegisterCode { get; set; } = string.Empty;

        [JsonPropertyName("internalFanStatus")]
        public string InternalFanStatus { get; set; } = string.Empty;

        [JsonPropertyName("ipAddr")]
        public string IpAddr { get; set; } = string.Empty;

        [JsonPropertyName("leftFilamentType")]
        public string LeftFilamentType { get; set; } = string.Empty;

        [JsonPropertyName("leftTargetTemp")]
        public int LeftTargetTemp { get; set; }

        [JsonPropertyName("leftTemp")]
        public int LeftTemp { get; set; }

        [JsonPropertyName("lightStatus")]
        public string LightStatus { get; set; } = string.Empty;

        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [JsonPropertyName("macAddr")]
        public string MacAddr { get; set; } = string.Empty;

        [JsonPropertyName("measure")]
        public string Measure { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("nozzleCnt")]
        public int NozzleCnt { get; set; }

        [JsonPropertyName("nozzleModel")]
        public string NozzleModel { get; set; } = string.Empty;

        [JsonPropertyName("nozzleStyle")]
        public int NozzleStyle { get; set; }

        [JsonPropertyName("pid")]
        public int Pid { get; set; }

        [JsonPropertyName("platTargetTemp")]
        public float PlatTargetTemp { get; set; }

        [JsonPropertyName("platTemp")]
        public float PlatTemp { get; set; }

        [JsonPropertyName("polarRegisterCode")]
        public string PolarRegisterCode { get; set; } = string.Empty;

        [JsonPropertyName("printDuration")]
        public int PrintDuration { get; set; }

        [JsonPropertyName("printFileName")]
        public string PrintFileName { get; set; } = string.Empty;

        [JsonPropertyName("printFileThumbUrl")]
        public string PrintFileThumbUrl { get; set; } = string.Empty;

        [JsonPropertyName("printLayer")]
        public int PrintLayer { get; set; }

        [JsonPropertyName("printProgress")]
        public double PrintProgress { get; set; }

        [JsonPropertyName("printSpeedAdjust")]
        public double PrintSpeedAdjust { get; set; }

        [JsonPropertyName("remainingDiskSpace")]
        public float RemainingDiskSpace { get; set; }

        [JsonPropertyName("rightFilamentType")]
        public string RightFilamentType { get; set; } = string.Empty;

        [JsonPropertyName("rightTargetTemp")]
        public double RightTargetTemp { get; set; }

        [JsonPropertyName("rightTemp")]
        public float RightTemp { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("targetPrintLayer")]
        public int TargetPrintLayer { get; set; }

        [JsonPropertyName("tvoc")]
        public int Tvoc { get; set; }

        [JsonPropertyName("zAxisCompensation")]
        public float ZAxisCompensation { get; set; }
    }
}
