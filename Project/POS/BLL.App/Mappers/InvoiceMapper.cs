using AutoMapper;
using DAL.Base;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers;

public class InvoiceMapper : BaseMapper<BllDto.Invoice, DalDto.Invoice>
{
    public InvoiceMapper(IMapper mapper) : base(mapper)
    {
    }
}