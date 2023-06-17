﻿using Smartwyre.DeveloperTest.Types;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Smartwyre.DeveloperTest.Data
{
    public class ProductDataStore : IProductDataStore
    {
        private readonly RebateDbContext dbContext;

        public ProductDataStore(RebateDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Product> GetProductAsync(string productIdentifier)
        {
            return await dbContext.Products.FirstOrDefaultAsync(p => p.Identifier == productIdentifier);
        }
    }
}
