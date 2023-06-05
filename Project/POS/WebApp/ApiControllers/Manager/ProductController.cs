using Asp.Versioning;
using AutoMapper;
using BLL.Contracts.App;
using Helpers.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class ProductController : ControllerBase
    {
        /// <summary>
        /// Reference to the BLL service
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// DTO mapper
        /// </summary>
        private readonly Public.DTO.Mappers.Manager.ProductMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Bll</param>
        /// <param name="autoMapper">Mapper config  </param>
        public ProductController(IAppBLL bll, IMapper autoMapper)
        {
            _bll = bll;
            _mapper = new Public.DTO.Mappers.Manager.ProductMapper(autoMapper);
        }

        // GET: api/Product
        /// <summary>
        /// Get all controller API records from DB
        /// </summary>
        /// <returns>All API records form db</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Manager.Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PublicDtoV1Manager.Product>>> GetBusinessProducts(
            [FromQuery] Guid businessId)
        {
            var result = await CheckIfUserIsSearchableBusinessManager(businessId);

            if (!result)
            {
                return BadRequest("User does not have access to the business");
            }

            var elements = await _bll.ProductService.GetBusinessProductsWithIncludes(businessId);

            return elements.Select(x => _mapper.Map(x)).ToList()!;
        }

        // GET: api/Product/5
        /// <summary>
        /// Get API data for single record
        /// </summary>
        /// <param name="id">ID to be loaded from DB</param>
        /// <param name="businessId">Id for business to check ownership</param>
        /// <returns>Data for single record</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Manager.Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicDtoV1Manager.Product>> GetBusinessProduct(Guid id,
            [FromQuery] Guid businessId)
        {
            var result = await CheckIfUserIsSearchableBusinessManager(businessId);

            if (!result)
            {
                return BadRequest("User does not have access to the business");
            }

            var element = await _bll.ProductService.GetBusinessProduct(id, businessId);

            if (element == null)
            {
                return NotFound();
            }

            return _mapper.Map(element)!;
        }

        // PUT: api/Product/5
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
        public async Task<IActionResult> PutBusinessProduct(Guid id, PublicDtoV1Manager.Product element)
        {
            if (id != element.Id)
            {
                return BadRequest();
            }

            var result = await CheckIfUserIsSearchableBusinessManager(element.BusinessId);

            if (!result)
            {
                return BadRequest("User does not have access to the business");
            }

            try
            {
                _bll.ProductService.Update(_mapper.Map(element)!);
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await ProductExists(id)))
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

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create new record to DB using API requests using API
        /// </summary>
        /// <param name="element">Record to be created</param>
        /// <returns>Created record</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(PublicDtoV1Manager.Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicDtoV1Manager.Product>> PostProduct(
            PublicDtoV1Manager.Product element)
        {
            var result = await CheckIfUserIsSearchableBusinessManager(element.BusinessId);

            if (!result)
            {
                return BadRequest("User does not have access to the business");
            }

            var value = _bll.ProductService.Add(_mapper.Map(element)!);
            await _bll.SaveChangesAsync();

            element.Id = value.Id;

            return CreatedAtAction("GetBusinessProduct", new { id = element.Id }, element);
        }

        /*
        // DELETE: api/Product/5
        /// <summary>
        /// Deletes DB record using API
        /// </summary>
        /// <param name="id">ID to be deleted from DB</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var element = await _bll.ProductService.FindAsync(id);

            if (element == null)
            {
                return NotFound();
            }
            else
            {
                await _bll.ProductService.RemoveAsync(id);
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
        private async Task<bool> ProductExists(Guid id)
        {
            return (await _bll.ProductService.FindAsync(id)) != null;
        }

        /// <summary>
        /// Checks if business belongs to the user
        /// </summary>
        /// <param name="businessId">Business Id</param>
        /// <returns>Boolean status and if not fount then returns NotFound action</returns>
        private async Task<bool> CheckIfUserIsSearchableBusinessManager(Guid businessId)
        {
            var businessBelongsToUser =
                await _bll.BusinessManagerService.GetUserManagedBusiness(businessId, User.GetUserId());

            return businessBelongsToUser != null;
        }
    }
}