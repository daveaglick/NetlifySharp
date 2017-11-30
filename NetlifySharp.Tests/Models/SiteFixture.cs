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
        public void GetFormsUsesCorrectEndpoint()
        {
            // Given
            TestApiClient apiClient = new TestApiClient();
            NetlifyClient client = new NetlifyClient(apiClient);
            Site site = new Site
            {
                Client = client
            };
            site.SetId("50e9bfc8-e242-428d-ba2b-3ae7c2d9863f");

            // When
            Form[] result = site.GetFormsAsync().Result;

            // Then
            apiClient.SendAndReadCalls.ShouldContain((HttpMethod.Get, "sites/50e9bfc8-e242-428d-ba2b-3ae7c2d9863f/forms"));
        }
    }
}
