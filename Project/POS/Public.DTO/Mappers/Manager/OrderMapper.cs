using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Manager;

public class OrderMapper: BaseMapper<BLL.DTO.Order, Public.DTO.v1.Manager.Order>
{
    public OrderMapper(IMapper mapper) : base(mapper)
    {
    }
}