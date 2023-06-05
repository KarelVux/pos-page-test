using AutoMapper;
using DAL.Base;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers;

public class InvoiceRowMapper : BaseMapper<BllDto.InvoiceRow, DalDto.InvoiceRow>
{
    public InvoiceRowMapper(IMapper mapper) : base(mapper)
    {
    }
}

public class InvoiceRowCustomMapper : BaseCustomMapper<BllDto.InvoiceRow, DalDto.InvoiceRow>
{
    public override BllDto.InvoiceRow? Map(DalDto.InvoiceRow? entity)
    {
        return entity != null
            ? new BllDto.InvoiceRow
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

    public override DalDto.InvoiceRow? Map(BllDto.InvoiceRow? entity)
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
}