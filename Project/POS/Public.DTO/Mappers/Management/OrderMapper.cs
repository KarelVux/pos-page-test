using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Management;

public class OrderMapper : BaseMapper<BLL.DTO.Order, Public.DTO.v1.Management.Order>
{
    public OrderMapper(IMapper mapper) : base(mapper)
    {
    }
}