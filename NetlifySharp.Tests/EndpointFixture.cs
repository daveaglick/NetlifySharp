using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetlifySharp.Tests
{
    [TestFixture]
    public class EndpointFixture
    {
        [Test]
        public void ShouldThrowForNullIdWithParamName()
        {
            // Given
            Endpoint endpoint = new Endpoint("foo");

            // When, Then
            Should.Throw<ArgumentException>(() => endpoint.Append(null, "bar")).Message.ShouldContain("bar");
        }

        [Test]
        public void ShouldThrowForEmptyIdWithParamName()
        {
            // Given
            Endpoint endpoint = new Endpoint("foo");

            // When, Then
            Should.Throw<ArgumentException>(() => endpoint.Append(string.Empty, "bar")).Message.ShouldContain("bar");
        }

        [Test]
        public void ShouldAppendIdWithSlashDelimiter()
        {
            // Given
            Endpoint foo = new Endpoint("foo");

            // When
            Endpoint foobar = foo.Append("12345", "bar");

            // Then
            foobar.ToString().ShouldBe("foo/12345");
        }

        [Test]
        public void ShouldAppendEndpointsWithSlashDelimiter()
        {
            // Given
            Endpoint foo = new Endpoint("foo");
            Endpoint bar = new Endpoint("bar");

            // When
            Endpoint foobar = foo.Append(bar);

            // Then
            foobar.ToString().ShouldBe("foo/bar");
        }
    }
}
