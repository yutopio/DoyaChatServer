using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WFE.Models
{
    [DataContract]
    public class RegisterModel : ITableEntity
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name = "gcm")]
        public string GcmId { get; set; }

        public bool ShouldSerializeId() { return Id.HasValue; }
        public bool ShouldSerializeGcmId() { return !string.IsNullOrEmpty(GcmId); }

        public string ETag
        {
            get { return "*"; }
            set { throw new NotImplementedException(); }
        }

        public string PartitionKey
        {
            get { return "foo"; }
            set { throw new NotImplementedException(); }
        }

        public void ReadEntity(IDictionary<string, EntityProperty> properties,
            OperationContext operationContext)
        {
            Id = properties["id"].Int32Value.Value;
            GcmId = properties["gcm"].StringValue;
        }

        public string RowKey
        {
            get { return Id.ToString(); }
            set { Id = int.Parse(value); }
        }

        public DateTimeOffset Timestamp { get; set; }

        public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var ret = new Dictionary<string, EntityProperty>();
            ret.Add("id", new EntityProperty(Id));
            ret.Add("gcm", new EntityProperty(GcmId));
            return ret;
        }
    }

    [DataContract]
    public class MessageModel
    {
        [DataMember(Name = "to", IsRequired = true), Required]
        public int ReceiverId { get; set; }
        [DataMember(Name = "msg", IsRequired = true), Required]
        public string Message { get; set; }
    }
    
    [DataContract]
    public class FacePushModel
    {
        [DataMember(Name = "from", IsRequired = true), Required]
        public int SenderId { get; set; }
        [DataMember(Name = "le", IsRequired = true), Required]
        public int LeftEye { get; set; }
        [DataMember(Name = "re", IsRequired = true), Required]
        public int RightEye { get; set; }
        [DataMember(Name = "lb", IsRequired = true), Required]
        public int LeftBrow { get; set; }
        [DataMember(Name = "rb", IsRequired = true), Required]
        public int RightBrow { get; set; }
        [DataMember(Name = "m", IsRequired = true), Required]
        public int Mouth { get; set; }
        [DataMember(Name = "g", IsRequired = true), Required]
        public int Gender { get; set; }
    }

    [DataContract]
    public class MessagePushModel
    {
        [DataMember(Name = "from", IsRequired = true), Required]
        public int SenderId { get; set; }
        [DataMember(Name = "msg", IsRequired = true), Required]
        public string Message { get; set; }
    }
}
