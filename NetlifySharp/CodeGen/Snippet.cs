
using System;

namespace NetlifySharp.Models
{
    public partial class Snippet : Model
    {
        public string General { get; private set; }
        public string GeneralPosition { get; private set; }
        public string Goal { get; private set; }
        public string GoalPosition { get; private set; }
        public int? Id { get; private set; }
        public string SiteId { get; private set; }
        public string Title { get; private set; }
    }
}
