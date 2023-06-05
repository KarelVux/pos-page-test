using DAL.Contracts.App;
using DAL.Contracts.Base;
using BllDto = BLL.DTO;

namespace BLL.Contracts.App;

public interface IInvoiceRowService : IBaseRepository<BllDto.InvoiceRow>,
    IInvoiceRowRepositoryCustom<BllDto.InvoiceRow>
{
    // add your custom service methods here
    Task<IEnumerable<BllDto.InvoiceRow>> GetInvoiceRows(Guid invoiceId);
  //  Task<IEnumerable<BllDto.InvoiceRow>> GetInvoiceRowsWithProduct(Guid invoiceId);
    
    

}