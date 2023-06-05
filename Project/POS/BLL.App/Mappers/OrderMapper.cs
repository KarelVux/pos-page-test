using AutoMapper;
using DAL.Base;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers;

public class OrderMapper : BaseMapper<BllDto.Order, DalDto.Order>
{
    public OrderMapper(IMapper mapper) : base(mapper)
    {
    }
}