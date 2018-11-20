using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace ConfigurationPrinter.UnitTests
{
    /// <summary>
    /// Testing tests for fun.
    /// </summary>
    public class ForTests
    {
        private readonly ITestOutputHelper helper;

        public ForTests(ITestOutputHelper helper)
        {
            this.helper = helper;
        }

        /// <summary>
        /// Testing test.
        /// </summary>
        [Fact]
        public void Test()
        {
        }

        [Fact]
        public void UsersApi_IntegrationTests()
        {
            var builder = new StringBuilder();
            IWebHostBuilder webHostBuilder = new WebHostBuilder().UseStartup<Startup>();
            webHostBuilder.ConfigureLogging(x => x.AddProvider(new XunitLoggerProvider(helper, msg => builder.AppendLine(msg))));
            var testServer = new TestServer(webHostBuilder);
            Assert.Contains("\"One\": \"One\"", builder.ToString());
            Assert.Contains("\"Two\": 2", builder.ToString());
        }
    }
}
