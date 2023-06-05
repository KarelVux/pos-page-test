using Contracts.Base;
using DAL.Contracts.App;
using DAL.DTO;
using DAL.EF.Base;
using Helpers;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.App.Repositories;

public class InvoiceRepository : EFBaseRepository<DAL.DTO.Invoice, Domain.App.Invoice, ApplicationDbContext>,
    IInvoiceRepository
{
    public InvoiceRepository(ApplicationDbContext dataContext, IMapper<DAL.DTO.Invoice, Domain.App.Invoice> mapper) :
        base(dataContext, mapper)
    {
    }


    /// <summary>
    /// Get a list of invoices with includes
    /// </summary>
    /// <returns>List of invoices</returns>
    public async Task<IEnumerable<DAL.DTO.Invoice>> AllAsyncWithInclude()
    {
        var res = await RepositoryDbSet
            .Include(i => i.AppUser)
            .Include(i => i.Business)
            .Include(x => x.Order)
            .ToListAsync();

        return res.Select(x => Mapper.Map(x)!);
    }

    /// <summary>
    /// Get a single invoice with includes
    /// </summary>
    /// <param name="id">Id of invoice that will be returned</param>
    /// <returns>A searchable invoice</returns>
    public async Task<DAL.DTO.Invoice?> FindAsyncWithInclude(Guid id)
    {
        var res = await RepositoryDbSet
            .Include(i => i.AppUser)
            .Include(i => i.Business)
            .Include(x => x.Order)
            .FirstOrDefaultAsync(m => m.Id == id);

        return Mapper.Map(res);
    }


    /// <summary>
    /// Get an invoice with id and user id
    /// </summary>
    /// <param name="id">Invoice ID</param>
    /// <param name="userId">Use ID</param>
    /// <returns></returns>
    public async Task<DAL.DTO.Invoice?> FindAsyncWithIdentity(Guid id, Guid userId)
    {
        var res = await RepositoryDbSet
            .Where(x => x.Id == id && x.AppUserId == userId)
            .FirstOrDefaultAsync();

        return Mapper.Map(res);
    }

    public async Task<IEnumerable<Invoice?>> GetUserInvoicesWhereStatusIsBiggerWithIncludes(Guid userId,
        InvoiceAcceptanceStatus acceptanceAcceptanceStatus)
    {
        var res = await RepositoryDbSet
            .Include(x => x.Order)
            .Where(x => x.AppUserId.Equals(userId) &&
                        (x.InvoiceAcceptanceStatus >= acceptanceAcceptanceStatus))
            .ToListAsync();

        return res.Select(x => Mapper.Map(x));
    }

    public async Task<IEnumerable<Invoice?>> GetAllBusinessInvoicesWhereStatusIsBiggerWithInclude(Guid businessId,
        InvoiceAcceptanceStatus statusValue)
    {
        var res = await RepositoryDbSet
            .Include(x => x.InvoiceRows)!
            .ThenInclude(y => y.Product)
            .Include(x => x.Order)
            .Include(x => x.AppUser)
            .Where(x =>
                x.BusinessId.Equals(businessId) &&
                (x.InvoiceAcceptanceStatus >= statusValue)
            )
            .ToListAsync();

        return res.Select(x => Mapper.Map(x));
    }

    /// <summary>
    /// Gets invoice (order included) with invoice rows and products
    /// </summary>
    /// <param name="invoiceId">Searchable invoice ID</param>
    /// <param name="businessId">Searchable business ID</param>
    /// <returns>Searched invocie</returns>
    public async Task<Invoice?> GetInvoiceWithRowsAndProducts(Guid invoiceId, Guid businessId)
    {
        var res = await RepositoryDbSet
            .Include(x => x.InvoiceRows)!
            .ThenInclude(y => y.Product)
            .Include(x => x.Order)
            .Include(x => x.AppUser)
            .FirstOrDefaultAsync(x => x.Id.Equals(invoiceId) && x.BusinessId.Equals(businessId));

        return Mapper.Map(res);
    }

    public async Task<Invoice?> GetInvoiceViaOrderId(Guid orderId)
    {
        var res = await RepositoryDbSet
            .FirstOrDefaultAsync(x => x.OrderId.Equals(orderId));

        return Mapper.Map(res);
    }
}