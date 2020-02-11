using System;
using System.Diagnostics;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
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
                Payload = payload,
            };
            var tokenGenerator = new AkamaiTokenGenerator();

            return tokenGenerator.GenerateToken(tokenConfig);
        }

        private const bool UseHeader = true;
        static void Main(string[] args)
        {
            var cfg = new DemoSettings();
            GetConfiguration().Bind(cfg);

            
            var testData = AssetLinkDemoFactory.GenerateDemoLinks(cfg.Key);

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
                    // unklock protected link
                    var token = GenerateToken(cfg.Key, testLink.AdditionalTokenConfiguration.Acl,
                        testLink.AdditionalTokenConfiguration.Payload, externalIp);
                    var uriBuilder = new UriBuilder(testLink.AssetSourceLink);
                    uriBuilder.Query += $"&hdnea={token}";
                    TestTarget = uriBuilder.Uri;
                }

                Console.WriteLine($"Testing : {TestTarget}");
                var request = WebRequest.Create(TestTarget);
                WebResponse response;
                try
                {
                    response = request.GetResponse();
                    Debug.Assert((response as HttpWebResponse).StatusCode == HttpStatusCode.OK);
                    Console.WriteLine("OK!\n");

                }
                catch (WebException e)
                {
                    Console.Error.WriteLine("auth failed " + e.Message);
                    //throw;
                }

            }
        }

        private static IConfigurationRoot GetConfiguration() =>
            new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
    }
}
