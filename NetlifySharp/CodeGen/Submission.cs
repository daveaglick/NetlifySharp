
using System;

namespace NetlifySharp.Models
{
    public partial class Submission : Model
    {
        public string Body { get; private set; }
        public string Company { get; private set; }
        public DateTime? CreatedAt { get; private set; }
        public SubmissionData Data { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string Id { get; private set; }
        public string LastName { get; private set; }
        public string Name { get; private set; }
        public int? Number { get; private set; }
        public string SiteUrl { get; private set; }
        public string Summary { get; private set; }
    }
}
