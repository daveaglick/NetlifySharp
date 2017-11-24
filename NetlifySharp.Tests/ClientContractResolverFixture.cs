using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetlifySharp.Tests
{
    [TestFixture]
    public class ClientContractResolverFixture
    {
        [TestCase("FooBarBaz", "foo_bar_baz")]
        [TestCase("foo_bar_baz", "foo_bar_baz")]
        [TestCase("Foo", "foo")]
        [TestCase("A", "a")]
        [TestCase("foo_barBaz", "foo_bar_baz")]
        [TestCase("Foo_Bar", "foo_bar")]
        public void ConvertsPascalToSnakeCase(string input, string expected)
        {
            // Given, When
            string actual = ClientContractResolver.ToSnakeCake(input);

            // Then
            actual.ShouldBe(expected);
        }
    }
}
