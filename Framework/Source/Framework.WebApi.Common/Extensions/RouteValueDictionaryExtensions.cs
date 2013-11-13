using System.Collections.Generic;
using System.Web.Routing;

namespace Framework.WebApi.Common.Extensions
{
    public static class RouteValueDictionaryExtensions
    {
        public static RouteValueDictionary Merge(this RouteValueDictionary dictionary, RouteValueDictionary dictionaryToMerge)
        {
            if(dictionaryToMerge == null)
                return dictionary;

            RouteValueDictionary newDictionary = new RouteValueDictionary(dictionary);
            foreach(KeyValuePair<string, object> item in dictionaryToMerge)
            {
                if(newDictionary.ContainsKey(item.Key))
                {
                    newDictionary[item.Key] = item.Value;
                }
                else
                {
                    newDictionary.Add(item.Key, item.Value);
                }
            }

            return newDictionary;
        }

        public static RouteValueDictionary Merge(this RouteValueDictionary dictionary, KeyValuePair<string, object> item)
        {
            RouteValueDictionary dictionaryToMerge = new RouteValueDictionary();
            dictionaryToMerge.Add(item.Key, item.Value);

            return dictionary.Merge(dictionaryToMerge);
        }

        public static RouteValueDictionary Merge(this RouteValueDictionary dictionary, KeyValuePair<string, object>[] items)
        {
            RouteValueDictionary dictionaryToMerge = new RouteValueDictionary();
            foreach(KeyValuePair<string, object> item in items)
            {
                dictionaryToMerge.Add(item.Key, item.Value);
            }

            return dictionary.Merge(dictionaryToMerge);
        }
    }
}