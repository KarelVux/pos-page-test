using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Management;

public class SettlementMapper : BaseMapper<BLL.DTO.Settlement, Public.DTO.v1.Management.Settlement>
{
    public SettlementMapper(IMapper mapper) : base(mapper)
    {
    }
}