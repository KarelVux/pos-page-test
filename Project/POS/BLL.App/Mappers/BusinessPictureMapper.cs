using AutoMapper;
using DAL.Base;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers;

public class BusinessPictureMapper : BaseMapper<BllDto.BusinessPicture, DalDto.BusinessPicture>
{
    public BusinessPictureMapper(IMapper mapper) : base(mapper)
    {
    }
}