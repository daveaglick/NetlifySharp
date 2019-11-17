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
    public partial class NetlifyClient
    {
        private readonly string _accessToken;
        private readonly HttpClient _httpClient;  // The HttpClient if provided

        public NetlifyClient(string accessToken)
            : this(accessToken, null)
        {
        }

        public NetlifyClient(string accessToken, HttpClient httpClient)
            : this()
        {
            _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            if (accessToken.Any(x => char.IsWhiteSpace(x) || char.IsControl(x)))
            {
                throw new ArgumentException("Invalid access token", nameof(accessToken));
            }
            _httpClient = httpClient;
        }

        private Task<HttpClient> CreateHttpClientAsync(CancellationToken cancellationToken) =>
            Task.FromResult(_httpClient == null ? new HttpClient() : new HttpClientWrapper(_httpClient));

        partial void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings)
        {
            settings.Converters.Add(new ClientModelCreationConverter(this));
        }

        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        {
            request.Headers.Add("User-Agent", nameof(NetlifySharp));
            request.Headers.Add("Authorization", "Bearer " + _accessToken);
        }

        public Task<Site> UpdateSiteAsync(Stream zipStream, string siteId) =>
            UpdateSiteAsync(zipStream, siteId, System.Threading.CancellationToken.None);

        public async Task<Site> UpdateSiteAsync(Stream zipStream, string siteId, System.Threading.CancellationToken cancellationToken)
        {
            _ = siteId ?? throw new ArgumentNullException(nameof(siteId));

            System.Text.StringBuilder urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : string.Empty).Append("/sites/{site_id}");
            urlBuilder_.Replace("{site_id}", Uri.EscapeDataString(ConvertToString(siteId, System.Globalization.CultureInfo.InvariantCulture)));

            HttpClient client_ = _httpClient;
            try
            {
                using (HttpRequestMessage request_ = new HttpRequestMessage())
                {
                    StreamContent content_ = new StreamContent(zipStream);
                    content_.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/zip");
                    request_.Content = content_;
                    request_.Method = new HttpMethod("PUT");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client_, request_, urlBuilder_);
                    string url_ = urlBuilder_.ToString();
                    request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        Dictionary<string, IEnumerable<string>> headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (KeyValuePair<string, IEnumerable<string>> item_ in response_.Content.Headers)
                            {
                                headers_[item_.Key] = item_.Value;
                            }
                        }

                        ProcessResponse(client_, response_);

                        string status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            ObjectResponseResult<Site> objectResponse_ = await ReadObjectResponseAsync<Site>(response_, headers_).ConfigureAwait(false);
                            return objectResponse_.Object;
                        }
                        else
                        {
                            ObjectResponseResult<Error> objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new NetlifyException<Error>("error", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                    }
                    finally
                    {
                        if (response_ != null)
                        {
                            response_.Dispose();
                        }
                    }
                }
            }
            finally
            {
            }
        }

        public Task<Site> UpdateSiteAsync(string directory, string siteId) =>
            UpdateSiteAsync(directory, siteId, System.Threading.CancellationToken.None);

        public Task<Site> UpdateSiteAsync(string directory, string siteId, System.Threading.CancellationToken cancellationToken)
        {
            _ = directory ?? throw new ArgumentNullException(nameof(directory));
            if (!Directory.Exists(directory))
            {
                throw new ArgumentException("The directory must exist", nameof(directory));
            }

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

            return UpdateSiteAsync(zipStream, siteId, cancellationToken);
        }
    }
}