using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    public class FileUploadResponse
    {
        [JsonPropertyName("filename")]
        public string Filename { get; set; } = string.Empty;

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("upload_time")]
        public long UploadTime { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
