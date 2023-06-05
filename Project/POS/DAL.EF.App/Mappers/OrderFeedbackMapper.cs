using AutoMapper;
using DAL.Base;
using DalDto = DAL.DTO;
using DomainApp = Domain.App;

namespace DAL.EF.App.Mappers;

public class OrderFeedbackMapper : BaseMapper<DalDto.OrderFeedback, DomainApp.OrderFeedback>
{
    public OrderFeedbackMapper(IMapper mapper) : base(mapper)
    {
    }
}