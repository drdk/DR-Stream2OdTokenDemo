using System;

namespace DR.Stream2OdTokenDemo.Models
{
    public class VideoAssetLink
    {
        public Uri AssetSourceLink { get; set; }
        public string IpAvailability { get; set; }
        public string DeliveryType { get; set; }
        public string Format { get; set; }
        public Subtitle[] Subtitles { get; set; }
        public bool IsTokenProtected { get; set; }
        public string TokenAcl { get; set; }
        public string TokenPayload { get; set; }
        public bool? IsStreamLive { get; set; }
        public string TokenKeyId { get; set; }
        public string TokenQueryName { get; set; }
        public string TokenVersion { get; set; }
        public int? Bitrate { get; set; }
        
    }
}
