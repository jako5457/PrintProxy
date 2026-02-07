using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    internal class FlashforgeDetail
    {
        [JsonProperty("autoShutdown")]
        public string AutoShutdown { get; set; } = string.Empty;

        [JsonProperty("autoShutdownTime")]
        public string AutoShutdownTime { get; set; } = string.Empty;

        [JsonProperty("cameraStreamUrl")]
        public string CameraStreamUrl { get; set; } = string.Empty;

        [JsonProperty("chamberFanSpeed")]
        public string ChamberFanSpeed { get; set; } = string.Empty;

        [JsonProperty("chamberTargetTemp")]
        public string ChamberTargetTemp { get; set; } = string.Empty;

        [JsonProperty("chamberTemp")]
        public string ChamberTemp { get; set; } = string.Empty;

        [JsonProperty("coolingFanSpeed")]
        public string CoolingFanSpeed { get; set; } = string.Empty;

        [JsonProperty("cumulativeFilament")]
        public string CumulativeFilament { get; set; } = string.Empty;

        [JsonProperty("cumulativePrintTime")]
        public string CumulativePrintTime { get; set; } = string.Empty;

        [JsonProperty("currentPrintSpeed")]
        public string CurrentPrintSpeed { get; set; } = string.Empty;

        [JsonProperty("doorStatus")]
        public string DoorStatus { get; set; } = string.Empty;

        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; } = string.Empty;

        [JsonProperty("estimatedLeftLen")]
        public string EstimatedLeftLen { get; set; } = string.Empty;

        [JsonProperty("estimatedLeftWeight")]
        public string EstimatedLeftWeight { get; set; } = string.Empty;

        [JsonProperty("estimatedRightLen")]
        public string EstimatedRightLen { get; set; } = string.Empty;

        [JsonProperty("estimatedRightWeight")]
        public string EstimatedRightWeight { get; set; } = string.Empty;

        [JsonProperty("estimatedTime")]
        public string EstimatedTime { get; set; } = string.Empty;

        [JsonProperty("externalFanStatus")]
        public string ExternalFanStatus { get; set; } = string.Empty;

        [JsonProperty("fillAmount")]
        public string FillAmount { get; set; } = string.Empty;

        [JsonProperty("firmwareVersion")]
        public string FirmwareVersion { get; set; } = string.Empty;

        [JsonProperty("flashRegisterCode")]
        public string FlashRegisterCode { get; set; } = string.Empty;

        [JsonProperty("internalFanStatus")]
        public string InternalFanStatus { get; set; } = string.Empty;

        [JsonProperty("ipAddr")]
        public string IpAddr { get; set; } = string.Empty;

        [JsonProperty("leftFilamentType")]
        public string LeftFilamentType { get; set; } = string.Empty;

        [JsonProperty("leftTargetTemp")]
        public string LeftTargetTemp { get; set; } = string.Empty;

        [JsonProperty("leftTemp")]
        public string LeftTemp { get; set; } = string.Empty;

        [JsonProperty("lightStatus")]
        public string LightStatus { get; set; } = string.Empty;

        [JsonProperty("location")]
        public string Location { get; set; } = string.Empty;

        [JsonProperty("macAddr")]
        public string MacAddr { get; set; } = string.Empty;

        [JsonProperty("measure")]
        public string Measure { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("nozzleCnt")]
        public string NozzleCnt { get; set; } = string.Empty;

        [JsonProperty("nozzleModel")]
        public string NozzleModel { get; set; } = string.Empty;

        [JsonProperty("nozzleStyle")]
        public string NozzleStyle { get; set; } = string.Empty;

        [JsonProperty("pid")]
        public string Pid { get; set; } = string.Empty;

        [JsonProperty("platTargetTemp")]
        public string PlatTargetTemp { get; set; } = string.Empty;

        [JsonProperty("platTemp")]
        public string PlatTemp { get; set; } = string.Empty;

        [JsonProperty("polarRegisterCode")]
        public string PolarRegisterCode { get; set; } = string.Empty;

        [JsonProperty("printDuration")]
        public string PrintDuration { get; set; } = string.Empty;

        [JsonProperty("printFileName")]
        public string PrintFileName { get; set; } = string.Empty;

        [JsonProperty("printFileThumbUrl")]
        public string PrintFileThumbUrl { get; set; } = string.Empty;

        [JsonProperty("printLayer")]
        public string PrintLayer { get; set; } = string.Empty;

        [JsonProperty("printProgress")]
        public int PrintProgress { get; set; }

        [JsonProperty("printSpeedAdjust")]
        public string PrintSpeedAdjust { get; set; } = string.Empty;

        [JsonProperty("remainingDiskSpace")]
        public string RemainingDiskSpace { get; set; } = string.Empty;

        [JsonProperty("rightFilamentType")]
        public string RightFilamentType { get; set; } = string.Empty;

        [JsonProperty("rightTargetTemp")]
        public string RightTargetTemp { get; set; } = string.Empty;

        [JsonProperty("rightTemp")]
        public string RightTemp { get; set; } = string.Empty;

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("targetPrintLayer")]
        public string TargetPrintLayer { get; set; } = string.Empty;

        [JsonProperty("tvoc")]
        public string Tvoc { get; set; } = string.Empty;

        [JsonProperty("zAxisCompensation")]
        public string ZAxisCompensation { get; set; } = string.Empty;
    }
}
