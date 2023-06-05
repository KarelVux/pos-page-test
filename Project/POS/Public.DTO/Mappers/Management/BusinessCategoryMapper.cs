using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Management;

public class BusinessCategoryMapper : BaseMapper<BLL.DTO.BusinessCategory, Public.DTO.v1.Management.BusinessCategory>
{
    public BusinessCategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}