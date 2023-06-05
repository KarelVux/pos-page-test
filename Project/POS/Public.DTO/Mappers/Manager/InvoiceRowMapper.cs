using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Manager;

public class InvoiceRowMapper: BaseMapper<BLL.DTO.InvoiceRow, Public.DTO.v1.Manager.InvoiceRow>
{
    public InvoiceRowMapper(IMapper mapper) : base(mapper)
    {
    }
}