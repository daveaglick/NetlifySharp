using System.Threading.Tasks;

namespace NetlifySharp.Models
{
    public partial class Site : Model
    {
        // Not in Open API definition
        public PlanData PlanData { get; private set; }
        public string SiteId { get; private set; }
        public bool Premium { get; private set; }
        public bool Claimed { get; private set; }

        // Endpoints
        public async Task<Form[]> GetFormsAsync() => await Client.GetFormsAsync(Id);

        // Used for testing
        internal void SetId(string id) => Id = id;
    }
}
