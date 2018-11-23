using System;
using System.Runtime.Serialization;

namespace Model
{
    /// <summary>
    /// Response Object
    /// </summary>
    [DataContract, Serializable]
    public class Response<T>
    {
        /// <summary>
        /// İstek durum kodu
        /// </summary>
        [DataMember]
        public ResponseCode ResponseCode { get; set; } = ResponseCode.Success;

        /// <summary>
        /// Data
        /// </summary>
        [DataMember]
        public T Data { get; set; }

        /// <summary>
        /// Ekstra paylaşılan data.
        /// </summary>
        [DataMember]
        public dynamic ExternalData { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Model.Response`1"/> class.
        /// </summary>
        public Response() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Model.Response`1"/> class.
        /// </summary>
        /// <param name="responseCode">Istek durum kodu.</param>
        public Response(ResponseCode responseCode)
        {
            ResponseCode = responseCode;
        }
    }
}