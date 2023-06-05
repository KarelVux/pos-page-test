using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Management;

public class PictureMapper: BaseMapper<BLL.DTO.Picture, Public.DTO.v1.Management.Picture>
{
    public PictureMapper(IMapper mapper) : base(mapper)
    {
    }
}