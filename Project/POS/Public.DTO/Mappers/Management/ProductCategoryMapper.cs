using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Management;

public class ProductCategoryMapper : BaseMapper<BLL.DTO.ProductCategory, Public.DTO.v1.Management.ProductCategory>
{
    public ProductCategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}