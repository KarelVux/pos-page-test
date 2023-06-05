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
    /// ONLY FOR ROOT USER Management CRUD API controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/management/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]    [Authorize(Roles = "Root")]

    public class OrdersController : ControllerBase
    {
        /// <summary>
        /// Reference to the BLL service
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// DTO mapper
        /// </summary>
        private readonly Public.DTO.Mappers.Management.OrderMapper _mapper;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="bll">References to BLL</param>
        /// <param name="autoMapper">Mapper config</param>
        public OrdersController(IMapper autoMapper, IAppBLL bll)
        {
            _bll = bll;
            _mapper = new Public.DTO.Mappers.Management.OrderMapper(autoMapper);
        }


        // GET: api/Orders
        /// <summary>
        /// Get all controller API records from DB
        /// </summary>
        /// <returns>All API records form db</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Management.Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicDtoV1Management.Order>>> GetOrders()
        {
            var elements = await _bll.OrderService.AllAsync();
            return elements.Select(x => _mapper.Map(x)).ToList()!;
        }

        // GET: api/Orders/5
        /// <summary>
        /// Get API data for single record
        /// </summary>
        /// <param name="id">ID to be loaded from DB</param>
        /// <returns>Data for single record</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Management.Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDtoV1Management.Order>> GetOrder(Guid id)
        {
            var element = await _bll.OrderService.FindAsync(id);

            if (element == null)
            {
                return NotFound();
            }

            return _mapper.Map(element)!;
        }

        // PUT: api/Orders/5
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
        public async Task<IActionResult> PutOrder(Guid id, PublicDtoV1Management.Order element)
        {
            if (id != element.Id)
            {
                return BadRequest();
            }

            try
            {
                _bll.OrderService.Update(_mapper.Map(element)!);
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await OrderExists(id)))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create new record to DB using API requests using API
        /// </summary>
        /// <param name="element">Record to be created</param>
        /// <returns>Created record</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(PublicDtoV1Management.Order), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDtoV1Management.Order>> PostOrder(PublicDtoV1Management.Order element)
        {
            _bll.OrderService.Add(_mapper.Map(element)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = element.Id }, element);
        }

        // DELETE: api/Orders/5
        /// <summary>
        /// Deletes DB record using API
        /// </summary>
        /// <param name="id">ID to be deleted from DB</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var element = await _bll.OrderService.FindAsync(id);

            if (element == null)
            {
                return NotFound();
            }
            else
            {
                await _bll.OrderService.RemoveAsync(id);
            }

            await _bll.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Check id entity exists in DB
        /// </summary>
        /// <param name="id">Id to be checked</param>
        /// <returns>Boolean value</returns>
        private async Task<bool> OrderExists(Guid id)
        {
            return (await _bll.OrderService.FindAsync(id)) != null;
        }
    }
}