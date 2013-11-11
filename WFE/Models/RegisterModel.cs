using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WFE.Models
{
    [DataContract]
    public class RegisterRequestModel
    {
        [DataMember(Name = "gcm", IsRequired = true), Required]
        public string GcmId { get; set; }
    }

    [DataContract]
    public class RegisterResponseModel
    {
        [DataMember(Name = "id", IsRequired = true), Required]
        public int Id { get; set; }
    }

    [DataContract]
    public class MessageModel
    {
        [DataMember(Name = "to", IsRequired = true), Required]
        public int ReceiverId { get; set; }
        [DataMember(Name = "msg", IsRequired = true), Required]
        public string Message { get; set; }
    }
}
