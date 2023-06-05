using Asp.Versioning;
using AutoMapper;
using BLL.Contracts.App;
using BLL.DTO;
using Domain.App.Identity;
using Helpers;
using Helpers.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using PublicDtoV1Manager = Public.DTO.v1.Manager;

namespace WebApp.ApiControllers.Manager
{
    /// <summary>
    /// Management CRUD API controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/manager/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "BusinessManager")]

    public class InvoicesController : ControllerBase
    {
        /// <summary>
        /// Reference to the BLL service
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// UserManager reference
        /// </summary>
        private readonly UserManager<AppUser> _userManager;

        /// <summary>
        /// DTO mapper
        /// </summary>
        private readonly Public.DTO.Mappers.Manager.InvoiceMapper _mapper;

        private readonly Public.DTO.Mappers.Manager.InvoiceEditMapper _mapperInvoiceEdit;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="bll">References to BLL</param>
        /// <param name="autoMapper">Mapper config</param>
        /// <param name="userManager">UserManager config</param>
        public InvoicesController(IMapper autoMapper, IAppBLL bll, UserManager<AppUser> userManager)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new Public.DTO.Mappers.Manager.InvoiceMapper(autoMapper);
            _mapperInvoiceEdit = new Public.DTO.Mappers.Manager.InvoiceEditMapper(autoMapper);
        }

        // GET: api/Invoices
        /// <summary>
        /// Get all controller API records from DB
        /// </summary>
        /// <returns>All API records form db</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Manager.Invoice>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PublicDtoV1Manager.Invoice>>> GetBusinessInvoices(
            [FromQuery] Guid businessId)
        {
            var result = await CheckIfUserIsBusinessManager(businessId);

            if (!result)
            {
                return BadRequest("User does not have access to the business");
            }

            var elements = await _bll.InvoiceService.GetAllUserAcceptedBusinessInvoicesWithInclude(businessId);
            return elements.Select(x => _mapper.MapWithProductNameAndUserName(x!)).ToList()!;
        }

        // GET: api/Invoices/5
        /// <summary>
        /// Get API data for single record
        /// </summary>
        /// <param name="id">ID to be loaded from DB</param>
        /// <returns>Data for single record</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Manager.Invoice>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDtoV1Manager.Invoice>> GetInvoice(Guid id)
        {
            var search = await _bll.InvoiceService.FindAsync(id);

            if (search == null)
            {
                return NotFound("Invoice was not found");
            }

            var result = await CheckIfUserIsBusinessManager(search.BusinessId);

            if (!result)
            {
                return BadRequest("User does not have access to the business");
            }

            var element = await _bll.InvoiceService.GetInvoiceWithRowsAndProducts(id, search.BusinessId);

            if (element == null)
            {
                return NotFound();
            }

            return _mapper.MapWithProductNameAndUserName(element);
        }

        /*
        // PUT: api/Invoices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Allows to edit DB data using API and if
        /// </summary>
        /// <param name="id">ID to be edited in DB</param>
        /// <param name="element">Edited object</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutInvoice(Guid id, PublicDtoV1Manager.InvoiceEdit element)
        {
            var result = await CheckIfUserIsSearchableBusinessManager(element.BusinessId);

            if (!result)
            {
                return BadRequest("User does not have access to the business");
            }

            if (id != element.Id)
            {
                return BadRequest();
            }

            try
            {
                if (element.InvoiceAcceptanceStatus == InvoiceAcceptanceStatus.BusinessRejected)
                {
                    await _bll.InvoiceService.RestoreProductSizesFromInvoices(element.Id);
                }
                else if (element.Order != null &&
                         element.Order.OrderAcceptanceStatus == OrderAcceptanceStatus.GivenToClient)
                {
                    element.Order.GivenToClientTime = DateTime.Now;
                    await _bll.InvoiceService.RestoreProductSizesFromInvoices(element.Id);
                }

                _bll.InvoiceService.Update(_mapperInvoiceEdit.Map(element)!);

                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await InvoiceExists(id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        */


        // POST: api/Invoices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create new record to DB using API requests using API
        /// </summary>
        /// <param name="element">Record to be created</param>
        /// <returns>Created record</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(PublicDtoV1Manager.Invoice), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDtoV1Manager.Invoice>> PostInvoice(
            PublicDtoV1Manager.Invoice element)
        {
            var result = await CheckIfUserIsBusinessManager(element.BusinessId);

            if (!result)
            {
                return BadRequest("User does not have access to the business");
            }

            _bll.InvoiceService.Add(_mapper.Map(element)!);
            await _bll.SaveChangesAsync();


            return CreatedAtAction("GetInvoice", new { id = element.Id }, element);
        }

        /// <summary>
        /// Edit limited allowed data in patch request and restore product data if invoice is cancelled
        /// </summary>
        /// <param name="id">>Invoice ID</param>
        /// <param name="element">Limited data element</param>
        /// <returns>Create edit invoice</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<PublicDtoV1Manager.InvoiceLimitedEdit>> ModifyLimitedInvoiceData(
            Guid id, [FromBody] PublicDtoV1Manager.InvoiceLimitedEdit element)
        {
            if (id != element.Id)
            {
                return BadRequest();
            }

            try
            {
                // check if records exist
                var invoiceBll = await _bll.InvoiceService.FindAsync(element.Id);
                if (invoiceBll == null)
                {
                    return NotFound("Invoice not found");
                }

                var result = await CheckIfUserIsBusinessManager(invoiceBll.BusinessId);
                if (!result)
                {
                    return BadRequest("User does not have access to the business");
                }


                var orderBll = await _bll.OrderService.FindAsync(invoiceBll.OrderId!.Value);
                if (orderBll == null)
                {
                    return NotFound("Order not found");
                }

                // Manipulate the public DTO
                if (element.InvoiceAcceptanceStatus == InvoiceAcceptanceStatus.BusinessRejected)
                {
                    await _bll.InvoiceService.RestoreProductSizesFromInvoices(element.Id);
                }
                else if (element.OrderAcceptanceStatus == OrderAcceptanceStatus.GivenToClient)
                {
                    orderBll.GivenToClientTime = DateTime.Now;
                    await _bll.InvoiceService.RestoreProductSizesFromInvoices(element.Id);
                }

                // Implement changes
                invoiceBll.InvoiceAcceptanceStatus = element.InvoiceAcceptanceStatus;
                invoiceBll.PaymentCompleted = element.PaymentCompleted;
                orderBll.OrderAcceptanceStatus = element.OrderAcceptanceStatus;

                await _bll.InvoiceService.UpdateAsync(invoiceBll);
                await _bll.OrderService.UpdateAsync(orderBll);

                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await InvoiceExists(id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        /*
        // DELETE: api/Invoices/5
        /// <summary>
        /// Deletes DB record using API
        /// </summary>
        /// <param name="id">ID to be deleted from DB</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteInvoice(Guid id)
        {
            var element = await _bll.InvoiceService.FindAsync(id);

            if (element == null)
            {
                return NotFound();
            }
            else
            {
                await _bll.InvoiceService.RemoveAsync(id);
            }

            await _bll.SaveChangesAsync();
            return NoContent();
        }
*/
        /// <summary>
        /// Check id entity exists in DB
        /// </summary>
        /// <param name="id">Id to be checked</param>
        /// <returns>Boolean value</returns>
        private async Task<bool> InvoiceExists(Guid id)
        {
            return (await _bll.InvoiceService.FindAsync(id)) != null;
        }

        /// <summary>
        /// Checks if business belongs to the user
        /// </summary>
        /// <param name="businessId">Business Id</param>
        /// <returns>Boolean status and if not fount then returns NotFound action</returns>
        private async Task<bool> CheckIfUserIsBusinessManager(Guid businessId)
        {
            var businessBelongsToUser =
                await _bll.BusinessManagerService.GetUserManagedBusiness(businessId, User.GetUserId());

            return businessBelongsToUser != null;
        }
    }
}