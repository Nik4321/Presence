using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Presence.Api;
using System;
using System.IO;
using System.Net.Http;

namespace Presence.Tests.Common
{
    public class TestStartupApi : IDisposable
    {
        public readonly TestServer Server;

        private readonly HttpClient client;

        public TestStartupApi()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(this.GetContentRootPath())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new WebHostBuilder()
                .UseContentRoot(this.GetContentRootPath())
                .UseConfiguration(configuration)
                .UseEnvironment("Development")
                .UseStartup<Startup>();

            this.Server = new TestServer(builder);
            this.client = this.Server.CreateClient();
        }

        public void Dispose()
        {
            this.client.Dispose();
            this.Server.Dispose();
        }

        private string GetContentRootPath()
        {
            var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            var relativePathToHostProject = @"..\..\..\..\Presence.Api";
            return Path.Combine(testProjectPath, relativePathToHostProject);
        }
    }
}
