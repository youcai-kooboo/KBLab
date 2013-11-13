using System.Collections.Generic;
using System.Net.Http;

namespace Framework.WebApi.Hal
{
    public class Link
    {
        private List<LinkQueryParameter> data;

        public Link(string rel, string href)
        {
            Rel = rel;
            Href = href;
        }

        public string Rel { get; private set; }

        public string Href { get; private set; }

        public HttpMethod method = HttpMethod.Get;

        public HttpMethod Method
        {
            get { return this.method; }
            set { this.method = value; }
        }

        public bool Templated { get; private set; }

        public List<LinkQueryParameter> Data
        {
            get { return data; }
            set
            {
                Templated = true;
                data = value;
            }
        }
    }
}