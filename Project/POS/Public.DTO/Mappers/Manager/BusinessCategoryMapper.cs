using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Manager;

public class BusinessCategoryMapper: BaseMapper<BLL.DTO.BusinessCategory, Public.DTO.v1.Manager.BusinessCategory>
{
    public BusinessCategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}