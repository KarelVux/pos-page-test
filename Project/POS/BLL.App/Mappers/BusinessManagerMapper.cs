using AutoMapper;
using DAL.Base;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers;

public class BusinessManagerMapper : BaseMapper<BllDto.BusinessManager, DalDto.BusinessManager>
{
    public BusinessManagerMapper(IMapper mapper) : base(mapper)
    {
    }
}