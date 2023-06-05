using BLL.Base;
using BLL.Contracts.App;
using Contracts.Base;
using DAL.Contracts.App;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Services;

public class InvoiceRowService :
    BaseEntityService<BllDto.InvoiceRow, DalDto.InvoiceRow, IInvoiceRowRepository>,
    IInvoiceRowService
{
    protected IAppUOW Uow;

    public InvoiceRowService(IAppUOW uow, IMapper<BllDto.InvoiceRow, DalDto.InvoiceRow> mapper)
        : base(uow.InvoiceRowRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<IEnumerable<BllDto.InvoiceRow>> AllAsyncWithInclude()
    {
        return (await Repository.AllAsyncWithInclude()).Select(e => Mapper.Map(e))!;
    }

    public async Task<BllDto.InvoiceRow?> FindAsyncWithInclude(Guid id)
    {
        return Mapper.Map(await Repository.FindAsyncWithInclude(id));
    }

    public async Task<IEnumerable<BllDto.InvoiceRow>> GetInvoiceRows(Guid invoiceId)
    {
        var result = await Repository.GetInvoiceRows(invoiceId);
        return result.Select(e => Mapper.Map(e)!);
    }

    /*
    public async Task<IEnumerable<BllDto.InvoiceRow>> GetInvoiceRowsWithProduct(Guid invoiceId)
    {
        var result = (await Repository.GetInvoiceRows(invoiceId)).ToList();

        foreach (var invoiceRow in result)
        {
            invoiceRow.Product = await Uow.ProductRepository.FindAsync(invoiceRow.ProductId);
        }

        return result.Select(e => Mapper.Map(e)!);
    }
    */
}