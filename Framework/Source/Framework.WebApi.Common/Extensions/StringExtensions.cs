using System;
using System.Web;

namespace Framework.WebApi.Common.Extensions
{
    /// <summary>
    /// String extensions motheods' class
    /// Adding some common and convient extension methods
    /// </summary>
    public static class StringExtensions
    {
        public static string CombineVirtualPath(this string source, string virtualPath)
        {
            if(!String.IsNullOrEmpty(virtualPath))
            {
                if(!String.IsNullOrEmpty(source))
                {
                    if(virtualPath.StartsWith("?"))
                    {
                        return String.Format("{0}{1}", source.TrimEnd('/'), virtualPath);
                    }
                    else
                    {
                        return String.Format("{0}/{1}", source.TrimEnd('/'), virtualPath.TrimStart('/'));
                    }
                }
                else
                {
                    return virtualPath;
                }
            }
            return source;
        }

        public static string AddQuery(this string source, string key, string value)
        {
            string delim;
            if((source == null) || !source.Contains("?"))
            {
                delim = "?";
            }
            else if(source.EndsWith("?") || source.EndsWith("&"))
            {
                delim = String.Empty;
            }
            else
            {
                delim = "&";
            }

            return source + delim + HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(value);
        }

        public static string TrimStart(this string source, string trimString, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if(!String.IsNullOrEmpty(source))
            {
                if(source.StartsWith(trimString, comparisonType))
                {
                    return source.Substring(trimString.Length);
                }
            }

            return source;
        }

        public static string AsString(this object source)
        {
            if (source != null)
            {
                return source.ToString();
            }
            return String.Empty;
        }

        public static bool IsGuid(this string source)
        {
            if(!String.IsNullOrEmpty(source))
            {
                Guid guidValue;
                return Guid.TryParse(source, out guidValue) && (guidValue != Guid.Empty);
            }
            return false;
        }

        public static string GetDomain(this string url)
        {
            Uri uri = new Uri(url);
            return uri.GetDomain();
        }

        public static string[] SplitRemoveEmptyEntries(this string source, char separator)
        {
            return SplitRemoveEmptyEntries(source, new char[] { separator });
        }

        public static string[] SplitRemoveEmptyEntries(this string source, char[] separator)
        {
            return source.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string Last(this string source, int length)
        {
            if (length >= source.Length)
                return source;

            return source.Substring(source.Length - length);
        }
    }
}