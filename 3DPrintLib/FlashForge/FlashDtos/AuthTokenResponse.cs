using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    public class AuthTokenResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
