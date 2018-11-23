using System;
namespace Model
{
    /// <summary>
    /// Decrypt edilmiş token değerlerini barındırır.
    /// </summary>
    [Serializable]
    public class Token
    {
        /// <summary>
        /// Token doğruluğu.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Token geçerlilik tarihi
        /// </summary>
        public string ExpiryDate { get; set; }

        /// <summary>
        /// Source identifier
        /// </summary>
        public Guid SourceId { get; set; }

        /// <summary>
        /// Application identifier
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// User identifier
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Device identifier
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Bundle identifier / Package name
        /// </summary>
        public string BundleId { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Language
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Is Admin
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}