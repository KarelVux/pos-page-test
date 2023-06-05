using DAL.Base;
using AutoMapper;

namespace Public.DTO.Mappers.Shop;

public class BusinessMapper : BaseMapper<BLL.DTO.Business, Public.DTO.v1.Shop.Business>
{
    public BusinessMapper(IMapper mapper) : base(mapper)
    {
    }
}