using Asp.Versioning;
using BLL.Contracts.App;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class BusinessPicturesController : ControllerBase
    {
        /// <summary>
        /// Reference to the BLL service
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// DTO mapper
        /// </summary>
        private readonly Public.DTO.Mappers.Manager.BusinessPictureMapper _mapper;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="bll">References to BLL</param>
        /// <param name="automapper">Mapper config</param>
        public BusinessPicturesController(IAppBLL bll, IMapper automapper)
        {
            _bll = bll;
            _mapper = new Public.DTO.Mappers.Manager.BusinessPictureMapper(automapper);
        }

        /// <summary>
        /// Get API data for single record if it user is this business manager  Via business ID
        /// </summary>
        /// <param name="id">businessId</param>
        /// <returns>Data for single record</returns>
        [HttpGet("business/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Manager.BusinessPicture>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicDtoV1Manager.BusinessPicture>>
            GetBusinessBusinessPictureViaBusinessId(Guid id)
        {
            var element = await _bll.BusinessPictureService.FindViaBusinessId(id);

            if (element == null)
            {
                return NotFound();
            }

            return _mapper.Map(element)!;
        }


        /*
        /// <summary>
        /// Get API data for single record if it user is this business manager 
        /// </summary>
        /// <param name="id">ID to be loaded from DB</param>
        /// <returns>Data for single record</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Manager.BusinessPicture>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicDtoV1Manager.BusinessPicture>> GetBusinessBusinessPicture(Guid id)
        {
            var element = await _bll.BusinessPictureService.FindAsync(id);

            if (element == null)
            {
                return NotFound();
            }

            return _mapper.Map(element)!;
        }


        
        // POST: api/BusinessPictures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create new record to DB using API requests using API
        /// </summary>
        /// <param name="element">Record to be created</param>
        /// <returns>Created record</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(PublicDtoV1Manager.BusinessPicture), StatusCodes.Status201Created)]
        public async Task<ActionResult<PublicDtoV1Manager.BusinessPicture>> PostBusinessPicture(
            [FromBody] PublicDtoV1Manager.Custom.BusinessPictureCreation element,
            [FromBody] IFormFile file)
        {
            if (await CheckIfUserIsSearchableBusinessManager(element.BusinessId))
            {
                return Unauthorized("User is not a manager of this business");
            }

            _bll.BusinessPictureService.Add(_mapper.Map(element)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetBusinessBusinessPicture", new { id = element.Id }, element);
        }

        // DELETE: api/BusinessPictures/5
        /// <summary>
        /// Deletes DB record using API
        /// </summary>
        /// <param name="id">ID to be deleted from DB</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteBusinessPicture(Guid id)
        {
            var element = await _bll.BusinessPictureService.FindAsync(id);

            if (element == null)
            {
                return NotFound();
            }

            if (await CheckIfUserIsSearchableBusinessManager(element.BusinessId))
            {
                return Unauthorized("User is not a manager of this business");
            }

            await _bll.BusinessPictureService.RemoveAsync(id);

            await _bll.SaveChangesAsync();
            return NoContent();
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
        
        */
    }
}