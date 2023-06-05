using Contracts.Base;
using DAL.Contracts.App;
using DAL.EF.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.App.Repositories;

public class InvoiceRowRepository : EFBaseRepository<DAL.DTO.InvoiceRow, Domain.App.InvoiceRow, ApplicationDbContext>, IInvoiceRowRepository
{
    public InvoiceRowRepository(ApplicationDbContext dataContext,  IMapper<DAL.DTO.InvoiceRow, Domain.App.InvoiceRow> mapper) : base(dataContext , mapper)
    {
    }

    public async Task<IEnumerable<DAL.DTO.InvoiceRow>> AllAsyncWithInclude()
    {
        var res = await RepositoryDbSet
            .Include(i => i.Invoice)
            .Include(i => i.Product)
            .ToListAsync();

        return res.Select(x => Mapper.Map(x)!);
    }

    public async Task<DAL.DTO.InvoiceRow?> FindAsyncWithInclude(Guid id)
    {
        var res = await RepositoryDbSet
            .Include(i => i.Invoice)
            .Include(i => i.Product)
            .FirstOrDefaultAsync(m => m.Id == id);

        return Mapper.Map(res);
    }

    /// <summary>
    /// Get invoice invoiceRows
    /// </summary>
    /// <param name="invoiceId">Invoice ID</param>
    /// <returns>InvoiceRows</returns>
    public async Task<IEnumerable<DAL.DTO.InvoiceRow>> GetInvoiceRows(Guid invoiceId)
    {
        var res =  await RepositoryDbSet
            .Where(i => i.InvoiceId == invoiceId)
            .ToListAsync();

        return res.Select(x => Mapper.Map(x)!);
    }
}