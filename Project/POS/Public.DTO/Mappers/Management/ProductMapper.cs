using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Management;

public class ProductMapper : BaseMapper<BLL.DTO.Product, Public.DTO.v1.Management.Product>
{
    public ProductMapper(IMapper mapper) : base(mapper)
    {
    }
}