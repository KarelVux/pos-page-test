using AutoMapper;
using DAL.Base;
using DalDto = DAL.DTO;
using DomainApp = Domain.App;

namespace DAL.EF.App.Mappers;

public class SettlementMapper : BaseMapper<DalDto.Settlement, DomainApp.Settlement>
{
    public SettlementMapper(IMapper mapper) : base(mapper)
    {
    }
}