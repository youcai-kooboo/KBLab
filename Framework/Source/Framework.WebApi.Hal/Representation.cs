using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Xml.Linq;
using System.Xml.XPath;
using Framework.WebApi.Common;
using Framework.WebApi.Common.Extensions;

namespace Framework.WebApi.Hal
{
    //[DataContract]
    public abstract class Representation : IRepresentation
    {
        private const string REPRESENTATIONS_CONFIG_FILE_PATH = "App_Data/Representations.config";
        private const string OPTIONAL_PROMPT = "(optional)";
        private const string DOMAIN = "{domain}";
        private const string DUPLICATE_SLASH_EXPRESSION = @"(?<!:)//+";
        
        protected Representation()
        {
            InitRepresentation();
        }
 
        public Dictionary<string, object> Hints { get; private set; }
 
        public virtual List<Link> Links { get; private set; }

        [IgnoreDataMember]
        public virtual Dictionary<string, string> Prompts { get; private set; }

        #region AddLink

        public Link AddLink(string rel, Uri uri)
        {
            Link link = new Link(rel, uri.ToString());
            Links.Add(link);
            return link;
        }

        public Link AddLink<TApiController, TModel>(string rel, ApiType apiType, ApiContext context, bool includeCustomerId = true)
            where TApiController : ApiController
            where TModel : class
        {
            return AddLink<TApiController, TModel>(rel, apiType, String.Empty, context, includeCustomerId);
        }

        public Link AddLink<TApiController, TModel>(string rel, ApiType apiType, string path, ApiContext context, bool includeCustomerId = true, ApiVersion version = ApiVersion.V1)
            where TApiController : ApiController
            where TModel : class
        {
            string href = GenerateLink<TApiController>(apiType, path, context, includeCustomerId, version);
            Link link = new Link(rel, href);
            link.Data = new List<LinkQueryParameter>();

            PropertyInfo[] properties = typeof(TModel).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object[] attributes = property.GetCustomAttributes(false);

                // Name
                string name = property.Name.ToLower();
                LinkQueryParameter dataItem = new LinkQueryParameter(name);

                // Value
                DefaultValueAttribute defaultValueAttribute =
                    attributes.OfType<DefaultValueAttribute>().FirstOrDefault();
                if (defaultValueAttribute != null)
                {
                    dataItem.Value = defaultValueAttribute.Value.ToString();
                }

                // Prompt
                DescriptionAttribute descriptionAttribute =
                    attributes.OfType<DescriptionAttribute>().FirstOrDefault();
                if (descriptionAttribute != null)
                {
                    dataItem.Prompt = descriptionAttribute.Description;
                }

                DataMemberAttribute dataMemberAttribute = attributes.OfType<DataMemberAttribute>().FirstOrDefault();
                bool isRequired = dataMemberAttribute != null && dataMemberAttribute.IsRequired;
                if (!isRequired)
                {
                    if (String.IsNullOrEmpty(dataItem.Prompt))
                    {
                        dataItem.Prompt = OPTIONAL_PROMPT;
                    }
                    else
                    {
                        dataItem.Prompt += " " + OPTIONAL_PROMPT;
                    }
                }

                link.Data.Add(dataItem);
            }

            Links.Add(link);
            return link;
        }

        public Link AddLink<TApiController>(string rel, ApiType apiType, ApiContext context, bool includeCustomerId = true, ApiVersion version = ApiVersion.V1)
            where TApiController : ApiController
        {
            return AddLink<TApiController>(rel, apiType, String.Empty, context, includeCustomerId, version);
        }

        public Link AddLink<TApiController>(string rel, ApiType apiType, string path, ApiContext context, bool includeCustomerId = true, ApiVersion version = ApiVersion.V1)
            where TApiController : ApiController
        {
            string href = GenerateLink<TApiController>(apiType, path, context, includeCustomerId, version);
            Link link = new Link(rel, href);
            Links.Add(link);
            return link;
        }

        public Link AddLink(string rel, ApiType apiType, ApiContext context, bool includeCustomerId = true, ApiVersion version = ApiVersion.V1)
        {
            return AddLink(rel, apiType, String.Empty, context, includeCustomerId, version);
        }

        public Link AddLink(string rel, ApiType apiType, string path, ApiContext context, bool includeCustomerId = true, ApiVersion version = ApiVersion.V1)
        {
            string href = GenerateLink(apiType, path, context, includeCustomerId, version);
            Link link = new Link(rel, href);
            Links.Add(link);
            return link;
        }

        public static string GenerateLink<TApiController>(ApiType apiType, ApiContext context, bool includeCustomerId)
            where TApiController : ApiController
        {
            return GenerateLink<TApiController>(apiType, String.Empty, context, includeCustomerId);
        }

        public static string GenerateLink<TApiController>(ApiType apiType, ApiContext context, bool includeCustomerId, ApiVersion version = ApiVersion.V1)
            where TApiController : ApiController
        {
            return GenerateLink<TApiController>(apiType, String.Empty, context, includeCustomerId, version);
        }

        public static string GenerateLink<TApiController>(ApiType apiType, string path, ApiContext context, bool includeCustomerId)
            where TApiController : ApiController
        {
            return GenerateLink(apiType, typeof(TApiController), path, context, includeCustomerId);
        }

        public static string GenerateLink<TApiController>(ApiType apiType, string path, ApiContext context, bool includeCustomerId, ApiVersion version = ApiVersion.V1)
            where TApiController : ApiController
        {
            return GenerateLink(apiType, typeof(TApiController), path, context, includeCustomerId, version);
        }

        public static string GenerateLink(ApiType apiType, Type controllerType, ApiContext context, bool includeCustomerId)
        {
            return GenerateLink(apiType, controllerType, String.Empty, context, includeCustomerId);
        }

        public static string GenerateLink(ApiType apiType, Type controllerType, string path, ApiContext context, bool includeCustomerId, ApiVersion version = ApiVersion.V1)
        {
            string apiControllerName = controllerType.Name.Replace("Controller", String.Empty).ToLower();
            string apiControllerVersion = version == ApiVersion.V1 ? string.Empty : version.ToString().ToLower();

            path = String.Concat(apiControllerName, apiControllerVersion).CombineVirtualPath(path);
            return GenerateLink(apiType, path, context, includeCustomerId);
        }

        public static string GenerateLink(ApiType apiType, ApiContext context, bool includeCustomerId)
        {
            return GenerateLink(apiType, String.Empty, context, includeCustomerId);
        }

        public static string GenerateLink(ApiType apiType, string path, ApiContext context, bool includeCustomerId, ApiVersion version = ApiVersion.V1)
        {
            string apiPath = ConfigurationManager.AppSettings[apiType.ToString()];
            string domain = String.Empty;
            //string channelFromRoute = String.Empty;
            //string subChannelFromRoute = String.Empty;
            //string languageFromRoute = String.Empty;
            //string customerIdFromRoute = String.Empty;
            //string versionFromRoute = version == ApiVersion.V1 ? string.Empty : version.ToString().ToLower();

            if (!String.IsNullOrEmpty(apiPath))
            {
                if (context != null)
                {
                    domain = context.Domain;

                    //if (!String.IsNullOrEmpty(context.ChannelCode))
                    //{
                    //    channelFromRoute = ChannelConstraint.ConvertChannelToRouteValue(context.ChannelCode);
                    //}

                    //if (!String.IsNullOrEmpty(context.SubChannelCode))
                    //{
                    //    subChannelFromRoute = SubChannelConstraint.ConvertSubChannelToRouteValue(context.SubChannelCode);
                    //}

                    //if (includeCustomerId && !String.IsNullOrEmpty(context.SessionId))
                    //{
                    //    customerIdFromRoute = CustomerIdConstraint.ConvertCustomerIdToRouteValue(context.CustomerId);
                    //}

                    //if (!context.IsDefaultLanguage())
                    //{
                    //    languageFromRoute = LanguageConstraint.ConvertLanguageToRouteValue(context.Language.LanguageCode);
                    //}
                }

                apiPath = apiPath.Replace(DOMAIN, domain);
                //apiPath = apiPath.Replace(CHANNEL, channelFromRoute);
                //apiPath = apiPath.Replace(SUBCHANNEL, subChannelFromRoute);
                //apiPath = apiPath.Replace(LANGUAGE, languageFromRoute);
                //apiPath = apiPath.Replace(CUSTOMERID, customerIdFromRoute);
                //apiPath = apiPath.Replace(VERSION, versionFromRoute);

                apiPath = Regex.Replace(apiPath, DUPLICATE_SLASH_EXPRESSION, "/");
                if (apiPath != "/")
                {
                    apiPath = apiPath.TrimEnd('/');
                }
            }

            return context.Request.ToAbsoluteUrl(apiPath.CombineVirtualPath(path));
        }

        #endregion

        #region InitRepresentation

        [ExcludeFromCodeCoverage] // Unable to test as we cannot test filesystem
        protected void InitRepresentation()
        {
            Links = new List<Link>();
            Hints = new Dictionary<string, object>();

            string representationsConfigFilePath = AppDomain.CurrentDomain.BaseDirectory+REPRESENTATIONS_CONFIG_FILE_PATH;
            if (File.Exists(representationsConfigFilePath))
            {
                XDocument representationsDocument = XDocument.Load(representationsConfigFilePath);
                InitRepresentation(representationsDocument);
            }
        }

        internal void InitRepresentation(XDocument representationsDocument)
        {
            if (representationsDocument != null)
            {
                string typeFullName = this.GetType().FullName;
                XElement representationElement =
                    representationsDocument.Root.XPathSelectElement("representation[@type='" + typeFullName + "']");
                if (representationElement != null)
                {
                    InitHints(representationElement);
                }
            }
        }

        internal void InitHints(XElement representationElement)
        {
            XElement hintsElement = representationElement.Element("hints");
            if (hintsElement != null)
            {
                IEnumerable<XElement> hintElements = hintsElement.Elements("hint");
                foreach (XElement hintElement in hintElements)
                {
                    XElement keyElement = hintElement.Element("key");
                    XElement valueElement = hintElement.Element("value");
                    if (keyElement != null && valueElement != null)
                    {
                        Hints.Add(keyElement.Value, valueElement.Value);
                    }
                }
            }
        }

        #endregion
    }
}