using AutoMapper;
using DAL.Base;
using DalDto = DAL.DTO;
using DomainApp = Domain.App;

namespace DAL.EF.App.Mappers;

public class ProductCategoryMapper : BaseMapper<DalDto.ProductCategory, DomainApp.ProductCategory>
{
    public ProductCategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}