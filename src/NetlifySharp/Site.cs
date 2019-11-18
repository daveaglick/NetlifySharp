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
        public Task<Site> UpdateSiteAsync(Stream zipStream) => Client.UpdateSiteAsync(zipStream, Id);
        public Task<Site> UpdateSiteAsync(Stream zipStream, CancellationToken cancellationToken) => Client.UpdateSiteAsync(zipStream, Id, cancellationToken);
        public Task<Site> UpdateSiteAsync(string directory) => Client.UpdateSiteAsync(directory, Id);
        public Task<Site> UpdateSiteAsync(string directory, CancellationToken cancellationToken) => Client.UpdateSiteAsync(directory, Id, cancellationToken);
    }
}