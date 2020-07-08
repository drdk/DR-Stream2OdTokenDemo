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
            var payloadDk = $"st={startSec:####},et={endSec:####},geo=dk";
            var payloadEu = $"st={startSec:####},et={endSec:####},geo=eu";

            var dkAcl = $"/hls/live/2014202/drtest/*";
            var dkUri = $"{akamaiHost}{akamaiPath}/master-archive.m3u8?startTime={startSec:####}&endTime={endSec:####}&hdnts={Program.GenerateToken(key,dkAcl,payloadDk,string.Empty)}";
            var euUri = $"{akamaiHost}{akamaiPath}/master-archive.m3u8?startTime={startSec:####}&endTime={endSec:####}";
            var euAcl = "/hls/live/2014202/drtest/*";

            return new[]
            {
                new VideoAssetLink
                {
                    AssetSourceLink = new Uri(dkUri),
                    DeliveryType = "Stream",
                    Format = "HLS",
                    IpAvailability = "DK",
                    IsTokenProtected = false,
                    IsStreamLive = false
                },
                new VideoAssetLink
                {
                    AssetSourceLink = new Uri(euUri),
                    DeliveryType = "Stream",
                    Format = "HLS",
                    IpAvailability = "EU",
                    IsTokenProtected = true,
                    TokenAcl = euAcl,
                    TokenPayload = payloadEu,
                    IsStreamLive = false,
                    TokenKeyId = "stream2od-test",
                    TokenVersion = "edge-auth-1",
                    TokenQueryName = "hdnts"
                },
            };
        }
    }
}
