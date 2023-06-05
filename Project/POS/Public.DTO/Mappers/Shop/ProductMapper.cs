using DAL.Base;
using AutoMapper;

namespace Public.DTO.Mappers.Shop;

public class ProductCategoryMapper : BaseMapper<BLL.DTO.ProductCategory, Public.DTO.v1.Shop.ProductCategory>
{
    public ProductCategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}