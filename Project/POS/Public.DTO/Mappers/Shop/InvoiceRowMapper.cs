using DAL.Base;
using AutoMapper;

namespace Public.DTO.Mappers.Shop;

public class InvoiceRow : BaseMapper<BLL.DTO.InvoiceRow, Public.DTO.v1.Shop.InvoiceRow>
{
    public InvoiceRow(IMapper mapper) : base(mapper)
    {
    }

    public Public.DTO.v1.Shop.InvoiceRow MapWithProductName(BLL.DTO.InvoiceRow invoiceRow, string productName)
    {
        var res = Mapper.Map<Public.DTO.v1.Shop.InvoiceRow>(invoiceRow);
        res.ProductName = productName; 
        return res;
    }
}