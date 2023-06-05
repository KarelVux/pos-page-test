using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Manager;

public class SettlementMapper : BaseMapper<BLL.DTO.Settlement, Public.DTO.v1.Manager.Settlement>
{
    public SettlementMapper(IMapper mapper) : base(mapper)
    {
    }
}