using Asp.Versioning;
using BLL.Contracts.App;
using AutoMapper;
using Helpers.Base;
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
        /// <param name="id">order feedback ID to be loaded from DB</param>
        /// <returns>Data for single record</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Manager.OrderFeedback>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDtoV1Manager.OrderFeedback>> GetOrderFeedback(Guid id)
        {
            var element = await _bll.OrderFeedbackService.FindAsync(id);

            if (element == null)
            {
                return NotFound("Order does not have an feedback");
            }

            bool status = await CheckIfUserIsBusinessManager(id);

            if (status)
            {
                return _mapper.Map(element)!;
            }

            return Unauthorized("User does not have an access to the feedback");
        }

        
        /// <summary>
        /// Get order feedback via orderId
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Data for single record</returns>
        [HttpGet("{id}/orderId")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Manager.OrderFeedback>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDtoV1Manager.OrderFeedback>> GetOrderFeedbackViaOrderId(Guid id)
        {
            var element = await _bll.OrderFeedbackService.FinOrderedFeedbackViaOderId(id);

            if (element == null)
            {
                return NotFound("Order does not have an feedback");
            }

            bool status = await CheckIfUserIsBusinessManager(element.Id);

            if (status)
            {
                return _mapper.Map(element)!;
            }

            return Unauthorized("User does not have an access to the feedback");
        }

        
        /// <summary>
        /// Checks if business user has access to the order feedback 
        /// </summary>
        /// <param name="orderFeedbackId">Order feedback ID</param>
        /// <returns>Boolean status and if not fount then returns NotFound action</returns>
        private async Task<bool> CheckIfUserIsBusinessManager(Guid orderFeedbackId)
        {
            var orderFeedback = await _bll.OrderFeedbackService.FindAsync(orderFeedbackId);

            if (orderFeedback == null)
            {
                return false;
            }


            var invoice = await _bll.InvoiceService.GetInvoiceViaOrderId(orderFeedback.OrderId);


            if (invoice == null)
            {
                return false;
            }

            var businessBelongsToUser =
                await _bll.BusinessManagerService.GetUserManagedBusiness(invoice.BusinessId, User.GetUserId());

            return businessBelongsToUser != null;
        }


    }
}