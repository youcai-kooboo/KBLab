using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.Website.Extensions
{
    public static class ExtensionMethods
    {
        #region General 
        public static String LimitLength(this String text, int length)
        {
            return (text != null && text != string.Empty && (text.Length > length)) ? text.Substring(0, length) + ".." : text;
        }
        #endregion
        
        #region Html helper
        #endregion
    }
}