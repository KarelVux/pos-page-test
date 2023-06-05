using AutoMapper;
using DAL.Base;
using DalDto = DAL.DTO;
using DomainApp = Domain.App;

namespace DAL.EF.App.Mappers;

public class BusinessMapper : BaseMapper<DalDto.Business, DomainApp.Business>
{
    public BusinessMapper(IMapper mapper) : base(mapper)
    {
    }
}