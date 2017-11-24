using System;

namespace NetlifySharp
{
    internal class Endpoint
    {
        private readonly string _endpoint;

        public Endpoint(string endpoint)
        {
            _endpoint = endpoint;
        }

        public Endpoint AppendId(string id, string paramName)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("The ID cannot be null or empty", paramName);
            }
            return new Endpoint($"{_endpoint}/{id}");
        }

        public Endpoint Append(Endpoint endpoint) => new Endpoint($"{_endpoint}/{endpoint._endpoint}");

        public override string ToString() => _endpoint;
    }
}
