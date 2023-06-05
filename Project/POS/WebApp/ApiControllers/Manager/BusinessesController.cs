using Asp.Versioning;
using AutoMapper;
using BLL.Contracts.App;
using BLL.DTO;
using Helpers.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Public.DTO.Mappers.Manager;
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
    public class BusinessesController : ControllerBase
    {
        /// <summary>
        /// Reference to the BLL service
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// DTO mapper
        /// </summary>
        private readonly BusinessMapper _mapper;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="bll">References to BLL</param>
        /// <param name="automapper">Mapper config</param>
        public BusinessesController(IAppBLL bll, IMapper automapper)
        {
            _bll = bll;
            _mapper = new BusinessMapper(automapper);
        }

        // GET: api/Businesses
        /// <summary>
        /// Get all controller API records from DB
        /// </summary>
        /// <returns>All API records form db</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Manager.Business>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicDtoV1Manager.Business>>> GetUserBusinesses()
        {
            var elements = await _bll.BusinessService.AllAsyncWithBusinessOwnerAndPicture(User.GetUserId());
            return elements.Select(b => _mapper.Map(b)).ToList()!;
        }

        // GET: api/Businesses/5
        /// <summary>
        /// Get API data for single record if it user is this business manager 
        /// </summary>
        /// <param name="id">ID to be loaded from DB</param>
        /// <returns>Data for single record</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Manager.Business>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicDtoV1Manager.Business>> GetUserBusiness(Guid id)
        {
            var result = await CheckIfUserIsSearchableBusinessManager(id);

            if (!result)
            {
                return BadRequest("User does not have access to the business");
            }

            var element = await _bll.BusinessService.GetBusinessDetailsPageData(id);

            if (element == null)
            {
                return NotFound();
            }

            element.Products = (await _bll.ProductService.GetBusinessProductsWithIncludes(id))
                .ToList();

            
            
            return _mapper.Map(element)!;
        }


        // PUT: api/Businesses/5
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
        public async Task<IActionResult> PutBusiness(Guid id, PublicDtoV1Manager.Business element)
        {
            if (id != element.Id)
            {
                return BadRequest();
            }

            var result = await CheckIfUserIsSearchableBusinessManager(id);

            if (!result)
            {
                return BadRequest("User does not have access to the business");
            }
            
            try
            {
                await _bll.BusinessService.UpdateAsync(_mapper.Map(element)!
                );
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await BusinessExists(id)))
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

        // POST: api/Businesses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create new record to DB using API requests using API a
        /// </summary>
        /// <param name="element">Record to be created</param>
        /// <returns>Created record</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(PublicDtoV1Manager.Business), StatusCodes.Status201Created)]
        public async Task<ActionResult<PublicDtoV1Manager.Business>> PostUserBusiness(
            PublicDtoV1Manager.Business element)
        {
            var createdBusiness = _bll.BusinessService.Add(_mapper.Map(element)!);
            _bll.BusinessManagerService.Add(new BusinessManager
            {
                AppUserId = User.GetUserId(),
                BusinessId = createdBusiness.Id,
            });
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetBusiness", new { id = element.Id }, element);
        }

        /*
        // DELETE: api/Businesses/5
        /// <summary>
        /// Deletes DB record using API
        /// </summary>
        /// <param name="id">ID to be deleted from DB</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteBusiness(Guid id)
        {
            var element = await _bll.BusinessService.FindAsync(id);

            if (element == null)
            {
                return NotFound();
            }
            else
            {
                await _bll.BusinessService.RemoveAsync(id);
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
        private async Task<bool> BusinessExists(Guid id)
        {
            return (await _bll.BusinessService.FindAsync(id)) != null;
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