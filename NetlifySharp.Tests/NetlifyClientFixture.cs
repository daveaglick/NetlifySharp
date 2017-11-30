using NetlifySharp.Models;
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
            VerifySite(result[0]);
            result[1].Id.ShouldBe("46b48455-90b4-4559-8233-6f6d08194696");
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
            VerifySite(result);
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

        private void VerifySite(Site site)
        {
            site.Id.ShouldBe("50e9bfc8-e242-428d-ba2b-3ae7c2d9863f");
            site.SiteId.ShouldBe("50e9bfc8-e242-428d-ba2b-3ae7c2d9863f");
            site.Plan.ShouldBe("nf_open_source");
            site.PlanData.Title.ShouldBe("Netlify Team Free");
            site.PlanData.AssetAcceleration.ShouldBe(true);
            site.PlanData.FormProcessing.ShouldBe(true);
            site.PlanData.CdnPropagation.ShouldBe("partial");
            site.PlanData.BuildGcExchange.ShouldBe("buildbot-gc");
            site.PlanData.BuildNodePool.ShouldBe("buildbot-ssd");
            site.PlanData.DomainAliases.ShouldBe(true);
            site.PlanData.SecureSite.ShouldBe(false);
            site.PlanData.Prerendering.ShouldBe(true);
            site.PlanData.Proxying.ShouldBe(true);
            site.PlanData.Ssl.ShouldBe("custom");
            site.PlanData.RateCents.ShouldBe(0);
            site.PlanData.YearlyRateCents.ShouldBe(0);
            site.PlanData.CdnNetwork.ShouldBe("free_cdn_network");
            site.PlanData.BranchDeploy.ShouldBe(true);
            site.PlanData.ManagedDns.ShouldBe(true);
            site.PlanData.GeoIp.ShouldBe(true);
            site.PlanData.SplitTesting.ShouldBe(true);
            site.PlanData.Id.ShouldBe("nf_team_dev");
            site.Premium.ShouldBe(false);
            site.Claimed.ShouldBe(true);
            site.Name.ShouldBe("daveaglick");
            site.CustomDomain.ShouldBe("daveaglick.com");
            site.NotificationEmail.ShouldBe(null);
            site.Url.ShouldBe("https://daveaglick.com");
            site.AdminUrl.ShouldBe("https://app.netlify.com/sites/daveaglick");
            site.ScreenshotUrl.ShouldBe("https://353a23c500dde3b2ad58-c49fe7e7355d384845270f4a7a0a7aa1.ssl.cf2.rackcdn.com/5a00dfb80b79b731343d0c65/screenshot.png");
            site.CreatedAt.ShouldBe(DateTime.Parse("2016-12-20T20:07:56.305Z", null, System.Globalization.DateTimeStyles.RoundtripKind));
            site.UpdatedAt.ShouldBe(DateTime.Parse("2017-11-06T22:19:20.128Z", null, System.Globalization.DateTimeStyles.RoundtripKind));
            site.UserId.ShouldBe("58543cf0c4d9cc4e6d4bf27a");
        }
    }
}
