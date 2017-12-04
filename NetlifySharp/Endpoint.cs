using System;

namespace NetlifySharp
{
    public class Endpoint
    {
        private readonly string _endpoint;

        public Endpoint(string endpoint)
        {
            _endpoint = endpoint;
        }

        public Endpoint Append(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                return this;
            }
            return new Endpoint($"{_endpoint}/{param}");
        }

        public Endpoint Append(Endpoint endpoint) => new Endpoint($"{_endpoint}/{endpoint._endpoint}");

        public static implicit operator string(Endpoint endpoint) => endpoint._endpoint;
        
        public static implicit operator Endpoint(string endpoint) => new Endpoint(endpoint);

        public override string ToString() => _endpoint;

        public override bool Equals(object obj) => (obj as Endpoint)?._endpoint.Equals(_endpoint) ?? false;

        public override int GetHashCode() => _endpoint?.GetHashCode() ?? 0;
    }
}
