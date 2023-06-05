using AutoMapper;
using DAL.Base;
using DalDto = DAL.DTO;
using DomainApp = Domain.App;

namespace DAL.EF.App.Mappers;

public class BusinessManagerMapper : BaseMapper<DalDto.BusinessManager, DomainApp.BusinessManager>
{
    public BusinessManagerMapper(IMapper mapper) : base(mapper)
    {
    }
}