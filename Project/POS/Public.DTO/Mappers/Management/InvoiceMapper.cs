using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Management;

public class InvoiceMapper : BaseMapper<BLL.DTO.Invoice, Public.DTO.v1.Management.Invoice>
{
    public InvoiceMapper(IMapper mapper) : base(mapper)
    {
    }
}