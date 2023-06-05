using BLL.Base;
using BLL.Contracts.App;
using Contracts.Base;
using DAL.Contracts.App;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Services;

public class ProductCategoryService :
    BaseEntityService<BllDto.ProductCategory, DalDto.ProductCategory, IProductCategoryRepository>,
    IProductCategoryService
{
    protected IAppUOW Uow;

    public ProductCategoryService(IAppUOW uow, IMapper<BllDto.ProductCategory, DalDto.ProductCategory> mapper)
        : base(uow.ProductCategoryRepository, mapper)
    {
        Uow = uow;
    }
}