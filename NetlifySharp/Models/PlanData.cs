namespace NetlifySharp.Models
{
    // Not in Open API specification (https://github.com/netlify/open-api/issues/49)
    public class PlanData : Model
    {
        public string Title { get; set; }
        public bool AssetAcceleration { get; set; }
        public bool FormProcessing { get; set; }
        public string CdnPropagation { get; set; }
        public string BuildGcExchange { get; set; }
        public string BuildNodePool { get; set; }
        public bool DomainAliases { get; set; }
        public bool SecureSite { get; set; }
        public bool Prerendering { get; set; }
        public bool Proxying { get; set; }
        public string Ssl { get; set; }
        public int RateCents { get; set; }
        public int YearlyRateCents { get; set; }
        public string CdnNetwork { get; set; }
        public bool BranchDeploy { get; set; }
        public bool ManagedDns { get; set; }
        public bool GeoIp { get; set; }
        public bool SplitTesting { get; set; }
        public string Id { get; set; }
    }
}
