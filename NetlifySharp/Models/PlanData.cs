namespace NetlifySharp.Models
{
    // Not in Open API definition
    public class PlanData : Model
    {
        public string Title { get; private set; }
        public bool AssetAcceleration { get; private set; }
        public bool FormProcessing { get; private set; }
        public string CdnPropagation { get; private set; }
        public string BuildGcExchange { get; private set; }
        public string BuildNodePool { get; private set; }
        public bool DomainAliases { get; private set; }
        public bool SecureSite { get; private set; }
        public bool Prerendering { get; private set; }
        public bool Proxying { get; private set; }
        public string Ssl { get; private set; }
        public int RateCents { get; private set; }
        public int YearlyRateCents { get; private set; }
        public string CdnNetwork { get; private set; }
        public bool BranchDeploy { get; private set; }
        public bool ManagedDns { get; private set; }
        public bool GeoIp { get; private set; }
        public bool SplitTesting { get; private set; }
        public string Id { get; private set; }
    }
}
