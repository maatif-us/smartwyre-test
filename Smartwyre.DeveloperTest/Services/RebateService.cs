using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Services
{
    public class RebateService : IRebateService
    {
        private readonly IRebateDataStore rebateDataStore;
        private readonly IProductDataStore productDataStore;

        public RebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore)
        {
            this.rebateDataStore = rebateDataStore;
            this.productDataStore = productDataStore;
        }

        public async Task InsertRebateAndProduct(Rebate rebate, Product product)
        {
            await rebateDataStore.SaveRebateAsync(rebate);
            await productDataStore.SaveProductAsync(product);
        }

        public async Task<CalculateRebateResult> CalculateAsync(CalculateRebateRequest request)
        {

            Rebate rebate = await rebateDataStore.GetRebateAsync(request.RebateIdentifier);
            Product product = await productDataStore.GetProductAsync(request.ProductIdentifier);
            SupportedIncentiveType productIncentive = product.SupportedIncentives;

            var result = new CalculateRebateResult();
            decimal rebateAmount = 0m;

            if (rebate == null || product == null)
            {
                result.Success = false;
                return result;
            }
            else
            {
                // Handle specific incentive types
                switch (rebate.Incentive)
                {
                    case IncentiveType.FixedCashAmount:
                        if (rebate.Amount == 0 || !productIncentive.HasFlag(SupportedIncentiveType.FixedCashAmount))
                        {
                            result.Success = false;
                        }
                        else
                        {
                            rebateAmount = rebate.Amount;
                            result.Success = true;
                        }
                        break;

                    case IncentiveType.FixedRateRebate:
                        if (rebate.Percentage == 0 || product.Price == 0 || request.Volume == 0 || !productIncentive.HasFlag(SupportedIncentiveType.FixedRateRebate))
                        {
                            result.Success = false;
                        }
                        else
                        {
                            rebateAmount += product.Price * rebate.Percentage * request.Volume;
                            result.Success = true;
                        }
                        break;

                    case IncentiveType.AmountPerUom:
                        if (rebate.Amount == 0 || request.Volume == 0 || !productIncentive.HasFlag(SupportedIncentiveType.AmountPerUom))
                        {
                            result.Success = false;
                        }
                        else
                        {
                            rebateAmount += rebate.Amount * request.Volume;
                            result.Success = true;
                        }
                        break;
                }
            }

            if (result.Success)
            {
                await rebateDataStore.StoreCalculationResultAsync(rebate, rebateAmount);
                result.RebateAmount = rebateAmount;
            }

            return result;
        }
    }
}
