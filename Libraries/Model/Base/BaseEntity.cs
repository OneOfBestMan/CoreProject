using System;
using System.Runtime.Serialization;
using Dapper.Contrib.Extensions;

namespace Model
{
    /// <summary>
    /// Domain base model
    /// </summary>
    [Serializable, DataContract]
    public class BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Model.Base.BaseEntity"/> class.
        /// </summary>
        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Benzersiz id bilgisi
        /// </summary>
        [DataMember, ExplicitKey]
        public Guid Id { get; set; }

        /// <summary>
        /// Kayıt kaynak id bilgisi
        /// </summary>
        /// <value>Ios/Android/Web etc.</value>
        [DataMember]
        public Guid? SourceId { get; set; } = null;

        /// <summary>
        /// Yayın durumu
        /// </summary>
        /// <value><c>true: </c>yayında -  <c>false: </c> taslak</value>
        [DataMember]
        public bool Published { get; set; } = true;

        /// <summary>
        /// Silinme durumu
        /// </summary>
        /// <value><c>true: </c>silinmiş -  <c>false: </c> silinmemiş</value>
        [DataMember]
        public bool Deleted { get; set; } = false;

        /// <summary>
        /// Oluşturma tarihi
        /// </summary>
        [DataMember]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}