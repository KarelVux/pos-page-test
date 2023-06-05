using AutoMapper;
using DAL.Base;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers;

public class BusinessCategoryMapper : BaseMapper<BllDto.BusinessCategory, DalDto.BusinessCategory>
{
    public BusinessCategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}