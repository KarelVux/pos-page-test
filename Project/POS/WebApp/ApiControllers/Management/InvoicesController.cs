using Asp.Versioning;
using AutoMapper;
using BLL.Contracts.App;
using DAL.Contracts.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublicDtoV1Management = Public.DTO.v1.Management;

namespace WebApp.ApiControllers.Management
{
    /// <summary>
    ///  Management CRUD API controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/management/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]    [Authorize(Roles = "Root")]

    public class InvoicesController : ControllerBase
    {
        // private readonly IAppUOW _uow;

        /// <summary>
        /// Reference to the BLL service
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// DTO mapper
        /// </summary>
        private readonly Public.DTO.Mappers.Management.InvoiceMapper _mapper;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="bll">References to BLL</param>
        /// <param name="autoMapper">Mapper config</param>
        public InvoicesController(IMapper autoMapper, IAppBLL bll)
        {
            _bll = bll;
            _mapper = new Public.DTO.Mappers.Management.InvoiceMapper(autoMapper);
        }

        // GET: api/Invoices
        /// <summary>
        /// Get all controller API records from DB
        /// </summary>
        /// <returns>All API records form db</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Management.Invoice>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicDtoV1Management.Invoice>>> GetInvoices()
        {
            var elements = await _bll.InvoiceService.AllAsync();
            return elements.Select(x => _mapper.Map(x)).ToList()!;
        }

        // GET: api/Invoices/5
        /// <summary>
        /// Get API data for single record
        /// </summary>
        /// <param name="id">ID to be loaded from DB</param>
        /// <returns>Data for single record</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Management.Invoice>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDtoV1Management.Invoice>> GetInvoice(Guid id)
        {
            var element = await _bll.InvoiceService.FindAsync(id);

            if (element == null)
            {
                return NotFound();
            }

            return  _mapper.Map(element)!;
        }

        // PUT: api/Invoices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Allows to edit DB data using API
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
        public async Task<IActionResult> PutInvoice(Guid id, PublicDtoV1Management.Invoice element)
        {
            if (id != element.Id)
            {
                return BadRequest();
            }

            try
            {
                _bll.InvoiceService.Update(_mapper.Map(element)!);

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
        [ProducesResponseType(typeof(PublicDtoV1Management.Invoice), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDtoV1Management.Invoice>> PostInvoice(
            PublicDtoV1Management.Invoice element)
        {
            _bll.InvoiceService.Add(_mapper.Map(element)!);
            await _bll.SaveChangesAsync();


            return CreatedAtAction("GetInvoice", new { id = element.Id }, element);
        }

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

        /// <summary>
        /// Check id entity exists in DB
        /// </summary>
        /// <param name="id">Id to be checked</param>
        /// <returns>Boolean value</returns>
        private async Task<bool> InvoiceExists(Guid id)
        {
            return (await _bll.InvoiceService.FindAsync(id)) != null;
        }
    }
}