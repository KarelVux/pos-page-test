using BLL.DTO.Shop;
using DAL.Contracts.App;
using DAL.Contracts.Base;
using Helpers;
using BllDto = BLL.DTO;

namespace BLL.Contracts.App;

public interface IInvoiceService : IBaseRepository<BllDto.Invoice>,
    IInvoiceRepositoryCustom<BllDto.Invoice>,
   IBaseRepositoryWithInclude<BllDto.Invoice>
{
    // add your custom service methods here
    Task<BllDto.Invoice?> FindAsyncWithIdentity(Guid id, Guid userId);

    Task<BllDto.Invoice?> CalculateAndCreateInvoice(Guid getUserId, Guid elementBusinessId, List<InvoiceCreateEditProduct> invoiceItemCreationCounts);
    Task SetUserAcceptanceValue(Guid invoiceId, InvoiceAcceptanceStatus acceptanceAcceptanceStatus, Guid userId);
    Task<BllDto.Invoice> RemoveInvoiceWithDependenciesAndRestoreProductCounts(Guid invoiceId,  Guid userId);
    Task<IEnumerable<BllDto.Invoice?>> GetUserInvoicesWhereStatusIsBiggerWithIncludes(Guid userId);
    Task<IEnumerable<BllDto.Invoice?>> GetAllUserAcceptedBusinessInvoicesWithInclude(Guid businessId);
    Task<BllDto.Invoice?> GetInvoiceWithRowsAndProducts(Guid invoiceId, Guid businessId);

    

    Task RestoreProductSizesFromInvoices(Guid invoiceId);
    Task<BllDto.Invoice?> GetInvoiceViaOrderId(Guid orderId);
}