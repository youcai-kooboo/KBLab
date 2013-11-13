using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WebApi.Hal
{
    [NonEmbedded]
    public class LinkParameter
    {
        public LinkParameter(string name)
            : this(name, null, null)
        {
        }

        public LinkParameter(string name, string prompt)
            : this(name, prompt, null)
        {
        }

        public LinkParameter(string name, string prompt, object defaultValue)
        {
            Name = name;
            Prompt = prompt;
            DefaultValue = defaultValue;
        }

        public string Name { get; private set; }

        public string Prompt { get; private set; }

        public object DefaultValue { get; private set; }
    }
}