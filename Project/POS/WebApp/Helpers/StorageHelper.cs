using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using WebApp.Models;

namespace WebApp.Helpers
{
    /// <summary>
    /// Represents storage helper for uploading files to azure storage
    /// </summary>
    public static class StorageHelper
    {
        /// <summary>
        /// Checks if file is image
        /// </summary>
        /// <param name="file">File to be checked</param>
        /// <returns>Booletan status</returns>
        public static bool IsImage(IFormFile file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };

            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Uploads file to azure storage
        /// </summary>
        /// <param name="fileStream">File stream</param>
        /// <param name="fileName">File name</param>
        /// <param name="storageConfig">Azure storage config</param>
        /// <returns></returns>
        public static async Task<bool> UploadFileToStorage(Stream fileStream, string fileName,
            AzureStorageConfig storageConfig)
        {
            // Create a URI to the blob
            Uri blobUri = new Uri("https://" +
                                  storageConfig.AccountName +
                                  ".blob.core.windows.net/" +
                                  storageConfig.ImageContainer +
                                  "/" + fileName);

            // Create StorageSharedKeyCredentials object by reading
            // the values from the configuration (appsettings.json)
            StorageSharedKeyCredential storageCredentials =
                new StorageSharedKeyCredential(storageConfig.AccountName, storageConfig.AccountKey);

            // Create the blob client.
            BlobClient blobClient = new BlobClient(blobUri, storageCredentials);

            // Upload the file
            await blobClient.UploadAsync(fileStream);

            return await Task.FromResult(true);
        }

        /// <summary>
        /// Returns all thumbnail urls
        /// </summary>
        /// <param name="storageConfig">Azure storage config</param>
        /// <returns></returns>
        public static async Task<List<string>> GetThumbNailUrls(AzureStorageConfig storageConfig)
        {
            List<string> thumbnailUrls = new List<string>();

            // Create a URI to the storage account
            Uri accountUri = new Uri("https://" + storageConfig.AccountName + ".blob.core.windows.net/");

            // Create BlobServiceClient from the account URI
            BlobServiceClient blobServiceClient = new BlobServiceClient(accountUri);

            // Get reference to the container
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(storageConfig.ThumbnailContainer);

            if (container.Exists())
            {
                foreach (BlobItem blobItem in container.GetBlobs())
                {
                    thumbnailUrls.Add(container.Uri + "/" + blobItem.Name);
                }
            }

            return await Task.FromResult(thumbnailUrls);
        }

        /// <summary>
        /// Returns  image urls
        /// </summary>
        /// <param name="imageName">Searchable image name</param>
        /// <param name="storageConfig">Azure storage config</param>
        /// <returns>Image path</returns>
        public static async Task<string?> GetImagePath(string imageName, AzureStorageConfig storageConfig)
        {
            List<string> thumbnailUrls = new List<string>();

            // Create a URI to the storage account
            Uri accountUri = new Uri("https://" + storageConfig.AccountName + ".blob.core.windows.net/");

            // Create BlobServiceClient from the account URI
            BlobServiceClient blobServiceClient = new BlobServiceClient(accountUri);

            // Get reference to the container
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(storageConfig.ImageContainer);

            if (container.Exists())
            {
                foreach (BlobItem blobItem in container.GetBlobs())
                {
                    if (blobItem.Name.Equals(imageName))
                    {
                        return await Task.FromResult(container.Uri + "/" + blobItem.Name);
                    }
                }
            }


            return null;
        }

        /*
        /// <summary>
        /// Deletes all thumbnails (aka operation marks all the specified blob and snapshot for deletion during next garbeage collecion )
        /// </summary>
        /// <param name="_storageConfig"></param>
        /// <returns></returns>
        public static async Task<List<int>> DeleteAllThumbnails(AzureStorageConfig _storageConfig)
        {
            List<string> thumbnailUrls = new List<string>();

            // Create a URI to the storage account
            Uri accountUri = new Uri("https://" + _storageConfig.AccountName + ".blob.core.windows.net/");

            // Create BlobServiceClient from the account URI
            BlobServiceClient blobServiceClient = new BlobServiceClient(accountUri);

            // Get reference to the container
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(_storageConfig.ThumbnailContainer);

            var responses = new List<int>();

            if (container.Exists())
            {
                foreach (BlobItem blobItem in container.GetBlobs())
                {
                    thumbnailUrls.Add(container.Uri + "/" + blobItem.Name);
                    var result = await container.DeleteBlobAsync(blobItem.Name, DeleteSnapshotsOption.IncludeSnapshots);
                    if (result != null)
                    {
                        responses.Add(result.Status);
                    }
                }
            }

            return responses;
        }
        
        */
    }
}