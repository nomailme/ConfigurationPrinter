using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nomailme.ConfigurationPrinter;

namespace ConfigurationPrinter.UnitTests
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TestOptions>(x => { });

            services.PrintOptions(x => { });
        }
    }

    public class TestOptions
    {
        public string One { get; set; } = "One";

        public int Two { get; set; } = 2;
    }
}
