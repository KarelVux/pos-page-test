using Asp.Versioning;
using BLL.Contracts.App;
using AutoMapper;
using Helpers.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicDtoV1Manager = Public.DTO.v1.Manager;

namespace WebApp.ApiControllers.Shop
{
    /// <summary>
    /// Management CRUD API controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/public/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderFeedbacksController : ControllerBase
    {
        /// <summary>
        /// Reference to the BLL service
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// DTO mapper
        /// </summary>
        private readonly Public.DTO.Mappers.Manager.OrderFeedbackMapper _mapper;


        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="bll">References to BLL</param>
        /// <param name="autoMapper">Mapper config</param>
        public OrderFeedbacksController(IAppBLL bll, IMapper autoMapper)
        {
            _bll = bll;
            _mapper = new Public.DTO.Mappers.Manager.OrderFeedbackMapper(autoMapper);
        }

        // GET: api/OrderFeedbacks/5
        /// <summary>
        /// Get API data for single record
        /// </summary>
        /// <param name="id">ID to be loaded from DB</param>
        /// <returns>Data for single record</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Manager.OrderFeedback>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDtoV1Manager.OrderFeedback>> GetUserOrderFeedback(Guid id)
        {
            
            var element = await _bll.OrderFeedbackService.FindAsync(id);
            
            if (element == null)
            {
                return NotFound();
            }

            if (!(await CheckIfOrderBelongsToUser(element.OrderId)))
            {
                return NotFound("Unable to find order for user");
            }

            return _mapper.Map(element)!;
        }

        /// <summary>
        /// Get API data for single record via order ID
        /// </summary>
        /// <param name="id">ID to be loaded from DB</param>
        /// <returns>Data for single record</returns>
        [HttpGet("{id}/orderId")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Manager.OrderFeedback>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDtoV1Manager.OrderFeedback>> GetUserOrderFeedbackViaOrderId(Guid id)
        {
            
            var element = await _bll.OrderFeedbackService.FinOrderedFeedbackViaOderId(id);
            
            if (element == null ||!(await CheckIfOrderBelongsToUser(element.OrderId)))
            {
                return NotFound("Unable to find order feedback for user because it is not added for this order");
            }

            return _mapper.Map(element)!;
        }



        // POST: api/OrderFeedbacks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create new record to DB using API requests using API
        /// </summary>
        /// <param name="element">Record to be created</param>
        /// <returns>Created record</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(PublicDtoV1Manager.OrderFeedback), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicDtoV1Manager.OrderFeedback>> PostUserOrderFeedback(
            PublicDtoV1Manager.OrderFeedback element)
        {
            if (!(await CheckIfOrderBelongsToUser(element.OrderId)))
            {
                return NotFound("Unable to find user order");
            }
            var order = await _bll.OrderService.FindAsyncWithInclude(element.OrderId);

            if (await CheckIfFeedbackHasBeenAddedToTheOrder(element.OrderId))
            {
                return BadRequest("Order has already feedback");

            }
            
            _bll.OrderFeedbackService.Add(_mapper.Map(element)!);
            await _bll.SaveChangesAsync();


            return CreatedAtAction("GetOrderFeedback", new { id = element.Id }, element);
        }

        /// <summary>
        /// Checks if order belongs to the user
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns>Boolean status</returns>
        private async Task<bool> CheckIfOrderBelongsToUser(Guid orderId)
        {
            var order = await _bll.OrderService.FindAsyncWithInclude(orderId);

            if (order == null)
            {
                return false;
            }

            if (order.Invoice != null && order.Invoice.AppUserId == User.GetUserId())
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Checks if feedback has been added to the order
        /// </summary>
        /// <param name="orderId">Order feedback ID</param>
        /// <returns>Boolean status</returns>
        private async Task<bool> CheckIfFeedbackHasBeenAddedToTheOrder(Guid orderId)
        {
            var order = await _bll.OrderFeedbackService.FinOrderedFeedbackViaOderId(orderId);

            if (order == null)
            {
                return false;
            }

            return false;
        }
    }
}