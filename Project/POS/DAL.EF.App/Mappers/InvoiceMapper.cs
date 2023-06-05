using AutoMapper;
using DAL.Base;
using DalDto = DAL.DTO;
using DomainApp = Domain.App;

namespace DAL.EF.App.Mappers;

public class InvoiceMapper : BaseMapper<DalDto.Invoice, DomainApp.Invoice>
{
    public InvoiceMapper(IMapper mapper) : base(mapper)
    {
    }
}