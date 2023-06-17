using Smartwyre.DeveloperTest.Types;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Data
{
    public interface IRebateDataStore
    {
        Task<Rebate> GetRebateAsync(string rebateIdentifier);
        Task StoreCalculationResultAsync(Rebate rebate, decimal rebateAmount);
        Task SaveRebateAsync(Rebate rebate);
    }
}
