using Contracts.Base;
using DAL.Contracts.App;
using DAL.EF.Base;

namespace DAL.EF.App.Repositories;

public class ProductCategoryRepository :
    EFBaseRepository<DAL.DTO.ProductCategory, Domain.App.ProductCategory, ApplicationDbContext>,
    IProductCategoryRepository
{
    public ProductCategoryRepository(ApplicationDbContext dataContext,
        IMapper<DAL.DTO.ProductCategory, Domain.App.ProductCategory> mapper) : base(dataContext, mapper)
    {
    }
}