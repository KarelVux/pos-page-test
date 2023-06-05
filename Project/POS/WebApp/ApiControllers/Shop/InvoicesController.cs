using System.Net.Mime;
using Asp.Versioning;
using AutoMapper;
using BLL.Contracts.App;
using Helpers;
using Helpers.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.v1;
using PublicDtoV1Shop = Public.DTO.v1.Shop;
using BllDto = BLL.DTO;

namespace WebApp.ApiControllers.Shop
{
    /// <summary>
    /// Public API for shop
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/public/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InvoicesController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the mapper
        /// </summary>
        private readonly Public.DTO.Mappers.Shop.InvoiceMapper _invoiceMapper;

        private readonly Public.DTO.Mappers.Shop.InvoiceRow _invoiceRowMapper;
        private readonly Public.DTO.Mappers.Shop.OrderMapper _orderMapper;

        /// <summary>
        /// Reference to the BLL service
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// Construct
        /// </summary>
        /// <param name="bll">BLL</param>
        /// <param name="autoMapper">Mapper</param>
        public InvoicesController(IMapper autoMapper, IAppBLL bll)
        {
            _bll = bll;
            _invoiceMapper = new Public.DTO.Mappers.Shop.InvoiceMapper(autoMapper);
            _invoiceRowMapper = new Public.DTO.Mappers.Shop.InvoiceRow(autoMapper);
            _orderMapper = new Public.DTO.Mappers.Shop.OrderMapper(autoMapper);
        }

        /// <summary>
        /// Creates a new invoice in DB
        /// </summary>
        /// <param name="element">List of businesses products to create a new invoice</param>
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateInvoice([FromBody] PublicDtoV1Shop.CreateEditInvoice element)
        {
            if (!(await BusinessExists(element.BusinessId)))
            {
                return NotFound($"Business [{element.BusinessId.ToString()}] was not found");
            }

            var invoiceItemCreationCounts = new List<BllDto.Shop.InvoiceCreateEditProduct>();

            foreach (var item in element.InvoiceCreateEditProducts)
            {
                var product =
                    await _bll.ProductService.GetBusinessProduct(item.ProductId, element.BusinessId, false);
                if (product == null)
                {
                    return NotFound($"Product [{item.ProductId}] was not found");
                }

                if (product.UnitCount < item.ProductUnitCount)
                {
                    return BadRequest(
                        $"Product [{item.ProductId}] has [{product.UnitCount}] units, but only [{item.ProductUnitCount}] units are available");
                }
                else
                {
                    invoiceItemCreationCounts.Add(new BllDto.Shop.InvoiceCreateEditProduct
                    {
                        ProductId = item.ProductId,
                        ProductUnitCount = item.ProductUnitCount
                    });
                }
            }


            var calculatedInvoice =
                await _bll.InvoiceService.CalculateAndCreateInvoice(User.GetUserId(), element.BusinessId,
                    invoiceItemCreationCounts);


            if (calculatedInvoice == null)
            {
                return BadRequest("There was a problem with received data");
            }


            element.Id = calculatedInvoice.Id;

            await _bll.SaveChangesAsync();
            return CreatedAtAction("GetUserInvoice", new { id = element.Id }, element);
        }


        /// <summary>
        /// Used to get users invoice details 
        /// </summary>
        /// <param name="id">Invoice id</param>
        /// <returns>User invoice details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PublicDtoV1Shop.Invoice>> GetUserInvoice(Guid id)
        {
            var invoice = await _bll.InvoiceService.FindAsyncWithIdentity(id, User.GetUserId());
            if (invoice == null)
            {
                return NotFound($"Invoice [{id.ToString()}] was not found");
            }

            var invoiceRows = (await _bll.InvoiceRowService.GetInvoiceRows(invoice.Id)).ToList();

            if (invoiceRows.Count == 0)
            {
                return NotFound($"Invoice [{id.ToString()}] has no data");
            }

            var returnable = _invoiceMapper.Map(invoice);
            returnable!.InvoiceRows = new List<PublicDtoV1Shop.InvoiceRow>();
            foreach (var invoiceRow in invoiceRows)
            {
                var product = await _bll.ProductService.FindAsync(invoiceRow.ProductId);

                if (product == null)
                {
                    return NotFound($"Product [{invoiceRow.ProductId}] was not found");
                }

                returnable.InvoiceRows.Add(_invoiceRowMapper.MapWithProductName(invoiceRow, product.Name)!);
            }


            var order = await _bll.OrderService.FindAsync(returnable.OrderId);

            if (order == null)
            {
                return NotFound($"Order for invoice [{invoice.Id}] Not Found");
            }

            returnable.Order = _orderMapper.Map(order)!;

            return returnable;
        }

        /// <summary>
        /// Used to get all user invoices
        /// </summary>
        /// <returns>User invoice details</returns>
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<PublicDtoV1Shop.Invoice>>> GetAllUserInvoices()
        {
            var invoices = (await _bll.InvoiceService
                .GetUserInvoicesWhereStatusIsBiggerWithIncludes(User.GetUserId())).ToList();
            var returnable = new List<PublicDtoV1Shop.Invoice>();
            if (invoices.Count > 0)
            {
                foreach (var invoice in invoices)
                {
                    var businessName = await _bll.BusinessService.FindAsync(invoice!.BusinessId);
                    returnable.Add(_invoiceMapper.MapBusinessName(invoice, businessName!.Name));
                }
                return returnable;
            }

            return invoices.Select(x =>
                _invoiceMapper.Map(x)
            ).ToList()!;
        }

        /// <summary>
        /// User is able to accept the invoice and send it to the business
        /// If acceptance true then customer will make a order
        /// </summary>
        /// <param name="id">>Invoice ID</param>
        /// <param name="acceptance">User opinion if he accepts the invoice</param>
        /// <returns>Create edit invoice</returns>
        [HttpPatch("{id}/acceptance")]
        public async Task<ActionResult<PublicDtoV1Shop.CreateEditInvoice>> Acceptance(Guid id,
            [FromBody] PublicDtoV1Shop.AcceptInvoice acceptance)
        {
            var invoice = await _bll.InvoiceService.FindAsyncWithIdentity(id, User.GetUserId());
            if (invoice == null)
            {
                return NotFound($"Invoice [{id.ToString()}] belonging to the user  was not found");
            }

            if (acceptance.Acceptance)
            {
                await _bll.InvoiceService.SetUserAcceptanceValue(id, InvoiceAcceptanceStatus.UserAccepted,
                    User.GetUserId());

                await _bll.SaveChangesAsync();
                return NoContent();
            }

            return NoContent();
        }


        /// <summary>
        /// Deletes Invoice with all dependencies using API if allowed
        /// </summary>
        /// <param name="id">ID to be deleted from DB</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteInvoiceIfAllowed(Guid id)
        {
            var element = await _bll.InvoiceService.FindAsyncWithIdentity(id, User.GetUserId());

            if (element == null)
            {
                return NotFound();
            }

            if (
                element.InvoiceAcceptanceStatus == InvoiceAcceptanceStatus.UserAccepted ||
                element.InvoiceAcceptanceStatus == InvoiceAcceptanceStatus.Unknown
            )
            {
                await _bll.InvoiceService.RemoveInvoiceWithDependenciesAndRestoreProductCounts(id, User.GetUserId());
            }
            else
            {
                return BadRequest("User in not allowed to delete invoice because it is already processed");
            }


            await _bll.SaveChangesAsync();
            return NoContent();
        }


        /// <summary>
        /// checks if business exists
        /// </summary>
        /// <param name="id">Business ID</param>
        /// <returns>Existence status</returns>
        private async Task<bool> BusinessExists(Guid id)
        {
            return (await _bll.BusinessService.FindAsync(id)) != null;
        }
    }
}