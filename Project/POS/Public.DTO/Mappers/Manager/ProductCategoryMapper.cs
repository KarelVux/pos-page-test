using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Manager;

public class ProductCategoryMapper: BaseMapper<BLL.DTO.ProductCategory, Public.DTO.v1.Manager.ProductCategory>
{
    public ProductCategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}