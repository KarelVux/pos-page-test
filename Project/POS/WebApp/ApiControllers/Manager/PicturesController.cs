using System.Net;
using Asp.Versioning;
using AutoMapper;
using BLL.Contracts.App;
using Helpers.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApp.Helpers;
using WebApp.Models;
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
    [Authorize(Roles = "BusinessManager")]
    public class PicturesController : ControllerBase
    {
        /// <summary>
        /// Reference to the BLL service
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// Configuration for Azure Storage (image holding)
        /// </summary>
        private readonly AzureStorageConfig _storageConfig;

        /// <summary>
        /// DTO mapper
        /// </summary>
        private readonly Public.DTO.Mappers.Manager.PictureMapper _mapper;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="bll">References to BLL</param>
        /// <param name="autoMapper">Mapper config</param>
        /// <param name="config">User to get access to Azure storage config</param>
        public PicturesController(IAppBLL bll, IMapper autoMapper, IOptions<AzureStorageConfig> config)
        {
            _bll = bll;
            _mapper = new Public.DTO.Mappers.Manager.PictureMapper(autoMapper);
            _storageConfig = config.Value;
        }

        // GET: api/Pictures/5
        /// <summary>
        /// Get API data for single record
        /// </summary>
        /// <param name="id">ID to be loaded from DB</param>
        /// <returns>Data for single record</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDTO.Picture>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDTO.Picture>> GetManagerPicture(Guid id)
        {
            var element = await _bll.PictureService.FindAsync(id);

            if (element == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(element);
            return res!;
        }

        /// <summary>
        /// Upload product picture and create a reference with business
        /// </summary>
        /// <param name="id">Business Id</param>
        /// <param name="file">upload able image</param>
        [HttpPost("business/{id}")]
        [ProducesResponseType(typeof(PublicDTO.Picture), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicDTO.Picture>> PostBusinessPicture(Guid id, IFormFile file)
        {
            var result = await CheckIfUserIsSearchableBusinessManager(id);

            if (!result)
            {
                return BadRequest("User does not have access to the business");
            }

            bool isUploaded = false;
            string fileName = "";
            try
            {
                if (_storageConfig.AccountKey == string.Empty ||
                    _storageConfig.AccountName == string.Empty ||
                    _storageConfig.ImageContainer == string.Empty)
                    return BadRequest(
                        "Configuration problems. Please check service configuration");

                if (StorageHelper.IsImage(file))
                {
                    if (file.Length > 0)
                    {
                        using (Stream stream = file.OpenReadStream())
                        {
                            fileName = Guid.NewGuid() + $".{file.FileName.Split(".").Last()}";
                            isUploaded = await StorageHelper.UploadFileToStorage(stream, fileName, _storageConfig);
                        }
                    }
                }
                else
                {
                    return new UnsupportedMediaTypeResult();
                }


                if (isUploaded)
                {
                    var imagePath = await StorageHelper.GetImagePath(fileName, _storageConfig);

                    if (string.IsNullOrWhiteSpace(imagePath))
                    {
                        return BadRequest("Unable to find uploaded image from storage");
                    }

                    var picture = new BLL.DTO.Picture
                    {
                        Id = Guid.NewGuid(),
                        Title = fileName,
                        Path = imagePath,
                    };
                    _bll.PictureService.Add(picture);


                    var businessPicture = new BLL.DTO.BusinessPicture
                    {
                        Id = Guid.NewGuid(),
                        PictureId = picture.Id,
                        BusinessId = id,
                    };
                    _bll.BusinessPictureService.Add(businessPicture);

                    await _bll.SaveChangesAsync();

                    return CreatedAtAction("GetManagerPicture", new { id = picture.Id }, picture);
                }
                else
                    return BadRequest("Look like the image couldnt upload to the storage");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Upload product picture and create a reference with business
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <param name="file">upload able image</param>
        [HttpPost("product/{id}")]
        [ProducesResponseType(typeof(PublicDTO.Picture), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicDTO.Picture>> PostProductPicture(Guid id, IFormFile file)
        {
            var product = await _bll.ProductService.FindAsync(id);

            if (product == null)
            {
                return BadRequest("Product does not exist");
            }

            var result = await CheckIfUserIsSearchableBusinessManager(product.BusinessId);

            if (!result)
            {
                return BadRequest("User does not have access to the business");
            }

            bool isUploaded = false;
            string fileName = "";
            try
            {
                if (_storageConfig.AccountKey == string.Empty ||
                    _storageConfig.AccountName == string.Empty ||
                    _storageConfig.ImageContainer == string.Empty)
                    return BadRequest(
                        "Configuration problems. Please check service configuration");

                if (StorageHelper.IsImage(file))
                {
                    if (file.Length > 0)
                    {
                        using (Stream stream = file.OpenReadStream())
                        {
                            fileName = Guid.NewGuid() + $".{file.FileName.Split(".").Last()}";
                            isUploaded = await StorageHelper.UploadFileToStorage(stream, fileName, _storageConfig);
                        }
                    }
                }
                else
                {
                    return new UnsupportedMediaTypeResult();
                }


                if (isUploaded)
                {
                    var imagePath = await StorageHelper.GetImagePath(fileName, _storageConfig);

                    if (string.IsNullOrWhiteSpace(imagePath))
                    {
                        return BadRequest("Unable to find uploaded image from storage");
                    }

                    var picture = new BLL.DTO.Picture
                    {
                        Id = Guid.NewGuid(),
                        Title = fileName,
                        Path = imagePath,
                    };
                    _bll.PictureService.Add(picture);


                    var businessPicture = new BLL.DTO.ProductPicture()
                    {
                        Id = Guid.NewGuid(),
                        PictureId = picture.Id,
                        ProductId = id,
                    };
                    _bll.ProductPictureService.Add(businessPicture);

                    await _bll.SaveChangesAsync();

                    return CreatedAtAction("GetManagerPicture", new { id = picture.Id }, picture);
                }
                else
                    return BadRequest("Look like the image couldnt upload to the storage");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Pictures/5
        /// <summary>
        /// Deletes DB record using API
        /// </summary>
        /// <param name="id">ID to be deleted from DB</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePicture(Guid id)
        {
            var businessId = await FindBusinessIdThatOwnsThePicture(id);

            if (businessId == Guid.Empty)
            {
                return BadRequest("Unable to find business that owns the picture");
            }

            var result = await CheckIfUserIsSearchableBusinessManager(businessId);

            if (!result)
            {
                return BadRequest("User does not have access to the business");
            }

            var element = await _bll.PictureService.FindAsync(id);

            if (element == null)
            {
                return NotFound();
            }
            else
            {
                // TODO: add picture removal for Azure Storage (currently deletion access  problems)
                await _bll.PictureService.RemoveAsync(id);
            }

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


        /// <summary>
        /// Business ID that owns the picture
        /// </summary>
        /// <param name="pictureId">Picture Id</param>
        /// <returns>Boolean status and if not fount then returns NotFound action</returns>
        private async Task<Guid> FindBusinessIdThatOwnsThePicture(Guid pictureId)
        {
            var businessPicture =
                await _bll.BusinessPictureService.FindViaPictureId(pictureId);

            if (businessPicture != null)
            {
                return businessPicture.BusinessId;
            }

            var productPicture =
                await _bll.ProductPictureService.FindViaPictureId(pictureId);

            if (productPicture != null)
            {
                var product = await _bll.ProductService.FindAsync(productPicture.ProductId);
                if (product == null)
                {
                    return Guid.Empty;
                }

                return product.BusinessId;
            }

            return Guid.Empty;
        }
    }
}