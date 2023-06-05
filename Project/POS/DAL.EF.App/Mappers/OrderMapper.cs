using AutoMapper;
using DAL.Base;
using DalDto = DAL.DTO;
using DomainApp = Domain.App;

namespace DAL.EF.App.Mappers;

public class OrderMapper : BaseMapper<DalDto.Order, DomainApp.Order>
{
    public OrderMapper(IMapper mapper) : base(mapper)
    {
    }
}