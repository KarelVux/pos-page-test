using AutoMapper;
using DAL.Base;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers;

public class SettlementMapper : BaseMapper<BllDto.Settlement, DalDto.Settlement>
{
    public SettlementMapper(IMapper mapper) : base(mapper)
    {
    }
}