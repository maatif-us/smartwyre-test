using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true)
                    .Build();
                // Setup dependency injection
                var serviceProvider = new ServiceCollection()
                    .AddDbContext<RebateDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")))
                    .AddScoped<IRebateDataStore, RebateDataStore>()
                    .AddScoped<IProductDataStore, ProductDataStore>()
                    .AddScoped<IRebateService, RebateService>()
                    .BuildServiceProvider();

                // Resolve the rebate service
                var rebateService = serviceProvider.GetService<IRebateService>();

                string rebateIdentifier = "R2";
                string productIdentifier = "P2";
                decimal volume = 10;

                // Create the request object
                var request = new CalculateRebateRequest
                {
                    RebateIdentifier = rebateIdentifier,
                    ProductIdentifier = productIdentifier,
                    Volume = volume
                };

                // Call the Calculate method
                CalculateRebateResult result = await rebateService.CalculateAsync(request);

                // Process the result
                if (result.Success)
                {
                    Console.WriteLine("Rebate calculation successful!");
                }
                else
                {
                    Console.WriteLine("Rebate calculation failed!");
                }

                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
