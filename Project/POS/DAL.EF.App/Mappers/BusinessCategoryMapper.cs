using AutoMapper;
using DAL.Base;
using DalDto = DAL.DTO;
using DomainApp = Domain.App;

namespace DAL.EF.App.Mappers;

public class BusinessCategoryMapper : BaseMapper<DalDto.BusinessCategory, DomainApp.BusinessCategory>
{
    public BusinessCategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}