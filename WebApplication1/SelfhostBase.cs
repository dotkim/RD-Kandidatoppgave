using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WebApplication1
{
    /// <summary>
    /// Usage: Extend your Selfhosted entrypoint (Program.cs) and invoke StartService with type of
    /// Startup (dotnet core pipeline config)
    /// </summary>
    public abstract class SelfhostBase
    {
        protected static int StartService<T>(string[] args) where T : class
        {
            try
            {
                BuildWebHost<T>(args).Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Horrible application crash:{ex.Message}");
                return 1;
            }
            return 0;
        }

        private static IWebHost BuildWebHost<T>(string[] args) where T : class
        {
            var fc = new ConfigurationBuilder()
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
                    .Build();

            var wbh = WebHost.CreateDefaultBuilder(args)
                .UseStartup<T>()
                .UseIISIntegration()
                .UseKestrel(options =>
                {
                    options.Limits.MaxRequestBodySize = GetMaxRequestSize(fc);
                });

            return wbh.Build();
        }

        private static int GetMaxRequestSize(IConfigurationRoot fileConfig)
        {
            var stringMaxRequestSize = fileConfig["App:Setup:MaxRequestSizeInMb"];
            var maxRequestSize = (!string.IsNullOrEmpty(stringMaxRequestSize) && int.TryParse(stringMaxRequestSize, out var parsed) ? parsed : 30) * 1000000; // using 30 mb as default(same as .netcore default)
            return maxRequestSize;
        }
    }
}
