using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NetlifySharp
{
    public class Site : Model
    {
        public string Id { get; private set; }
        public bool Premium { get; private set; }
        public bool Claimed { get; private set; }
        public string Name { get; private set; }
        public string CustomDomain { get; private set; }
        public string NotificationEmail { get; private set; }
        public string Url { get; private set; }
        public string AdminUrl { get; private set; }
        public string ScreenshotUrl { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string UserId { get; private set; }

        public async Task<Form[]> GetFormsAsync() => await Client.GetFormsAsync(Id);
    }
}
