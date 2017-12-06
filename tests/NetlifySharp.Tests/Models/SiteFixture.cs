using NetlifySharp.Models;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NetlifySharp.Tests.Models
{
    [TestFixture]
    public class SiteFixture
    {
        [Test]
        public void ListSiteFormsUsesCorrectEndpoint()
        {
            // Given
            TestApiClient apiClient = new TestApiClient();
            NetlifyClient client = new NetlifyClient(apiClient);
            Site site = new Site(client);
            site.Id = "50e9bfc8-e242-428d-ba2b-3ae7c2d9863f";

            // When
            Form[] result = site.ListSiteForms().SendAsync().Result;

            // Then
            apiClient.Requests[0].Method.ShouldBe(HttpMethod.Get);
            apiClient.Requests[0].RequestUri.ToString().ShouldBe("/sites/50e9bfc8-e242-428d-ba2b-3ae7c2d9863f/forms");
        }
    }
}
