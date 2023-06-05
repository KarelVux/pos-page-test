using AutoMapper;
using DAL.Base;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers;

public class OrderFeedbackMapper : BaseMapper<BllDto.OrderFeedback, DalDto.OrderFeedback>
{
    public OrderFeedbackMapper(IMapper mapper) : base(mapper)
    {
    }
}