
using System;

namespace NetlifySharp.Models
{
    public partial class Deploy : Model
    {
        public string AdminUrl { get; private set; }
        public string Branch { get; private set; }
        public string BuildId { get; private set; }
        public string CommitRef { get; private set; }
        public string CommitUrl { get; private set; }
        public string Context { get; private set; }
        public DateTime? CreatedAt { get; private set; }
        public string DeploySslUrl { get; private set; }
        public string DeployUrl { get; private set; }
        public bool? Draft { get; private set; }
        public string ErrorMessage { get; private set; }
        public string Id { get; private set; }
        public bool? Locked { get; private set; }
        public string Name { get; private set; }
        public DateTime? PublishedAt { get; private set; }
        public string[] Required { get; private set; }
        public string[] RequiredFunctions { get; private set; }
        public int? ReviewId { get; private set; }
        public string ReviewUrl { get; private set; }
        public string ScreenshotUrl { get; private set; }
        public string SiteId { get; private set; }
        public bool? Skipped { get; private set; }
        public string SslUrl { get; private set; }
        public string State { get; private set; }
        public string Title { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string Url { get; private set; }
        public string UserId { get; private set; }
    }
}
