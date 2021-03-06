﻿using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Nomailme.ConfigurationPrinter
{
    /// <summary>
    /// Extension class for <see cref="IServiceCollection"/> that allows printing configured options.
    /// </summary>
    public static class ConfigurationPrinterExtensions
    {
        /// <summary>
        /// Print all user configured configured options.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="config">Configuration parameters.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection PrintOptions(this IServiceCollection services, Action<ConfigurationPrinterOptions> config)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            var configuration = new ConfigurationPrinterOptions();
            config(configuration);

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            ILogger logger = loggerFactory.CreateLogger("ConfigurationPrinter");

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MaxDepth = 10,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
                Formatting = Formatting.Indented,
                ContractResolver = new MaskingContractResolver(configuration.MaskedProperties) 
            };
            foreach (ServiceDescriptor service in services)
            {
                if (!CheckIfIsOption(service.ServiceType))
                {
                    continue;
                }

                Type typeInQuestion = service.ServiceType;

                if (ShouldSkipThisConfiguration(typeInQuestion, configuration))
                {
                    continue;
                }

                Type registeredType = typeInQuestion.GetGenericArguments().First();
                Type configuredOptionsType = typeof(IOptions<>).MakeGenericType(registeredType);
                try
                {
                    object configuredOptions = serviceProvider.GetService(configuredOptionsType);
                    object value = GetOptionsValue(configuredOptions);

                    string result = JsonConvert.SerializeObject(value, settings);

                    if (result.Length > configuration.MaxOptionsLength)
                    {
                        continue;
                    }

                    var builder = new StringBuilder();
                    builder.AppendLine($"{registeredType.Name} ({registeredType.FullName})");
                    builder.Append(result);

                    logger.LogInformation(builder.ToString());
                }
                catch (Exception ex)
                {
                    logger.LogWarning($"Unable to print type {typeInQuestion.Name}{Environment.NewLine}{ex.Message}");
                }
            }

            return services;
        }

        private static bool CheckIfIsOption(Type service)
        {
            if (service.IsGenericType == false)
            {
                return false;
            }

            return service.GetGenericTypeDefinition()?.IsAssignableFrom(typeof(IConfigureOptions<>)) ?? false;
        }

        private static object GetOptionsValue(object configuredOptions)
        {
            PropertyInfo property = configuredOptions.GetType()
                .GetRuntimeProperties()
                .SingleOrDefault(x => string.Equals(x.Name, "Value"));

            return property.GetValue(configuredOptions);
        }

        private static bool ShouldSkipThisConfiguration(Type type, ConfigurationPrinterOptions configuration)
        {
            if (configuration.IgnoreMicrosoftOptions == false)
            {
                return false;
            }

            string fullName = type.GetGenericArguments().FirstOrDefault()?.FullName ?? string.Empty;
            if (fullName.StartsWith("Microsoft"))
            {
                return true;
            }

            return false;
        }
    }
}
