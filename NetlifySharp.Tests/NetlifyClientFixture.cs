using NUnit.Framework;
using Shouldly;
using System;
using System.Net.Http;

namespace NetlifySharp.Tests
{
    [TestFixture]
    public class NetlifyClientFixture
    {
        [Test]
        public void GetSitesUsesCorrectEndpoint()
        {
            // Given
            TestApiClient apiClient = new TestApiClient();
            NetlifyClient client = new NetlifyClient(apiClient);

            // When
            Site[] result = client.GetSitesAsync().Result;

            // Then
            apiClient.SendAndReadCalls.ShouldContain((HttpMethod.Get, "sites"));
        }

        [Test]
        public void GetSitesParsesJson()
        {
            // Given
            TestApiClient apiClient = new TestApiClient().WithResourceRespose("Sites.json");
            NetlifyClient client = new NetlifyClient(apiClient);

            // When
            Site[] result = client.GetSitesAsync().Result;

            // Then
            result.Length.ShouldBe(2);

            result[0].Id.ShouldBe("50e9bfc8-e242-428d-ba2b-3ae7c2d9863f");
            result[0].Premium.ShouldBe(false);
            result[0].Claimed.ShouldBe(true);
            result[0].Name.ShouldBe("daveaglick");
            result[0].CustomDomain.ShouldBe("daveaglick.com");
            result[0].NotificationEmail.ShouldBe(null);
            result[0].Url.ShouldBe("https://daveaglick.com");
            result[0].AdminUrl.ShouldBe("https://app.netlify.com/sites/daveaglick");
            result[0].ScreenshotUrl.ShouldBe("https://353a23c500dde3b2ad58-c49fe7e7355d384845270f4a7a0a7aa1.ssl.cf2.rackcdn.com/5a00dfb80b79b731343d0c65/screenshot.png");
            result[0].CreatedAt.ShouldBe(DateTime.Parse("2016-12-20T20:07:56.305Z", null, System.Globalization.DateTimeStyles.RoundtripKind));
            result[0].UpdatedAt.ShouldBe(DateTime.Parse("2017-11-06T22:19:20.128Z", null, System.Globalization.DateTimeStyles.RoundtripKind));
            result[0].UserId.ShouldBe("58543cf0c4d9cc4e6d4bf27a");

            result[1].Id.ShouldBe("46b48455-90b4-4559-8233-6f6d08194696");
            result[1].Premium.ShouldBe(false);
            result[1].Claimed.ShouldBe(true);
            result[1].Name.ShouldBe("wyam");
            result[1].CustomDomain.ShouldBe("wyam.io");
            result[1].NotificationEmail.ShouldBe(null);
            result[1].Url.ShouldBe("https://wyam.io");
            result[1].AdminUrl.ShouldBe("https://app.netlify.com/sites/wyam");
            result[1].ScreenshotUrl.ShouldBe("https://353a23c500dde3b2ad58-c49fe7e7355d384845270f4a7a0a7aa1.ssl.cf2.rackcdn.com/5a108c1c0b79b71c127d94d3/screenshot.png");
            result[1].CreatedAt.ShouldBe(DateTime.Parse("2016-12-16T20:02:48.841Z", null, System.Globalization.DateTimeStyles.RoundtripKind));
            result[1].UpdatedAt.ShouldBe(DateTime.Parse("2017-11-18T19:38:32.763Z", null, System.Globalization.DateTimeStyles.RoundtripKind));
            result[1].UserId.ShouldBe("58543cf0c4d9cc4e6d4bf27a");
        }

        [Test]
        public void GetSiteUsesCorrectEndpoint()
        {
            // Given
            TestApiClient apiClient = new TestApiClient();
            NetlifyClient client = new NetlifyClient(apiClient);

            // When
            Site result = client.GetSiteAsync("50e9bfc8-e242-428d-ba2b-3ae7c2d9863f").Result;

            // Then
            apiClient.SendAndReadCalls.ShouldContain((HttpMethod.Get, "sites/50e9bfc8-e242-428d-ba2b-3ae7c2d9863f"));
        }

        [Test]
        public void GetSiteParsesJson()
        {
            // Given
            TestApiClient apiClient = new TestApiClient().WithResourceRespose("Site.json");
            NetlifyClient client = new NetlifyClient(apiClient);

            // When
            Site result = client.GetSiteAsync("50e9bfc8-e242-428d-ba2b-3ae7c2d9863f").Result;

            // Then
            result.ShouldNotBeNull();
            result.Id.ShouldBe("50e9bfc8-e242-428d-ba2b-3ae7c2d9863f");
            result.Premium.ShouldBe(false);
            result.Claimed.ShouldBe(true);
            result.Name.ShouldBe("daveaglick");
            result.CustomDomain.ShouldBe("daveaglick.com");
            result.NotificationEmail.ShouldBe(null);
            result.Url.ShouldBe("https://daveaglick.com");
            result.AdminUrl.ShouldBe("https://app.netlify.com/sites/daveaglick");
            result.ScreenshotUrl.ShouldBe("https://353a23c500dde3b2ad58-c49fe7e7355d384845270f4a7a0a7aa1.ssl.cf2.rackcdn.com/5a00dfb80b79b731343d0c65/screenshot.png");
            result.CreatedAt.ShouldBe(DateTime.Parse("2016-12-20T20:07:56.305Z", null, System.Globalization.DateTimeStyles.RoundtripKind));
            result.UpdatedAt.ShouldBe(DateTime.Parse("2017-11-06T22:19:20.128Z", null, System.Globalization.DateTimeStyles.RoundtripKind));
            result.UserId.ShouldBe("58543cf0c4d9cc4e6d4bf27a");
        }

        [Test]
        public void GetFormsUsesCorrectEndpoint()
        {
            // Given
            TestApiClient apiClient = new TestApiClient();
            NetlifyClient client = new NetlifyClient(apiClient);

            // When
            Form[] result = client.GetFormsAsync().Result;

            // Then
            apiClient.SendAndReadCalls.ShouldContain((HttpMethod.Get, "forms"));
        }

        [Test]
        public void GetFormsForSiteUsesCorrectEndpoint()
        {
            // Given
            TestApiClient apiClient = new TestApiClient();
            NetlifyClient client = new NetlifyClient(apiClient);

            // When
            Form[] result = client.GetFormsAsync("50e9bfc8-e242-428d-ba2b-3ae7c2d9863f").Result;

            // Then
            apiClient.SendAndReadCalls.ShouldContain((HttpMethod.Get, "sites/50e9bfc8-e242-428d-ba2b-3ae7c2d9863f/forms"));
        }
    }
}
