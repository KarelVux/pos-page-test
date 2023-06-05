using AutoMapper;
using DAL.Base;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers;

public class BusinessMapper : BaseMapper<BllDto.Business, DalDto.Business>
{
    public BusinessMapper(IMapper mapper) : base(mapper)
    {
    }
}