using System;
using System.Collections.Generic;
using DR.Stream2OdTokenDemo.Models;

namespace DR.Stream2OdTokenDemo
{
    static class AssetLinkDemoFactory
    {
        public static VideoAssetLink[] GenerateDemoLinks(string key)
        {
            // https://drhlstestx.akamaized-staging.net/hls/live/2000342/drhlstestx/master-archive.m3u8?startTime=1584521074&endTime=1584521134&hdnts=st=1584521211~exp=1585521211~acl=/*~data=st=1584521074,et=1584521134~hmac=ccdfabeca067be9c906b61972051798e69a04afd73cb8237d69b6dc9e5803454
            var akamaiHost = "https://drhlstestx.akamaized-staging.net";
            var akamaiPath = "/hls/live/2000342/drhlstestx";

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
