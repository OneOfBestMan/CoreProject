using System;
using System.Runtime.Serialization;
using Dapper.Contrib.Extensions;

namespace Model
{
    /// <summary>
    /// Application
    /// </summary>
    [Serializable, DataContract, Table("Applications")]
    public class Applications : BaseEntity
    {
        /// <summary>
        /// Application Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; } = null;

        /// <summary>
        /// Application Icon Url
        /// </summary>
        [DataMember]
        public string IconUrl { get; set; } = null;

        /// <summary>
        /// Web Site
        /// </summary>
        [DataMember]
        public string WebSite { get; set; } = null;

        /// <summary>
        /// Support Email
        /// </summary>
        [DataMember]
        public string SupportMail { get; set; } = null;
    }
}