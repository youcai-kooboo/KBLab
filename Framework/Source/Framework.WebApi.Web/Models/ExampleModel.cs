using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Framework.WebApi.Web.Models
{
    [DataContract]
    public class ExampleModel
    {
        [DataMember(IsRequired = true)]
        [Required]
        public int? Points { get; set; }

        [DataMember]
        public int? ProductID { get; set; }
    }
}