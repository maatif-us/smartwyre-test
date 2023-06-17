using Moq;
using System.Threading.Tasks;
using Xunit;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateServiceTests
{
    [Fact]
    public async Task CalculateAsync_ValidFixedCashAmount_ReturnsSuccessfulCalculation()
    {
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        var productDataStoreMock = new Mock<IProductDataStore>();

        var rebateService = new RebateService(rebateDataStoreMock.Object, productDataStoreMock.Object);

        var rebateIdentifier = "rebate1";
        var productIdentifier = "product1";
        var volume = 10m;

        var rebate = new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 50m
        };

        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount,
            Price = 100m
        };

        rebateDataStoreMock.Setup(mock => mock.GetRebateAsync(rebateIdentifier)).ReturnsAsync(rebate);
        productDataStoreMock.Setup(mock => mock.GetProductAsync(productIdentifier)).ReturnsAsync(product);

        // Act
        var result = await rebateService.CalculateAsync(new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume
        });

        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public async Task CalculateAsync_ValidFixedRateRebate_ReturnsSuccessfulCalculation()
    {
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        var productDataStoreMock = new Mock<IProductDataStore>();

        var rebateService = new RebateService(rebateDataStoreMock.Object, productDataStoreMock.Object);

        var rebateIdentifier = "rebate2";
        var productIdentifier = "product2";
        var volume = 5m;

        var rebate = new Rebate
        {
            Incentive = IncentiveType.FixedRateRebate,
            Percentage = 0.1m
        };

        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
            Price = 200m
        };

        rebateDataStoreMock.Setup(mock => mock.GetRebateAsync(rebateIdentifier)).ReturnsAsync(rebate);
        productDataStoreMock.Setup(mock => mock.GetProductAsync(productIdentifier)).ReturnsAsync(product);

        // Act
        var result = await rebateService.CalculateAsync(new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume
        });

        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public async Task CalculateAsync_ValidAmountPerUom_ReturnsSuccessfulCalculation()
    {
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        var productDataStoreMock = new Mock<IProductDataStore>();

        var rebateService = new RebateService(rebateDataStoreMock.Object, productDataStoreMock.Object);

        var rebateIdentifier = "rebate3";
        var productIdentifier = "product3";
        var volume = 10m;

        var rebate = new Rebate
        {
            Incentive = IncentiveType.AmountPerUom,
            Amount = 5m
        };

        var product = new Product
        {
            Price = 100m,
            SupportedIncentives = SupportedIncentiveType.AmountPerUom
        };

        rebateDataStoreMock.Setup(mock => mock.GetRebateAsync(rebateIdentifier)).ReturnsAsync(rebate);
        productDataStoreMock.Setup(mock => mock.GetProductAsync(productIdentifier)).ReturnsAsync(product);

        // Act
        var result = await rebateService.CalculateAsync(new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume
        });

        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public async Task CalculateAsync_InvalidFixedCashAmount_ReturnsFailedCalculation()
    {
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        var productDataStoreMock = new Mock<IProductDataStore>();

        var rebateService = new RebateService(rebateDataStoreMock.Object, productDataStoreMock.Object);

        var rebateIdentifier = "rebate4";
        var productIdentifier = "product4";
        var volume = 10m;

        var rebate = new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 0m
        };

        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount,
            Price = 100m
        };

        rebateDataStoreMock.Setup(mock => mock.GetRebateAsync(rebateIdentifier)).ReturnsAsync(rebate);
        productDataStoreMock.Setup(mock => mock.GetProductAsync(productIdentifier)).ReturnsAsync(product);

        // Act
        var result = await rebateService.CalculateAsync(new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume
        });

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public async Task CalculateAsync_InvalidFixedRateRebate_ReturnsFailedCalculation()
    {
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        var productDataStoreMock = new Mock<IProductDataStore>();

        var rebateService = new RebateService(rebateDataStoreMock.Object, productDataStoreMock.Object);

        var rebateIdentifier = "rebate5";
        var productIdentifier = "product5";
        var volume = 0m;

        var rebate = new Rebate
        {
            Incentive = IncentiveType.FixedRateRebate,
            Percentage = 0.1m
        };

        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
            Price = 200m
        };

        rebateDataStoreMock.Setup(mock => mock.GetRebateAsync(rebateIdentifier)).ReturnsAsync(rebate);
        productDataStoreMock.Setup(mock => mock.GetProductAsync(productIdentifier)).ReturnsAsync(product);

        // Act
        var result = await rebateService.CalculateAsync(new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume
        });

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public async Task CalculateAsync_InvalidAmountPerUom_ReturnsFailedCalculation()
    {
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        var productDataStoreMock = new Mock<IProductDataStore>();

        var rebateService = new RebateService(rebateDataStoreMock.Object, productDataStoreMock.Object);

        var rebateIdentifier = "rebate6";
        var productIdentifier = "product6";
        var volume = 0;
        var rebate = new Rebate
        {
            Incentive = IncentiveType.AmountPerUom,
            Amount = 5m
        };

        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.AmountPerUom,
            Price = 100m
        };

        rebateDataStoreMock.Setup(mock => mock.GetRebateAsync(rebateIdentifier)).ReturnsAsync(rebate);
        productDataStoreMock.Setup(mock => mock.GetProductAsync(productIdentifier)).ReturnsAsync(product);

        // Act
        var result = await rebateService.CalculateAsync(new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume
        });

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public async Task CalculateAsync_RebateNotFound_ReturnsFailedCalculation()
    {
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        var productDataStoreMock = new Mock<IProductDataStore>();

        var rebateService = new RebateService(rebateDataStoreMock.Object, productDataStoreMock.Object);

        var rebateIdentifier = "rebate7";
        var productIdentifier = "product7";
        var volume = 10m;

        rebateDataStoreMock.Setup(mock => mock.GetRebateAsync(rebateIdentifier)).ReturnsAsync((Rebate)null);

        // Act
        var result = await rebateService.CalculateAsync(new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume
        });

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public async Task CalculateAsync_ProductNotFound_ReturnsFailedCalculation()
    {
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        var productDataStoreMock = new Mock<IProductDataStore>();

        var rebateService = new RebateService(rebateDataStoreMock.Object, productDataStoreMock.Object);

        var rebateIdentifier = "rebate8";
        var productIdentifier = "product8";
        var volume = 10m;

        var rebate = new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 50m
        };

        rebateDataStoreMock.Setup(mock => mock.GetRebateAsync(rebateIdentifier)).ReturnsAsync(rebate);
        productDataStoreMock.Setup(mock => mock.GetProductAsync(productIdentifier)).ReturnsAsync((Product)null);

        // Act
        var result = await rebateService.CalculateAsync(new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume
        });

        // Assert
        Assert.False(result.Success);
    }
}




