using DAL.Base;
using AutoMapper;

namespace Public.DTO.Mappers.Shop;

public class ProductMapper : BaseMapper<BLL.DTO.Product, Public.DTO.v1.Shop.Product>
{
    public ProductMapper(IMapper mapper) : base(mapper)
    {
    }
}