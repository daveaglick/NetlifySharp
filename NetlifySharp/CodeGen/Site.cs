
using System;

namespace NetlifySharp.Models
{
    public partial class Site : Model
    {
        public string AccountName { get; private set; }
        public string AccountSlug { get; private set; }
        public string AdminUrl { get; private set; }
        public SiteBuildSettings BuildSettings { get; private set; }
        public DateTime? CreatedAt { get; private set; }
        public string CustomDomain { get; private set; }
        public string DeployHook { get; private set; }
        public string DeployUrl { get; private set; }
        public string[] DomainAliases { get; private set; }
        public bool? ForceSsl { get; private set; }
        public string GitProvider { get; private set; }
        public string Id { get; private set; }
        public bool? ManagedDns { get; private set; }
        public string Name { get; private set; }
        public string NotificationEmail { get; private set; }
        public string Password { get; private set; }
        public string Plan { get; private set; }
        public Deploy PublishedDeploy { get; private set; }
        public string ScreenshotUrl { get; private set; }
        public string SessionId { get; private set; }
        public bool? Ssl { get; private set; }
        public string SslUrl { get; private set; }
        public string State { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string Url { get; private set; }
        public string UserId { get; private set; }
    }
}
