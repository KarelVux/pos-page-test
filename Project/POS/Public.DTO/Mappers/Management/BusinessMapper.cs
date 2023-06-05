using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Management;

public class BusinessMapper: BaseMapper<BLL.DTO.Business, Public.DTO.v1.Management.Business>
{
    public BusinessMapper(IMapper mapper) : base(mapper)
    {
    }
}