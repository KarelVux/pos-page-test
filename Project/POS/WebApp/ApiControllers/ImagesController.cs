using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// IMage upload Controller is only for root user
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Root")]
    public class ImagesController : Controller
    {
        // make sure that appsettings.json is filled with the necessary details of the azure storage
        private readonly AzureStorageConfig _storageConfig;

        /// <summary>
        /// Constructs a new instance of the ImagesController.
        /// </summary>
        /// <param name="config">Azure config.</param>
        public ImagesController(IOptions<AzureStorageConfig> config)
        {
            _storageConfig = config.Value;
        }

        /// <summary>
        /// Uploads an image file to Azure Blob Storage.
        /// </summary>
        /// <param name="file">The image file to upload.</param>
        /// <returns>The result of the upload operation.</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            bool isUploaded = false;
            string fileName = "";
            try
            {
                if (_storageConfig.AccountKey == string.Empty || _storageConfig.AccountName == string.Empty)
                    return BadRequest(
                        "sorry, can't retrieve your azure storage details from appsettings.js, make sure that you add azure storage details there");

                if (_storageConfig.ImageContainer == string.Empty)
                    return BadRequest("Please provide a name for your image container in the azure blob storage");


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
                    return Ok($"File [{fileName}] was uploaded successfully");
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
        /// Retrieves the URLs of thumbnail images from Azure Blob Storage.
        /// </summary>
        /// <returns>The URLs of the thumbnail images.</returns>
        [HttpGet("thumbnails")]
        public async Task<IActionResult> GetThumbNails()
        {
            try
            {
                if (_storageConfig.AccountKey == string.Empty || _storageConfig.AccountName == string.Empty)
                    return BadRequest(
                        "Sorry, can't retrieve your Azure storage details from appsettings.js, make sure that you add Azure storage details there.");

                if (_storageConfig.ImageContainer == string.Empty)
                    return BadRequest("Please provide a name for your image container in Azure blob storage.");

                List<string> thumbnailUrls = await StorageHelper.GetThumbNailUrls(_storageConfig);
                return new ObjectResult(thumbnailUrls);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /*
        // GET /api/images/thumbnails
        [HttpDelete("thumbnails")]
        public async Task<IActionResult> DeleteThumbNails()
        {
            try
            {
                if (_storageConfig.AccountKey == string.Empty || _storageConfig.AccountName == string.Empty)
                    return BadRequest(
                        "Sorry, can't retrieve your Azure storage details from appsettings.js, make sure that you add Azure storage details there.");

                if (_storageConfig.ImageContainer == string.Empty)
                    return BadRequest("Please provide a name for your image container in Azure blob storage.");

                var  thumbnailDeletionResults = await StorageHelper.DeleteAllThumbnails(_storageConfig);
                return new ObjectResult(thumbnailDeletionResults);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        */
    }
}