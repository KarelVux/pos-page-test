using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Manager;

public class PictureMapper: BaseMapper<BLL.DTO.Picture, Public.DTO.v1.Manager.Picture>
{
    public PictureMapper(IMapper mapper) : base(mapper)
    {
    }
}