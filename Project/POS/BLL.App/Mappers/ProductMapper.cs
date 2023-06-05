using AutoMapper;
using DAL.Base;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers;

public class ProductMapper : BaseMapper<BllDto.Product, DalDto.Product>
{
    public ProductMapper(IMapper mapper) : base(mapper)
    {
    }
}
public class ProductCustomMapper : BaseCustomMapper<BllDto.Product, DalDto.Product>
{
    public override BllDto.Product? Map(DalDto.Product? entity)
    {
        return entity != null
            ? new BllDto.Product
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

    public override DalDto.Product? Map(BllDto.Product? entity)
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
}