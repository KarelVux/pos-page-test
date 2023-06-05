using Asp.Versioning;
using AutoMapper;
using BLL.Contracts.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicDTO = Public.DTO.v1.Manager;

namespace WebApp.ApiControllers.Manager
{
    /// <summary>
    /// Management CRUD API controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/manager/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "BusinessManager")]    public class SettlementsController : ControllerBase
    {
        /// <summary>
        /// Reference to the BLL service
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// DTO mapper
        /// </summary>
        private readonly Public.DTO.Mappers.Manager.SettlementMapper _mapper;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="bll">References to BLL</param>
        /// <param name="autoMapper">Mapper config</param>
        public SettlementsController(IAppBLL bll, IMapper autoMapper)
        {
            _bll = bll;
            _mapper = new Public.DTO.Mappers.Manager.SettlementMapper(autoMapper);
        }

        // GET: api/Settlements
        /// <summary>
        /// Get all controller API records from DB
        /// </summary>
        /// <returns>All API records form db</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDTO.Settlement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PublicDTO.Settlement>>> GetSettlements()
        {
            var elements = await _bll.SettlementService.AllAsync();
            return elements.Select(x => _mapper.Map(x)).ToList()!;
        }

        // GET: api/Settlements/5
        /// <summary>
        /// Get API data for single record
        /// </summary>
        /// <param name="id">ID to be loaded from DB</param>
        /// <returns>Data for single record</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDTO.Settlement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDTO.Settlement>> GetSettlement(Guid id)
        {
            var element = await _bll.SettlementService.FindAsync(id);

            if (element == null)
            {
                return NotFound();
            }

            return _mapper.Map(element)!;
        }


        // POST: api/Settlements
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create new record to DB using API requests using API
        /// </summary>
        /// <param name="element">Record to be created</param>
        /// <returns>Created record</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(PublicDTO.Settlement), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDTO.Settlement>> PostSettlement(PublicDTO.Settlement element)
        {
            _bll.SettlementService.Add(_mapper.Map(element)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetSettlement", new { id = element.Id }, element);
        }
    }
}