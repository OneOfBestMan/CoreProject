using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// ListResponse Object
    /// </summary>
    public class ListResponse<T>
    {
        /// <summary>
        /// İstek durum kodu
        /// </summary>
        public ResponseCode ResponseCode { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// Ekstra paylaşılan data.
        /// </summary>
        public dynamic ExternalData { get; set; }
    }
}