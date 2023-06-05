using Asp.Versioning;
using AutoMapper;
using BLL.Contracts.App;
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
    public class BusinessCategoriesController : ControllerBase
    {
        /// <summary>
        /// Reference to the BLL service
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// DTO mapper
        /// </summary>
        private readonly Public.DTO.Mappers.Manager.BusinessCategoryMapper _mapper;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="bll">References to BLL</param>
        /// <param name="autoMapper">Mapper config</param>
        public BusinessCategoriesController(IAppBLL bll, IMapper autoMapper)
        {
            _bll = bll;
            _mapper = new Public.DTO.Mappers.Manager.BusinessCategoryMapper(autoMapper);
        }

        // GET: api/BusinessCategories
        /// <summary>
        /// Get all controller API records from DB
        /// </summary>
        /// <returns>All API records form db</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Manager.BusinessCategory>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PublicDtoV1Manager.BusinessCategory>>> GetBusinessCategories()
        {
            var elements = await _bll.BusinessCategoryService.AllAsync();
            return elements.Select(x => _mapper.Map(x)).ToList()!;
        }

        // GET: api/BusinessCategories/5
        /// <summary>
        /// Get API data for single record
        /// </summary>
        /// <param name="id">ID to be loaded from DB</param>
        /// <returns>Data for single record</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Manager.BusinessCategory>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDtoV1Manager.BusinessCategory>> GetBusinessCategory(Guid id)
        {
            var element = await _bll.BusinessCategoryService.FindAsync(id);

            if (element == null)
            {
                return NotFound();
            }

            return _mapper.Map(element)!;
        }


        // POST: api/BusinessCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create new record to DB using API requests using API
        /// </summary>
        /// <param name="element">Record to be created</param>
        /// <returns>Created record</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(PublicDtoV1Manager.BusinessCategory), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDtoV1Manager.BusinessCategory>> PostBusinessCategory(
            PublicDtoV1Manager.BusinessCategory element)
        {
            _bll.BusinessCategoryService.Add(_mapper.Map(element)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetBusinessCategory", new { id = element.Id }, element);
        }
    }
}