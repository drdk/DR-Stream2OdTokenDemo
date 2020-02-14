using System;
using System.Collections.Generic;
using DR.Stream2OdTokenDemo.Models;

namespace DR.Stream2OdTokenDemo
{
    static class AssetLinkDemoFactory
    {
        public static VideoAssetLink[] GenerateDemoLinks(string key)
        {
            var akamaiHost = "https://dr01-lh.akamaihd.net";
            var akamaiEvent = "dr01_0@147054";

            var now = DateTimeOffset.UtcNow;

            var startSec = now.AddMinutes(-5).ToUnixTimeSeconds();
            var endSec = now.ToUnixTimeSeconds();
            var payload = $"{startSec:####}-{endSec:####}";

            var dkAcl = $"/i/dk/{akamaiEvent}/*";
            var dkUri = $"{akamaiHost}/i/dk/{akamaiEvent}/master.m3u8?start={startSec:####}&end={endSec:####}&hdnea={Program.GenerateToken(key,dkAcl,payload,string.Empty)}";
            var euUri = $"{akamaiHost}/i/eu/{akamaiEvent}/master.m3u8?start={startSec:####}&end={endSec:####}";
            var euAcl = $"/i/eu/{akamaiEvent}/*";

            return new[]
            {
                new VideoAssetLink
                {
                    AssetSourceLink = new Uri(dkUri),
                    DeliveryType = "Stream",
                    Format = "HLS",
                    IPAvailability = "DK",
                    IsTokenProtected = false
                },
                new VideoAssetLink
                {
                    AssetSourceLink = new Uri(euUri),
                    DeliveryType = "Stream",
                    Format = "HLS",
                    IPAvailability = "EU",
                    IsTokenProtected = true,
                    TokenAcl = euAcl,
                    TokenPayload = payload
                },
            };
        }
    }
}
