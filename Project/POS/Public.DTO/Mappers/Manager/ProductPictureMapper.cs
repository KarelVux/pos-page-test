using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Manager;

public class ProductPictureMapper: BaseMapper<BLL.DTO.ProductPicture, Public.DTO.v1.Manager.ProductPicture>
{
    public ProductPictureMapper(IMapper mapper) : base(mapper)
    {
    }
}