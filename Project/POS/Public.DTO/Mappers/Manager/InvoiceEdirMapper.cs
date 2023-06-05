using AutoMapper;
using BLL.DTO;
using DAL.Base;
using Helpers;
using Public.DTO.v1.Manager;
using Invoice = BLL.DTO.Invoice;

namespace Public.DTO.Mappers.Manager;

public class InvoiceEditMapper : BaseMapper<BLL.DTO.Invoice, Public.DTO.v1.Manager.InvoiceLimitedEdit>
{
    public InvoiceEditMapper(IMapper mapper) : base(mapper)
    {
    }
}