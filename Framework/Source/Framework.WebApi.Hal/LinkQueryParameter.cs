namespace Framework.WebApi.Hal
{
    public class LinkQueryParameter
    {
        public LinkQueryParameter(string name)
        {
            Name = name;
        }

        public LinkQueryParameter(string name, string prompt)
        {
            Name = name;
            Prompt = prompt;
        }

        public string Name { get; private set; }

        public string Prompt { get; set; }

        public string Value { get; set; }
    }
}