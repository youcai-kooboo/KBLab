using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Framework.WebApi.Common.Extensions
{
    public static class UriExtensions
    {
        public static Uri ToHttps(this Uri uri)
        {
            UriBuilder uriBuilder = new UriBuilder(uri);
            uriBuilder.Scheme = Uri.UriSchemeHttps;
            uriBuilder.Port = 443;
            return uriBuilder.Uri;
        }

        /// <summary>
        /// Given the <see cref="Uri"/> Host string property, retrives the higest
        /// subdomain name out. Eg. givent the domain http://m.mydomain.domain.com/whatever
        /// the method will return the string "m".
        /// </summary>
        /// <param name="uri">A valid instance of <see cref="Uri"/> class.</param>
        /// <returns>Returns the string rapresenting the name of the highest subdomain.</returns>
        public static string GetHighestSubdomain(this Uri uri)
        {
            if (uri == null)
            {
                return null;
            }

            string highestSubdomain = null;

            Match match = Regex.Match(uri.Host, @"(?:http[s]*\:\/\/)*(.*?)\.(?=[^\/]*\..{2,5})",
                                            RegexOptions.IgnoreCase);

            if (match.Success)
            {
                highestSubdomain = match.Groups[1].Value;
            }

            return highestSubdomain;
        }

        public static string GetDomain(this Uri url)
        {
            var dotBits = url.Host.Split('.');
            if (dotBits.Length == 1) return url.Host; //eg http://localhost/blah.php = "localhost"
            if (dotBits.Length == 2) return url.Host; //eg http://blah.co/blah.php = "localhost"

            var domain = GetDomainUsingTLDs(url.Host);

            if (String.IsNullOrEmpty(domain) && !TldPatterns.EXCLUDED.Contains(url.Host))
            {
                domain = GetDomainUsingMessyTLDs(url.Host);
            }

            return String.IsNullOrEmpty(domain) ? url.Host : domain;
        }

        private static string GetDomainUsingMessyTLDs(string host)
        {
            var tlds = TldPatterns.EXCLUDED;
            var hostNameToCheck = host.Split('.').Skip(1).Aggregate((acc, item) => acc + "." + item);

            string bestMatch = "";
            foreach (var tld in tlds)
            {
                if (hostNameToCheck.EndsWith(tld, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (tld.Length > bestMatch.Length) bestMatch = tld;
                }
            }

            if (string.IsNullOrEmpty(bestMatch))
            {
                return bestMatch;
            }

            //add the domain name onto tld
            string[] bestBits = bestMatch.Split('.');
            string[] inputBits = host.Split('.');
            int getLastBits = bestBits.Length + 1;
            bestMatch = "";
            for (int c = inputBits.Length - getLastBits; c < inputBits.Length; c++)
            {
                if (bestMatch.Length > 0) bestMatch += ".";
                bestMatch += inputBits[c];
            }
            return bestMatch;
        }

        private static String GetDomainUsingTLDs(String host)
        {
            var tlds = TldPatterns.EXACT;

            string bestMatch = "";
            foreach (var tld in tlds)
            {
                if (host.EndsWith(tld, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (tld.Length > bestMatch.Length) bestMatch = tld;
                }
            }
            if (!string.IsNullOrEmpty(bestMatch))
            {
                //add the domain name onto tld
                string[] bestBits = bestMatch.Split('.');
                string[] inputBits = host.Split('.');
                int getLastBits = bestBits.Length + 1;
                bestMatch = "";
                for (int c = inputBits.Length - getLastBits; c < inputBits.Length; c++)
                {
                    if (bestMatch.Length > 0) bestMatch += ".";
                    bestMatch += inputBits[c];
                }
            }
            return bestMatch;
        }

        // This database was taken from http://guava-libraries.googlecode.com/svn-history/r42/trunk/src/com/google/common/net/TldPatterns.java
        // With many thanks to all involved
        /**
         * A generated static class containing public members which provide domain
         * name patterns used in determining whether a given domain name is an
         * effective top-level domain (TLD).
         */
        private static class TldPatterns
        {
            /**
             * If a hostname is contained in this set, it is a TLD.
             */
            #region EXACT

            public static List<string> EXACT = new List<string>
                {

                };
            #endregion

   

            /**
             * The elements in this set would pass the UNDER test, but are
             * known not to be TLDs and are thus excluded from consideration.
             */
            #region EXCLUDED

            public static List<string> EXCLUDED = new List<string>
                {

                };

            #endregion
        }
    }
}