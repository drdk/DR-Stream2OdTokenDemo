using System;
using System.Collections.Generic;
using DR.Stream2OdTokenDemo.Models;

namespace DR.Stream2OdTokenDemo
{
    static class AssetLinkDemoFactory
    {
        public static VideoAssetLink[] GenerateDemoLinks(string key)
        {
            var akamaiHost = "https://drtesthls.akamaized.net";
            var akamaiPath = "/hls/live/2014202/drtest";

            var now = DateTimeOffset.UtcNow;

            var startSec = now.AddHours(-1).AddMinutes(-5).ToUnixTimeSeconds();
            var endSec = now.AddHours(-1).ToUnixTimeSeconds();
            var payload = $"st={startSec:####},et={endSec:####}";   

            var dkAcl = $"/*";
            var dkUri = $"{akamaiHost}{akamaiPath}/master-archive.m3u8?startTime={startSec:####}&endTime={endSec:####}&hdnts={Program.GenerateToken(key,dkAcl,payload,string.Empty)}";
            var euUri = $"{akamaiHost}{akamaiPath}/master-archive.m3u8?startTime={startSec:####}&endTime={endSec:####}";
            var euAcl = "/*";

            return new[]
            {
                new VideoAssetLink
                {
                    AssetSourceLink = new Uri(dkUri),
                    DeliveryType = "Stream",
                    Format = "HLS",
                    IPAvailability = "DK",
                    IsTokenProtected = false,
                    IsStreamLive = false
                },
                new VideoAssetLink
                {
                    AssetSourceLink = new Uri(euUri),
                    DeliveryType = "Stream",
                    Format = "HLS",
                    IPAvailability = "EU",
                    IsTokenProtected = true,
                    TokenAcl = euAcl,
                    TokenPayload = payload,
                    IsStreamLive = false
                },
            };
        }
    }
}
