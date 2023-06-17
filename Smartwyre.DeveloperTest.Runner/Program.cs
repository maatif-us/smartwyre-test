using System;
using System.Collections.Generic;
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
                var rebateService = ConfigureRebateService();
                var product = GetProductInfo();
                var rebate = GetRebateInfo();

                await rebateService.InsertRebateAndProduct(rebate, product);

                decimal volume = GetDecimalUserInput("Enter Volume : ");

                // Create the request object
                var request = new CalculateRebateRequest
                {
                    RebateIdentifier = rebate.Identifier,
                    ProductIdentifier = product.Identifier,
                    Volume = volume
                };

                // Call the Calculate method
                CalculateRebateResult result = await rebateService.CalculateAsync(request);

                // Process the result
                if (result.Success)
                {
                    Console.WriteLine($"Rebate calculation successful!  Rebate Amount {result.RebateAmount}");
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

        private static IRebateService ConfigureRebateService()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddDbContext<RebateDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")))
                .AddScoped<IRebateDataStore, RebateDataStore>()
                .AddScoped<IProductDataStore, ProductDataStore>()
                .AddScoped<IRebateService, RebateService>()
                .BuildServiceProvider();

            return serviceProvider.GetService<IRebateService>();
        }

        private static Product GetProductInfo()
        {
            Console.WriteLine(" Product Info ");
            string productIdentifier = GetUserInput("Enter Identifier: ");
            decimal productPrice = GetDecimalUserInput("Enter Price: ");
            string productUOM = GetUserInput("Enter UOM: ");
            SupportedIncentiveType productIncentive = (SupportedIncentiveType)GetIncentiveUserInput<SupportedIncentiveType>("Select an Incentive:");
            return new Product
            {
                Identifier = productIdentifier,
                Price = productPrice,
                Uom = productUOM,
                SupportedIncentives = productIncentive
            };
            //decimal volume = GetDecimalUserInput("Enter Incentives: ");
        }

        private static Rebate GetRebateInfo()
        {
            Console.WriteLine(" Rebate Info ");
            string rebateIdentifier = GetUserInput("Enter Identifier: ");
            decimal rebateAmount = GetDecimalUserInput("Enter Price: ");
            IncentiveType rebateIncentive = (IncentiveType)GetIncentiveUserInput<IncentiveType>("Select an Incentive:");
            decimal rebatePercentage = GetDecimalUserInput("Enter Rebate Percenatge: ");
            return new Rebate
            {
                Identifier = rebateIdentifier,
                Incentive = rebateIncentive,
                Amount = rebateAmount,
                Percentage = rebatePercentage
            };
        }

        private static int GetIncentiveUserInput<T>(string message) where T : Enum
        {
            Console.WriteLine(message);
            int index = 0;

            foreach (T incentive in Enum.GetValues(typeof(T)))
            {
                Console.WriteLine($"{index}: {incentive}");
                index++;
            }

            int value;
            string input = Console.ReadLine();
            while (!int.TryParse(input, out value) || value < 0 || value >= index)
            {
                Console.WriteLine("Invalid input. Please enter a valid integer value.");
                Console.Write(message);
                input = Console.ReadLine();
            }

            return value;
        }

        private static string GetUserInput(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        private static decimal GetDecimalUserInput(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine();
            decimal value;
            while (!decimal.TryParse(input, out value))
            {
                Console.WriteLine("Invalid input. Please enter a valid decimal value.");
                Console.Write(message);
                input = Console.ReadLine();
            }
            return value;
        }
    }
}
