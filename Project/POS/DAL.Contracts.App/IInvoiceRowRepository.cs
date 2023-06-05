using DAL.Contracts.Base;
using DAL.DTO;
using Domain.Contracts.Base;

namespace DAL.Contracts.App;

public interface IInvoiceRowRepository : IBaseRepository<InvoiceRow>
    , IInvoiceRowRepositoryCustom<InvoiceRow>
{
    // add here custom methods for repo only
    Task<IEnumerable<InvoiceRow>> GetInvoiceRows(Guid invoiceId);
}

public interface IInvoiceRowRepositoryCustom<TEntity> : IBaseRepositoryWithInclude<TEntity>
    where TEntity : class, IDomainEntityId
{
    // add here shared methods between repo and service
   
}