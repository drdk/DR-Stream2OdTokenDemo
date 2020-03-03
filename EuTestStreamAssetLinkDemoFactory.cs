using System;
using System.Collections.Generic;
using DR.Stream2OdTokenDemo.Models;

namespace DR.Stream2OdTokenDemo
{
    //https://drhlstestxeutoken.akamaized.net/hls/live/2012916/drhlstesteu/master.m3u8
    static class EuTestStreamAssetLinkDemoFactory
    {
        public static VideoAssetLink[] GenerateDemoLinks(string key)
        {
            var akamaiHost = "https://drhlstestxeutoken.akamaized.net";
            var akamaiEvent = "2012916";

            //var now = DateTimeOffset.UtcNow;

            //var startSec = now.AddMinutes(-5).ToUnixTimeSeconds();
            //var endSec = now.ToUnixTimeSeconds();
            //var payload = $"{startSec:####}-{endSec:####}";

            var euUri = $"{akamaiHost}/hls/live/{akamaiEvent}/drhlstesteu/master.m3u8";//?start={startSec:####}&end={endSec:####}";
            var euAcl =             $"/hls/live/{akamaiEvent}/drhlstesteu/*";

            return new[]
            {
                new VideoAssetLink
                {
                    AssetSourceLink = new Uri(euUri),
                    DeliveryType = "Stream",
                    Format = "HLS",
                    IPAvailability = "EU",
                    IsTokenProtected = true,
                    TokenAcl = euAcl,
                    //TokenPayload = payload,
                    IsStreamLive = false
                },
            };
        }
    }
}
