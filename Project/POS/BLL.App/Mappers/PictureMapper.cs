using AutoMapper;
using DAL.Base;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers;

public class PictureMapper : BaseMapper<BllDto.Picture, DalDto.Picture>
{
    public PictureMapper(IMapper mapper) : base(mapper)
    {
    }
}