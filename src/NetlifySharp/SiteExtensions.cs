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
    public static class SiteExtensions
    {
        public static Task DeleteAsync(this Site site) => site.Client.DeleteSiteAsync(site.Id);
        public static Task DeleteAsync(this Site site, CancellationToken cancellationToken) => site.Client.DeleteSiteAsync(site.Id, cancellationToken);

        // TODO: Update site

        public static Task<ICollection<Form>> ListFormsAsync(this Site site) => site.Client.ListSiteFormsAsync(site.Id);
        public static Task<ICollection<Form>> ListFormsAsync(this Site site, CancellationToken cancellationToken) => site.Client.ListSiteFormsAsync(site.Id, cancellationToken);
    }
}