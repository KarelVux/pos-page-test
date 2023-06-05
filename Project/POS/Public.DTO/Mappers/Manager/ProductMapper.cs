using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Manager;

public class ProductMapper: BaseMapper<BLL.DTO.Product, Public.DTO.v1.Manager.Product>
{
    public ProductMapper(IMapper mapper) : base(mapper)
    {
    }
}