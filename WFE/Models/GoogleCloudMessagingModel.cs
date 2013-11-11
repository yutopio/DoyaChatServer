using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;

namespace WFE.Models
{
    [DataContract]
    public class GcmMessage<T>
    {
        [DataMember(Name = "registration_ids", IsRequired = true), Required]
        public List<string> RegistrationIds { get; set; }
        // TODO: http://developer.android.com/google/gcm/notifications.html
        //[DataMember(Name="notification_key")]
        //string NotificationKey { get; set; }
        [DataMember(Name = "collapse_key")]
        public string CollapseKey { get; set; }
        [DataMember(Name = "data", IsRequired = true), Required]
        public T Data { get; set; }
        [DataMember(Name = "delay_while_idle")]
        public bool? DelayWhileIdle { get; set; }
        [DataMember(Name = "time_to_live")]
        public uint? TimeToLive { get; set; }
        [DataMember(Name = "restricted_package_name")]
        public string RestrictedPackageName { get; set; }
        [DataMember(Name = "dry_run")]
        public bool? DryRun { get; set; }

        public bool ShouldSerializeCollapseKey() { return !string.IsNullOrWhiteSpace(CollapseKey); }
        public bool ShouldSerializeDelayWhileIdle() { return DelayWhileIdle.HasValue; }
        public bool ShouldSerializeTimeToLive() { return TimeToLive.HasValue; }
        public bool ShouldSerializeRestrictedPackageName() { return !string.IsNullOrWhiteSpace(RestrictedPackageName); }
        public bool ShouldSerializeDryRun() { return DryRun.HasValue; }
    }

    public class GcmMessageStatus
    {
        public string message_id { get; set; }
        public string registration_id { get; set; }
        public string error { get; set; }
    }

    public class GcmResponse
    {
        public ulong multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public List<GcmMessageStatus> results { get; set; }
    }

    [DataContract]
    public class DoyaMessage<T>
    {
        [DataMember(Name = "tag", IsRequired = true), Required]
        public string Tag { get; set; }

        [DataMember(Name = "body")]
        public T Data { get; set; }
    }

    public class GoogleCloudMessagingModel : JsonApiProxy
    {
        static readonly string serverKey;

        static GoogleCloudMessagingModel()
        {
            serverKey = ConfigurationManager.AppSettings["GoogleApiServerKey"];
        }

        public override string BaseUri
        {
            get { return "https://android.googleapis.com/gcm/"; }
        }

        public GcmResponse Send<T>(GcmMessage<T> data)
        {
            var client = NewClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=" + serverKey);
            var response = client.PostAsJsonAsync("send", data).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<GcmResponse>().Result;
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                throw new InvalidDataContractException();
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            throw new InvalidOperationException();
        }
    }
}
