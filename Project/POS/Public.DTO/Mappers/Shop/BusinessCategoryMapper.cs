using DAL.Base;
using AutoMapper;

namespace Public.DTO.Mappers.Shop;

public class BusinessCategoryMapper : BaseMapper<BLL.DTO.BusinessCategory, Public.DTO.v1.Shop.BusinessCategory>
{
    public BusinessCategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}