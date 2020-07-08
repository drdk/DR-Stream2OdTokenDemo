using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using BookBeat.Akamai.EdgeAuthToken;
using Microsoft.Extensions.Configuration;

namespace DR.Stream2OdTokenDemo
{
    class Program
    {
        public static string GenerateToken(string key, string acl, string payload, string clientIp)
        {
            var tokenConfig = new AkamaiTokenConfig
            {
                Window = 86400, // 24h, Time to live (in seconds)
                Acl = acl, // Access control list containing token permissions
                Key = key, // Encryption key
                Ip = clientIp, // end user's ip
                Payload = payload
            };
            var tokenGenerator = new AkamaiTokenGenerator();

            return tokenGenerator.GenerateToken(tokenConfig);
        }

        private const bool UseHeader = true;
        static void Main(string[] args)
        {
            var cfg = new DemoSettings();
            GetConfiguration().Bind(cfg);

#if BUILDINFACTORY
            var testData = AssetLinkDemoFactory.GenerateDemoLinks(cfg.Key);
#else
            var testData =
                JsonSerializer.Deserialize<Models.VideoAsset[]>(
                new WebClient().DownloadString(
                "http://odpstst01:9200/api/OnlineAsset/GetVideoAssetByProductionId?productionId=00061898130")).SelectMany(va => va.VideoAssetLinks)
                    .Where(val => val.Format == "HLS")
                    .ToArray();

#endif
            var s = JsonSerializer.Serialize(testData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                IgnoreNullValues = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            Console.WriteLine("...\nVideoAssetLinks : ");
            Console.WriteLine(s);
            Console.WriteLine(",\n...\n");

            var externalIp = new WebClient().DownloadString("http://icanhazip.com").Replace("\n",string.Empty);
            Console.WriteLine($"Client ip detected to : {externalIp}");

            foreach (var testLink in testData)
            {
                Uri TestTarget;
                if (!testLink.IsTokenProtected)
                {
                    TestTarget = testLink.AssetSourceLink;
                }
                else
                {
                    // unlock protected link
                    var token = GenerateToken(cfg.Key, testLink.TokenAcl, testLink.TokenPayload, externalIp);
                    var uriBuilder = new UriBuilder(testLink.AssetSourceLink);
                    uriBuilder.Query += $"&{testLink.TokenQueryName}={token}";
                    TestTarget = uriBuilder.Uri;
                }

                Console.WriteLine($"Testing : {TestTarget}");
                var psi = new ProcessStartInfo
                {
                    FileName = TestTarget.ToString(),
                    UseShellExecute = true
                };
                
                //Process.Start(psi);

                var request = WebRequest.Create(TestTarget);
                WebResponse response;
                try
                {
                    response = request.GetResponse();

                    Debug.Assert((response as HttpWebResponse).StatusCode == HttpStatusCode.OK);

                    var masterString = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                    
                   
                    Console.WriteLine("OK!\n");

                }
                catch (WebException e)
                {
                    Console.Error.WriteLine("auth failed " + e.Message);
                    //throw;
                }
                Console.WriteLine();
            }

            //Console.WriteLine("Press Enter to quit.");
            //Console.ReadLine();
        }

        private static IConfigurationRoot GetConfiguration() =>
            new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
    }
}
