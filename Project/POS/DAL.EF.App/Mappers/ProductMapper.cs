using AutoMapper;
using DAL.Base;
using DalDto = DAL.DTO;
using DomainApp = Domain.App;

namespace DAL.EF.App.Mappers;

public class ProductMapper : BaseMapper<DalDto.Product, DomainApp.Product>
{
    public ProductMapper(IMapper mapper) : base(mapper)
    {
    }
}
public class ProductCustomMapper : BaseCustomMapper<DalDto.Product, DomainApp.Product>
{
    public override DalDto.Product? Map(DomainApp.Product? entity)
    {
        return entity != null
            ? new DalDto.Product
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                UnitPrice = entity.UnitPrice,
                UnitDiscount = entity.UnitDiscount,
                UnitCount = entity.UnitCount,
                TaxPercent = entity.TaxPercent,
                Currency = entity.Currency,
                Frozen = entity.Frozen,
                ProductCategoryId = entity.ProductCategoryId,
                BusinessId = entity.BusinessId,

            }
            : null;        
    }

    public override DomainApp.Product? Map(DalDto.Product? entity)
    {
        return entity != null
            ? new DomainApp.Product
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                UnitPrice = entity.UnitPrice,
                UnitDiscount = entity.UnitDiscount,
                UnitCount = entity.UnitCount,
                TaxPercent = entity.TaxPercent,
                Currency = entity.Currency,
                Frozen = entity.Frozen,
                ProductCategoryId = entity.ProductCategoryId,
                BusinessId = entity.BusinessId,
            }
            : null;  
    }
}