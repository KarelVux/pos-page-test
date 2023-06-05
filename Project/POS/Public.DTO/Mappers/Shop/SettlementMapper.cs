using DAL.Base;
using AutoMapper;

namespace Public.DTO.Mappers.Shop;

public class SettlementMapper : BaseMapper<BLL.DTO.Settlement, Public.DTO.v1.Shop.Settlement>
{
    public SettlementMapper(IMapper mapper) : base(mapper)
    {
    }
}