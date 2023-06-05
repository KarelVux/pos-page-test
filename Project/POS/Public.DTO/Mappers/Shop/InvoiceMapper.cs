using DAL.Base;
using AutoMapper;
using BLL.DTO;

namespace Public.DTO.Mappers.Shop;

public class InvoiceMapper : BaseMapper<BLL.DTO.Invoice, Public.DTO.v1.Shop.Invoice>
{
    public InvoiceMapper(IMapper mapper) : base(mapper)
    {
    }


    public Public.DTO.v1.Shop.Invoice MapBusinessName(Invoice invoice, string businessName)
    {
        var res = Mapper.Map<Public.DTO.v1.Shop.Invoice>(invoice);
        res.BusinessName = businessName; 
        return res;
    }
}