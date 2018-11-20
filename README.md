# ConfigurationPrinter

[![Build status](https://ci.appveyor.com/api/projects/status/v8c8qjqla1dve1j9?svg=true)](https://ci.appveyor.com/project/nomailme/configurationprinter)

Tired of sleepless nights? Fed up of your neighbours? Painkillers don't work anymore? Worry not! I present you an extension for your ASPNet Core application that will feed your cat, walk the dog fill the tax papers. Behold... Configuration Printer

Let's say you have simple class that you want to register as `IOptions<TestOptions>`

![ExampleOptionsClass](https://github.com/nomailme/ConfigurationPrinter/blob/master/docs/assets/example.png)

Let us register it
![Registration](https://github.com/nomailme/ConfigurationPrinter/blob/master/docs/assets/usage.png)

Now, during application startup following will be printed in the output
![Output](https://github.com/nomailme/ConfigurationPrinter/blob/master/docs/assets/log_output.png)

# Usage

To print all registered configurations call the extension method at the end `ConfigureServices` method in your startup.
```csharp
services.PrintOptions(x =>
            {
                x.IgnoreMicrosoftOptions = true;
                x.MaxOptionsLength = 1000;
                x.MaskedProperties = new List<string>
                {
                    nameof(TestOptions.Password)
                };
            });
```
