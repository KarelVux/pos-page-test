using AutoMapper;
using DAL.Base;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers;

public class ProductCategoryMapper : BaseMapper<BllDto.ProductCategory, DalDto.ProductCategory>
{
    public ProductCategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}