using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Manager;

public class BusinessMapper: BaseMapper<BLL.DTO.Business, Public.DTO.v1.Manager.Business>
{
    public BusinessMapper(IMapper mapper) : base(mapper)
    {
    }
}