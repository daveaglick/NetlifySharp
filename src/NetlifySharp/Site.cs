using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace NetlifySharp
{
    public partial class Site
    {
        public Task DeleteAsync() => Client.DeleteSiteAsync(Id);
        public Task DeleteAsync(CancellationToken cancellationToken) => Client.DeleteSiteAsync(Id, cancellationToken);

        public Task<Site> UpdateAsync(SiteSetup site) => Client.UpdateSiteAsync(site, Id);
        public Task<Site> UpdateAsync(SiteSetup site, CancellationToken cancellationToken) => Client.UpdateSiteAsync(site, Id, cancellationToken);

        public Task<Site> UpdateAsync(Stream zipStream) => Client.UpdateSiteAsync(zipStream, Id);
        public Task<Site> UpdateAsync(Stream zipStream, CancellationToken cancellationToken) => Client.UpdateSiteAsync(zipStream, Id, cancellationToken);

        public Task<Site> UpdateAsync(string directory) => Client.UpdateSiteAsync(directory, Id);
        public Task<Site> UpdateAsync(string directory, CancellationToken cancellationToken) => Client.UpdateSiteAsync(directory, Id, cancellationToken);

        public Task<ICollection<Form>> ListFormsAsync() => Client.ListSiteFormsAsync(Id);
        public Task<ICollection<Form>> ListFormsAsync(CancellationToken cancellationToken) => Client.ListSiteFormsAsync(Id, cancellationToken);
    }
}