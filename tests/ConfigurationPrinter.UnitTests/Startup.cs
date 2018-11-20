using System;
using System.Collections.Generic;
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

            services.PrintOptions(x =>
            {
                x.IgnoreMicrosoftOptions = true;
                x.MaxOptionsLength = 1000;
                x.MaskedProperties = new List<string>
                {
                    nameof(TestOptions.Password)
                };
            });
        }
    }

    public class TestOptions
    {
        public string Password = "super_secret_password";
        public string One { get; set; } = "One";

        public int Two { get; set; } = 2;
    }
}
