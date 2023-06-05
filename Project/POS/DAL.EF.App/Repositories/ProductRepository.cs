using Contracts.Base;
using DAL.Contracts.App;
using DAL.DTO;
using DAL.EF.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.App.Repositories;

public class ProductRepository : EFBaseRepository<DAL.DTO.Product, Domain.App.Product, ApplicationDbContext>,
    IProductRepository
{
    public ProductRepository(ApplicationDbContext dataContext, IMapper<DAL.DTO.Product, Domain.App.Product> mapper) :
        base(dataContext, mapper)
    {
    }

    /// <summary>
    /// Get all products from DB
    /// </summary>
    /// <returns>All products from DB</returns>
    public async Task<IEnumerable<DAL.DTO.Product>> AllAsyncWithInclude()
    {
        var res = await RepositoryDbSet
            .Include(p => p.Business)
            .Include(p => p.ProductCategory)
            .ToListAsync();

        return res.Select(x => Mapper.Map(x)!);
    }

    /// <summary>
    /// Return single product with include
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Single product with includes</returns>
    public async Task<DAL.DTO.Product?> FindAsyncWithInclude(Guid id)
    {
        var res = await RepositoryDbSet
            .Include(p => p.Business)
            .Include(p => p.ProductCategory)
            .FirstOrDefaultAsync(m => m.Id == id);

        return Mapper.Map(res);
    }

    /// <summary>
    /// Get business products with includes
    /// </summary>
    /// <param name="businessId">Business ID</param>
    /// <param name="isFrozen">Find products with described status</param>
    /// <returns>Business products</returns>
    public async Task<IEnumerable<DAL.DTO.Product>> GetBusinessProductsWithIncludes(Guid businessId, bool isFrozen)
    {
        var res = await RepositoryDbSet
            .Include(p => p.ProductCategory)
            .Where(m => m.BusinessId == businessId && m.Frozen == isFrozen)
            .ToListAsync();

        return res.Select(x => Mapper.Map(x)!);
    }

    /// <summary>
    /// Gets single business product with includes
    /// </summary>
    /// <param name="productId">Searchable product ID</param>
    /// <param name="businessId">Business ID</param>
    /// <param name="isFrozen">Find products with described status</param>
    /// <returns>Business product</returns>
    public async Task<DAL.DTO.Product?> GetBusinessProduct(Guid productId, Guid businessId, bool isFrozen)
    {
        var res = await RepositoryDbSet
            .FirstOrDefaultAsync(m => m.Id == productId && m.BusinessId == businessId && m.Frozen == isFrozen);

        return Mapper.Map(res);
    }

    /// <summary>
    /// Get business products with includes
    /// </summary>
    /// <param name="businessId">Business ID</param>
    /// <returns>Business products</returns>
    public async Task<IEnumerable<Product>> GetBusinessProductsWithIncludes(Guid businessId)
    {
        var res = await RepositoryDbSet
            .Include(p => p.ProductCategory)
            .Where(m => m.BusinessId == businessId)
            .ToListAsync();

        return res.Select(x => Mapper.Map(x)!);
    }

    /// <summary>
    /// Gets single business product with includes
    /// </summary>
    /// <param name="productId">Searchable product ID</param>
    /// <param name="businessId">Business ID</param>
    /// <returns>Business product</returns>
    public async Task<Product?> GetBusinessProduct(Guid productId, Guid businessId)
    {
        var res = await RepositoryDbSet
            .FirstOrDefaultAsync(m => m.Id == productId && m.BusinessId == businessId);

        return Mapper.Map(res);
    }
}