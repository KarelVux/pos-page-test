namespace WebApp.Models
{
    /// <summary>
    /// Represents azure storage config
    /// </summary>
    public class AzureStorageConfig
    {
        /// <summary>
        /// Represents azure account name
        /// </summary>
        public string AccountName { get; set; } = default!;

        /// <summary>
        /// Represents azure account key
        /// </summary>
        public string AccountKey { get; set; } = default!;

        /// <summary>
        /// Represents azure image container folder
        /// </summary>
        public string ImageContainer { get; set; } = default!;

        /// <summary>
        /// Represents azure thumbnail container folder
        /// </summary>
        public string ThumbnailContainer { get; set; } = default!;
    }
}