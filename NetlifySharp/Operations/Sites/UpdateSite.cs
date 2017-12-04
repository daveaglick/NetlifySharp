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
            if (siteSetup == null)
            {
                throw new ArgumentNullException(nameof(siteSetup));
            }

            Body = siteSetup;
            _getRequest = ct => base.GetRequest(ct);
        }

        // Not in Open API specification
        internal UpdateSite(NetlifyClient client, string siteId, FileInfo zipFile)
            : base(client, NetlifyClient.SitesEndpoint.Append(siteId), HttpMethod.Put)
        {
            if (zipFile == null)
            {
                throw new ArgumentNullException(nameof(zipFile));
            }
            if (!zipFile.Exists)
            {
                throw new ArgumentException("The zip file must exist", nameof(zipFile));
            }

            _getRequest = ct =>
            {
                HttpRequestMessage request = base.GetRequest(ct);
                request.Content = new StreamContent(zipFile.OpenRead());
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
                return request;
            };
        }

        // Not in Open API specification
        internal UpdateSite(NetlifyClient client, string siteId, DirectoryInfo directory)
            : base(client, NetlifyClient.SitesEndpoint.Append(siteId), HttpMethod.Put)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }
            if (!directory.Exists)
            {
                throw new ArgumentException("The diretory must exist", nameof(directory));
            }

            _getRequest = ct =>
            {
                HttpRequestMessage request = base.GetRequest(ct);
                MemoryStream zipStream = new MemoryStream();
                using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach(string file in Directory.EnumerateFiles(directory.FullName, "*", SearchOption.AllDirectories))
                    {
                        zipArchive.CreateEntryFromFile(file,
                            Path.GetFullPath(directory.FullName).Substring(Path.GetFullPath(file).Length + 1));
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
