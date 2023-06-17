using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data
{
    public interface IProductDataStore
    {
        Task<Product> GetProductAsync(string productIdentifier);
    }
}
