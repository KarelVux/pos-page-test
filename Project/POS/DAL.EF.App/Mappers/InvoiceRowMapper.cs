using AutoMapper;
using DAL.Base;
using DalDto = DAL.DTO;
using DomainApp = Domain.App;

namespace DAL.EF.App.Mappers;

public class InvoiceRowMapper : BaseMapper<DalDto.InvoiceRow, DomainApp.InvoiceRow>
{
    public InvoiceRowMapper(IMapper mapper) : base(mapper)
    {
    }
}

public class InvoiceRowCustomMapper : BaseCustomMapper<DalDto.InvoiceRow, DomainApp.InvoiceRow>
{
    public override DalDto.InvoiceRow? Map(DomainApp.InvoiceRow? entity)
    {
        return entity != null
            ? new DalDto.InvoiceRow
            {
                Id = entity.Id,
                FinalProductPrice = entity.FinalProductPrice,
                ProductUnitCount =entity.ProductUnitCount,
                ProductPricePerUnit = entity.ProductPricePerUnit,
                TaxPercent =entity.TaxPercent,
                TaxAmountFromPercent = entity.TaxAmountFromPercent,
                Currency = entity.Currency,
                Comment = entity.Comment,
                ProductId = entity.ProductId,
                InvoiceId = entity.InvoiceId,
            }
            : null;
    }

    public override DomainApp.InvoiceRow? Map(DalDto.InvoiceRow? entity)
    {
        return entity != null
            ? new DomainApp.InvoiceRow
            {
                Id = entity.Id,
                FinalProductPrice = entity.FinalProductPrice,
                ProductUnitCount =entity.ProductUnitCount,
                ProductPricePerUnit = entity.ProductPricePerUnit,
                TaxPercent =entity.TaxPercent,
                TaxAmountFromPercent = entity.TaxAmountFromPercent,
                Currency = entity.Currency,
                Comment = entity.Comment,
                ProductId = entity.ProductId,
                InvoiceId = entity.InvoiceId,
            }
            : null;
    }
}