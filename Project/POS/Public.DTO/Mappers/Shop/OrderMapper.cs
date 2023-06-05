using DAL.Base;
using AutoMapper;

namespace Public.DTO.Mappers.Shop;

public class OrderMapper : BaseMapper<BLL.DTO.Order, Public.DTO.v1.Shop.Order>
{
    public OrderMapper(IMapper mapper) : base(mapper)
    {
    }
}