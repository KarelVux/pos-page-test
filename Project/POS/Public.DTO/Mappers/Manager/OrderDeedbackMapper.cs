using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Manager;

public class OrderFeedbackMapper: BaseMapper<BLL.DTO.OrderFeedback, Public.DTO.v1.Manager.OrderFeedback>
{
    public OrderFeedbackMapper(IMapper mapper) : base(mapper)
    {
    }
}