using System.Net.Http;
using NetlifySharp.Models;
using System;
using System.IO;
using System.Threading;
using System.Net.Http.Headers;
using System.IO.Compression;

namespace NetlifySharp.Operations.Sites
{
    public class UpdateSite : ResponseOperation<Site, UpdateSite>
    {
        private readonly Func<CancellationToken, HttpRequestMessage> _getRequest;

        internal UpdateSite(NetlifyClient client, string siteId, SiteSetup siteSetup)
            : base(client, NetlifyClient.SitesEndpoint.Append(siteId), new HttpMethod("PATCH"))
        {
            if (string.IsNullOrEmpty(siteId))
            {
                throw new ArgumentException("A site ID must be provided", nameof(siteId));
            }

            Body = siteSetup ?? throw new ArgumentNullException(nameof(siteSetup));
            _getRequest = ct => base.GetRequest(ct);
        }

        // Not in Open API specification
        internal UpdateSite(NetlifyClient client, string siteId, Stream zipStream)
            : base(client, NetlifyClient.SitesEndpoint.Append(siteId), HttpMethod.Put)
        {
            if (zipStream == null)
            {
                throw new ArgumentNullException(nameof(zipStream));
            }

            _getRequest = ct =>
            {
                HttpRequestMessage request = base.GetRequest(ct);
                request.Content = new StreamContent(zipStream);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
                return request;
            };
        }

        // Not in Open API specification
        internal UpdateSite(NetlifyClient client, string siteId, string directory)
            : base(client, NetlifyClient.SitesEndpoint.Append(siteId), HttpMethod.Put)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }
            if (!Directory.Exists(directory))
            {
                throw new ArgumentException("The directory must exist", nameof(directory));
            }

            _getRequest = ct =>
            {
                HttpRequestMessage request = base.GetRequest(ct);
                MemoryStream zipStream = new MemoryStream();
                using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    directory = Path.GetFullPath(directory);
                    int startIndex = directory.Length + 1;
                    foreach (string file in Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories))
                    {
                        // We need to normalize the path separator so non-Windows Netlify systems can unzip it
                        zipArchive.CreateEntryFromFile(file, Path.GetFullPath(file).Substring(startIndex).Replace('\\', '/'));
                    }
                }
                zipStream.Position = 0;
                request.Content = new StreamContent(zipStream);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
                return request;
            };
        }

        protected override HttpRequestMessage GetRequest(CancellationToken cancellationToken) => _getRequest(cancellationToken);
    }
}
