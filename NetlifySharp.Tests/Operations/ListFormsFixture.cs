using NUnit.Framework;
using Shouldly;
using System.Net.Http;
using NetlifySharp.Models;

namespace NetlifySharp.Tests.Operations
{
    [TestFixture]
    public class ListFormsFixture
    {
        [Test]
        public void ListFormsUsesCorrectEndpoint()
        {
            // Given
            TestApiClient apiClient = new TestApiClient();
            NetlifyClient client = new NetlifyClient(apiClient);

            // When
            Form[] result = client.ListForms().SendAsync().Result;

            // Then
            apiClient.Requests[0].Method.ShouldBe(HttpMethod.Get);
            apiClient.Requests[0].RequestUri.ToString().ShouldBe("/forms");
        }
    }
}
