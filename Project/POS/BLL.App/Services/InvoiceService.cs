using BLL.App.Mappers;
using BLL.Base;
using BLL.Contracts.App;
using BLL.DTO.Shop;
using Contracts.Base;
using DAL.Contracts.App;
using Helpers;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Services;

public class InvoiceService :
    BaseEntityService<BllDto.Invoice, DalDto.Invoice, IInvoiceRepository>,
    IInvoiceService
{
    protected IAppUOW Uow;

    public InvoiceService(IAppUOW uow, IMapper<BllDto.Invoice, DalDto.Invoice> mapper)
        : base(uow.InvoiceRepository, mapper)
    {
        Uow = uow;
    }

    /// <summary>
    /// Gets all invoices with includes
    /// </summary>
    /// <returns>Returns all invoices</returns>
    public async Task<IEnumerable<BllDto.Invoice>> AllAsyncWithInclude()
    {
        return (await Repository.AllAsyncWithInclude()).Select(e => Mapper.Map(e))!;
    }

    /// <summary>
    /// Returns invoice with includes
    /// </summary>
    /// <param name="id">Invoice ID</param>
    /// <returns>Searchable invoice</returns>
    public async Task<BllDto.Invoice?> FindAsyncWithInclude(Guid id)
    {
        return Mapper.Map(await Repository.FindAsyncWithInclude(id));
    }

    /// <summary>
    /// Finds the invoice that belongs to the specified user
    /// </summary>
    /// <param name="id">Invoice ID</param>
    /// <param name="userId">User id</param>
    /// <returns>User invoices</returns>
    public async Task<BllDto.Invoice?> FindAsyncWithIdentity(Guid id, Guid userId)
    {
        return Mapper.Map(await Repository.FindAsyncWithIdentity(id, userId));
    }


    /// <summary>
    /// Used ot calculate and create the invoice
    /// </summary>
    /// <param name="getUserId"></param>
    /// <param name="elementBusinessId"></param>
    /// <param name="invoiceItemCreationCounts"></param>
    /// <returns></returns>
    public async Task<BllDto.Invoice?> CalculateAndCreateInvoice(Guid getUserId, Guid elementBusinessId,
        List<InvoiceCreateEditProduct> invoiceItemCreationCounts)
    {
        var invoice = new BLL.DTO.Invoice
        {
            FinalTotalPrice = 0,
            TaxAmount = 0,
            TotalPriceWithoutTax = 0,
            PaymentCompleted = false,
            AppUserId = getUserId,
            BusinessId = elementBusinessId,
            CreationTime = DateTime.Now,
            InvoiceRows = new List<BllDto.InvoiceRow>()
        };

        // Create invoice row
        foreach (var item in invoiceItemCreationCounts)
        {
            var product =
                await Uow.ProductRepository.GetBusinessProduct(item.ProductId, elementBusinessId, false);

            if (product == null || product.UnitCount < item.ProductUnitCount)
            {
                return null;
            }
            else
            {
                product.UnitCount -= item.ProductUnitCount;
            }


            var pricePerUnit = product.UnitPrice + product.UnitDiscount;
            var finalPricePerUnit =
                pricePerUnit * item.ProductUnitCount;

            var invoiceRow = new BllDto.InvoiceRow
            {
                FinalProductPrice = finalPricePerUnit,
                ProductUnitCount = item.ProductUnitCount,
                ProductPricePerUnit = pricePerUnit,
                TaxPercent = product.TaxPercent,
                Currency = product.Currency,
                ProductId = product.Id,
            };

            invoiceRow.TaxAmountFromPercent =
                Math.Round(((invoiceRow.TaxPercent / 100) * invoiceRow.FinalProductPrice), 2);

            await Uow.ProductRepository.UpdateAsync(product);
            invoice.InvoiceRows.Add(invoiceRow);
        }

        invoice.FinalTotalPrice = invoice.InvoiceRows.Sum(x => x.FinalProductPrice);
        invoice.TaxAmount = invoice.InvoiceRows.Sum(x => x.TaxAmountFromPercent);
        invoice.TotalPriceWithoutTax = invoice.FinalTotalPrice - invoice.TaxAmount;

        invoice.Order = new BllDto.Order
        {
            StartTime = DateTime.Now,
            GivenToClientTime = default,
            OrderAcceptanceStatus = OrderAcceptanceStatus.Unknown,
            CustomerComment = null,
        };

        var value = Add(invoice);


        return value;
    }


    /// <summary>
    /// Allow customer to set acceptance status (aka user wants to order or will reject it)
    /// </summary>
    /// <param name="invoiceId">User invoice ID</param>
    /// <param name="acceptance">User acceptance status</param>
    /// <param name="userId">User ID</param>
    public async Task SetUserAcceptanceValue(Guid invoiceId, InvoiceAcceptanceStatus acceptanceAcceptanceStatus,
        Guid userId)
    {
        var invoice = (await FindAsyncWithIdentity(invoiceId, userId));

        if (invoice == null)
        {
            throw new NullReferenceException();
        }

        // accept and update invoice
        invoice.InvoiceAcceptanceStatus = acceptanceAcceptanceStatus;
        await UpdateAsync(invoice);
    }

    /// <summary>
    /// Invoice with invoice rows will be deleted and products counts will be updated
    /// </summary>
    /// <param name="invoiceId">User invoice ID</param>
    /// <param name="userId">User ID</param>
    public async Task<BllDto.Invoice> RemoveInvoiceWithDependenciesAndRestoreProductCounts(Guid invoiceId, Guid userId)
    {
        var invoice = (await FindAsyncWithIdentity(invoiceId, userId));

        if (invoice == null)
        {
            throw new NullReferenceException();
        }

        // delete all values
        
        if (invoice.OrderId != Guid.Empty && invoice.OrderId != null)
        {
            await Uow.OrderRepository.RemoveAsync(invoice.OrderId.Value);
        }

        var invoiceRows =
            (await Uow.InvoiceRowRepository.GetInvoiceRows(invoice.Id)).Select(x =>
                new InvoiceRowCustomMapper().Map(x));

        foreach (var invoiceRow in invoiceRows)
        {
            var product =
                new ProductCustomMapper().Map(await Uow.ProductRepository.FindAsync(invoiceRow!.ProductId));

            product!.UnitCount += invoiceRow.ProductUnitCount;

            await Uow.ProductRepository.UpdateAsync(new ProductCustomMapper().Map(product)!);
            await Uow.InvoiceRowRepository.RemoveAsync(invoiceRow.Id);
        }

        await RemoveAsync(invoice.Id);
        return invoice;
    }


    /// <summary>
    /// Get all user accepted invoices with includes
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>All user repositories</returns>
    public async Task<IEnumerable<BLL.DTO.Invoice?>> GetUserInvoicesWhereStatusIsBiggerWithIncludes(Guid userId)
    {
        var res = await Repository
            .GetUserInvoicesWhereStatusIsBiggerWithIncludes(userId, InvoiceAcceptanceStatus.UserAccepted);
        return res.Select(x => Mapper.Map(x));
    }

    public async Task<IEnumerable<BllDto.Invoice?>> GetAllUserAcceptedBusinessInvoicesWithInclude(Guid businessId)
    {
        var res = await Repository.GetAllBusinessInvoicesWhereStatusIsBiggerWithInclude(businessId,
            InvoiceAcceptanceStatus.UserAccepted);
        return res.Select(x => Mapper.Map(x));
    }

    public async Task<BllDto.Invoice?> GetInvoiceWithRowsAndProducts(Guid invoiceId, Guid businessId)
    {
        var res = await Repository.GetInvoiceWithRowsAndProducts(invoiceId, businessId);
        return Mapper.Map(res);
    }

    /// <summary>
    /// Used to restore business product sizes but it does not modify invoice/invoiceRow data
    /// </summary>
    /// <param name="invoiceId">Invoice Id that will be modified</param>
    /// <returns>Task</returns>
    public async Task RestoreProductSizesFromInvoices(Guid invoiceId)
    {
        var invoiceRows =
            (await Uow.InvoiceRowRepository.GetInvoiceRows(invoiceId)).Select(x =>
                new InvoiceRowCustomMapper().Map(x));

        foreach (var invoiceRow in invoiceRows)
        {
            var product =
                new ProductCustomMapper().Map(await Uow.ProductRepository.FindAsync(invoiceRow!.ProductId));

            product!.UnitCount += invoiceRow.ProductUnitCount;
            await Uow.ProductRepository.UpdateAsync(new ProductCustomMapper().Map(product)!);
        }
    }

    /// <summary>
    /// Finds invoice via order ID
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public async Task<BllDto.Invoice?> GetInvoiceViaOrderId(Guid orderId)
    {
        var res = await Repository.GetInvoiceViaOrderId(orderId);
        return Mapper.Map(res);

    }
}