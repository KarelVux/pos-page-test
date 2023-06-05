using AutoMapper;
using DAL.Base;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers;

public class ProductPictureMapper : BaseMapper<BllDto.ProductPicture, DalDto.ProductPicture>
{
    public ProductPictureMapper(IMapper mapper) : base(mapper)
    {
    }
}