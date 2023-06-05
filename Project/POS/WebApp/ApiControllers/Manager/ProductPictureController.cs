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
    /// Represents ProductPicture API functionalities
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/manager/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "BusinessManager")]
    public class ProductPictureController : ControllerBase
    {
        /// <summary>
        /// Reference to the BLL service
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// DTO mapper
        /// </summary>
        private readonly Public.DTO.Mappers.Manager.ProductPictureMapper _mapper;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="bll">References to BLL</param>
        /// <param name="automapper">Mapper config</param>
        public ProductPictureController(IAppBLL bll, IMapper automapper)
        {
            _bll = bll;
            _mapper = new Public.DTO.Mappers.Manager.ProductPictureMapper(automapper);
        }

        /// <summary>
        /// Get API data for single record if it user is this business manager  Via business ID
        /// </summary>
        /// <param name="id">product Id</param>
        /// <returns>Data for single record</returns>
        [HttpGet("product/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDtoV1Manager.ProductPicture>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicDtoV1Manager.ProductPicture>>
            GetProductPicturePictureViaProductId(Guid id)
        {
            var element = await _bll.ProductPictureService.FindViaProductId(id);

            if (element == null)
            {
                return NotFound();
            }

            return _mapper.Map(element)!;
        }


        /*
        // PUT: api/ProductPicture/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductPicture(Guid id, ProductPicture productPicture)
        {
            if (id != productPicture.Id)
            {
                return BadRequest();
            }

            _context.Entry(productPicture).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductPictureExists(id))
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

        // POST: api/ProductPicture
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductPicture>> PostProductPicture(ProductPicture productPicture)
        {
            if (_context.ProductPictures == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ProductPictures'  is null.");
            }

            _context.ProductPictures.Add(productPicture);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductPicture", new { id = productPicture.Id }, productPicture);
        }

        // DELETE: api/ProductPicture/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductPicture(Guid id)
        {
            if (_context.ProductPictures == null)
            {
                return NotFound();
            }

            var productPicture = await _context.ProductPictures.FindAsync(id);
            if (productPicture == null)
            {
                return NotFound();
            }

            _context.ProductPictures.Remove(productPicture);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductPictureExists(Guid id)
        {
            return (_context.ProductPictures?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
        */
    }
}