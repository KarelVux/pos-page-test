using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Manager;

public class InvoiceMapper : BaseMapper<BLL.DTO.Invoice, Public.DTO.v1.Manager.Invoice>
{
    public InvoiceMapper(IMapper mapper) : base(mapper)
    {
    }

    public Public.DTO.v1.Manager.Invoice MapWithProductNameAndUserName(BLL.DTO.Invoice invoice)
    {
        var res = Mapper.Map<Public.DTO.v1.Manager.Invoice>(invoice);

        if (invoice.AppUser != null)
        {
            res.UserName = invoice.AppUser.NormalizedUserName!;
        }
        
        if (res.InvoiceRows != null)
        {
            foreach (var item in res.InvoiceRows)
            {
                if (invoice.InvoiceRows != null)
                    item.ProductName = invoice.InvoiceRows
                        .First(x => x.Id.Equals(item.Id))
                        .Product!.Name;
            }
        }

        return res;
    }
}