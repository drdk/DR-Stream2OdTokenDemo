using System;
using System.Diagnostics;
using System.Net;
using BookBeat.Akamai.EdgeAuthToken;
using Microsoft.Extensions.Configuration;

namespace DR.ThirdPartyTokenDemo
{
    class Program
    {
        private const bool UseHeader = true;
        static void Main(string[] args)
        {
            var cfg = new DemoSettings();
            GetConfiguration().Bind(cfg);

            var uriBuilder = new UriBuilder(cfg.TestUrl);

            var tokenConfig = new AkamaiTokenConfig
            {
                Window = 86400, // 24h, Time to live (in seconds)
                Acl = uriBuilder.Path, // Access control list containing token permissions
                Key = cfg.Key // Encryption key
            };

            var tokenGenerator = new AkamaiTokenGenerator();

            var token = tokenGenerator.GenerateToken(tokenConfig);

            if (!UseHeader)
            {
                uriBuilder.Query = "hdnea="+token;
            }

            // Test access
            var request = WebRequest.Create(uriBuilder.Uri);
            request.Method = "HEAD";
            if (UseHeader)
            {
                request.Headers.Add("hdnea", token);
            }

            WebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException e)
            {
                Console.Error.WriteLine("auth failed " +  e.Message);
                throw;
            }

            Debug.Assert((response as HttpWebResponse).StatusCode == HttpStatusCode.OK);

            Console.WriteLine("Test passed");
        }

        private static IConfigurationRoot GetConfiguration() =>
            new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
    }
}
