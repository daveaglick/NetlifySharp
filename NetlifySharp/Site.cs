using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NetlifySharp
{
    [DataContract]
    public class Site
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "premium")]
        public bool Premium { get; set; }

        [DataMember(Name = "claimed")]
        public bool Claimed { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "custom_domain")]
        public string CustomDomain { get; set; }

        [DataMember(Name = "notification_email")]
        public string NotificationEmail { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "admin_url")]
        public string AdminUrl { get; set; }

        [DataMember(Name = "screenshot_url")]
        public string ScreenshotUrl { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }

        [DataMember(Name = "user_id")]
        public string UserId { get; set; }


    }
}
