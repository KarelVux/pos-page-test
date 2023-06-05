using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Manager;

public class BusinessPictureMapper: BaseMapper<BLL.DTO.BusinessPicture, Public.DTO.v1.Manager.BusinessPicture>
{
    public BusinessPictureMapper(IMapper mapper) : base(mapper)
    {
    }
}