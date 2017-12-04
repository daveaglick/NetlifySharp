using NetlifySharp.Models;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NetlifySharp.Tests.Operations.Sites
{
    [TestFixture]
    public class ListSitesFixture
    {
        [Test]
        public void ListSitesUsesCorrectEndpoint()
        {
            // Given
            TestApiClient apiClient = new TestApiClient();
            NetlifyClient client = new NetlifyClient(apiClient);

            // When
            Site[] result = client.ListSites().SendAsync().Result;

            // Then
            apiClient.Requests[0].Method.ShouldBe(HttpMethod.Get);
            apiClient.Requests[0].RequestUri.ToString().ShouldBe("/sites");
        }

        [Test]
        public void ListSitesParsesJson()
        {
            // Given
            TestApiClient apiClient = new TestApiClient().WithResourceResposeContent("Sites.json");
            NetlifyClient client = new NetlifyClient(apiClient);

            // When
            Site[] result = client.ListSites().SendAsync().Result;

            // Then
            result.Length.ShouldBe(2);
            GetSiteFixture.VerifySite(result[0]);
            result[1].Id.ShouldBe("46b48455-90b4-4559-8233-6f6d08194696");
        }
    }
}
