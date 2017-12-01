using NUnit.Framework;
using Shouldly;
using System.Net.Http;
using NetlifySharp.Models;

namespace NetlifySharp.Tests.Operations
{
    [TestFixture]
    public class ListSiteFormsFixture
    {
        [Test]
        public void ListSiteFormsUsesCorrectEndpoint()
        {
            // Given
            TestApiClient apiClient = new TestApiClient();
            NetlifyClient client = new NetlifyClient(apiClient);

            // When
            Form[] result = client.ListSiteForms("50e9bfc8-e242-428d-ba2b-3ae7c2d9863f").SendAsync().Result;

            // Then
            apiClient.Requests[0].Method.ShouldBe(HttpMethod.Get);
            apiClient.Requests[0].RequestUri.ToString().ShouldBe("/sites/50e9bfc8-e242-428d-ba2b-3ae7c2d9863f/forms");
        }
    }
}
