using DAL.Contracts.Base;
using DAL.DTO;
using Helpers;

namespace DAL.Contracts.App;

public interface IInvoiceRepository : IBaseRepository<Invoice>, IInvoiceRepositoryCustom<Invoice>, IBaseRepositoryWithInclude<Invoice>
{
    // add here custom methods for repo only
    Task<Invoice?> FindAsyncWithIdentity(Guid id, Guid userId);
    
    Task<IEnumerable<Invoice?>> GetUserInvoicesWhereStatusIsBiggerWithIncludes(Guid userId, InvoiceAcceptanceStatus acceptanceAcceptanceStatus);


    Task<IEnumerable<Invoice?>> GetAllBusinessInvoicesWhereStatusIsBiggerWithInclude(Guid businessId, InvoiceAcceptanceStatus acceptanceAcceptanceStatus);
    Task<Invoice?> GetInvoiceWithRowsAndProducts(Guid invoiceId, Guid businessId);
    Task<Invoice?> GetInvoiceViaOrderId(Guid orderId);
}

public interface IInvoiceRepositoryCustom<TEntity>
{
    // add here shared methods between repo and service
}