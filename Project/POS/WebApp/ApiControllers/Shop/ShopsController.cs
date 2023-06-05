using System.Net.Mime;
using Asp.Versioning;
using AutoMapper;
using BLL.Contracts.App;
using BLL.DTO.Shop;
using Helpers.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.v1;
using PublicDtoV1Shop = Public.DTO.v1.Shop;
using BllDto = BLL.DTO;

namespace WebApp.ApiControllers.Shop
{
    /// <summary>
    /// Public API for shop
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/public/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ShopsController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the mapper
        /// </summary>
        private readonly Public.DTO.Mappers.Shop.BusinessMapper _businessMapper;


        /// <summary>
        /// Reference to the BLL service
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// Construct
        /// </summary>
        /// <param name="bll">BLL</param>
        /// <param name="autoMapper">Mapper</param>
        public ShopsController(IMapper autoMapper, IAppBLL bll)
        {
            _bll = bll;
            _businessMapper = new Public.DTO.Mappers.Shop.BusinessMapper(autoMapper);
        }

        /// <summary>
        /// Gets the all businesses with the specified settlement and business category
        /// </summary>
        /// <param name="settlementId">Settlement Id</param>
        /// <param name="businessCategoryId">Business Id</param>
        /// <returns>List of businesses</returns>
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<PublicDtoV1Shop.Business>>> GetBusinesses(
            [FromQuery] Guid settlementId,
            [FromQuery] Guid? businessCategoryId = null)
        {
            if (!(await SettlementExists(settlementId)))
            {
                return NotFound($"Settlement [{settlementId.ToString()}] was not found");
            }


            if (businessCategoryId != null && !(await BusinessCategoryExists(businessCategoryId.Value)))
            {
                return NotFound($"Business category [{settlementId.ToString()}] was not found");
            }


            var elements =
                await _bll.BusinessService.GetBusinessesWithIncludes(settlementId, businessCategoryId);

            var res = elements != null
                ? elements
                    .Select(x => _businessMapper.Map(x))
                    .ToList()!
                : new List<PublicDtoV1Shop.Business>();

            return res;
        }

        /// <summary>
        /// Gets specified business with products
        /// </summary>
        /// <param name="id">Busienss Id</param>
        /// <returns>A business with products</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PublicDtoV1Shop.Business>> GetBusiness(Guid id)
        {
            if (!(await BusinessExists(id)))
            {
                return NotFound($"Business [{id.ToString()}] was not found");
            }

            var businessDetails =
                await _bll.BusinessService.GetBusinessDetailsPageData(id);

            businessDetails!.Products =
                (await _bll.ProductService.GetBusinessProductsWithIncludes(id, false))
                .Where(p => p.UnitCount > 0)
                .ToList();
            
            return _businessMapper.Map(businessDetails)!;
        }

        /// <summary>
        /// Gets specified business information    
        /// </summary>
        /// <param name="id">Busienss Id</param>
        /// <returns>A Business info</returns>
        [HttpGet("{id}/info")]
        public async Task<ActionResult<PublicDtoV1Shop.Business>> GetBusinessInfo(Guid id)
        {
            if (!(await BusinessExists(id)))
            {
                return NotFound($"Business [{id.ToString()}] was not found");
            }

            var businessDetails =
                await _bll.BusinessService.FindAsync(id);

            return _businessMapper.Map(businessDetails)!;
        }

        /// <summary>
        /// Checks if settlement exists
        /// </summary>
        /// <param name="id">Settlement id</param>
        /// <returns>Existence status</returns>
        private async Task<bool> SettlementExists(Guid id)
        {
            return (await _bll.SettlementService.FindAsync(id)) != null;
        }


        /// <summary>
        /// checks if business exists
        /// </summary>
        /// <param name="id">Business ID</param>
        /// <returns>Existence status</returns>
        private async Task<bool> BusinessExists(Guid id)
        {
            return (await _bll.BusinessService.FindAsync(id)) != null;
        }

        /// <summary>
        /// checks if business category exists
        /// </summary>
        /// <param name="id">Business category ID</param>
        /// <returns>Existence status</returns>
        private async Task<bool> BusinessCategoryExists(Guid id)
        {
            return (await _bll.BusinessCategoryService.FindAsync(id)) != null;
        }
    }
}