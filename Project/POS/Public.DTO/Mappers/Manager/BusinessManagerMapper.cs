using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Manager;

public class BusinessManagerMapper: BaseMapper<BLL.DTO.BusinessManager, Public.DTO.v1.Manager.BusinessManager>
{
    public BusinessManagerMapper(IMapper mapper) : base(mapper)
    {
    }
}