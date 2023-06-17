using Microsoft.EntityFrameworkCore;
using Smartwyre.DeveloperTest.Types;
using System.Linq;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore : IRebateDataStore
{
    private readonly RebateDbContext dbContext;

    public RebateDataStore(RebateDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Rebate> GetRebateAsync(string rebateIdentifier)
    {
        return await dbContext.Rebates.FirstOrDefaultAsync(r => r.Identifier == rebateIdentifier);
    }

    public async Task SaveRebateAsync(Rebate rebate)
    {
        var obj = await GetRebateAsync(rebate.Identifier);

        if (obj == null)
        {
            dbContext.Rebates.Add(rebate);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task StoreCalculationResultAsync(Rebate account, decimal rebateAmount)
    {
        var calculation = new RebateCalculation
        {
            RebateIdentifier = account.Identifier,
            IncentiveType = account.Incentive,
            Amount = rebateAmount
        };

        dbContext.RebateCalculations.Add(calculation);
        await dbContext.SaveChangesAsync();
    }
}
