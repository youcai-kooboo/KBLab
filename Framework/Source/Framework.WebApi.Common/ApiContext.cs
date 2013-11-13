using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.WebApi.Common
{
    public class ApiContext : IDisposable
    {
        #region RunningApplication enum

        /// <summary>
        /// Enumeration that indicates which applicationtype is executing the code. 
        /// </summary>
        public enum RunningApplication
        {
            Unknown,
            Api
        }

        #endregion
         /// <summary>
        /// private constructor so we cannot instantiate a new one
        /// </summary>
        private ApiContext()
        {
        }

        
        /// <summary>
        /// Static constructor.
        /// </summary>
        static ApiContext()
        {
            apiContextSlot = Thread.AllocateNamedDataSlot("ApiContext");
        }

        /// <summary>
        /// currentRunningApplication
        /// </summary>
        private RunningApplication currentRunningApplication = RunningApplication.Unknown;
        /// <summary>
        /// Holds the current running application that is using the context
        /// </summary>
        public RunningApplication CurrentRunningApplication
        {
            get { return currentRunningApplication; }
            set { currentRunningApplication = value; }
        }

        private static LocalDataStoreSlot apiContextSlot;

        public static ApiContext Current
        {
            get
            {
                var context = (ApiContext)Thread.GetData(apiContextSlot);
                //TODO:Needs to remove this null check for better implementation of the Current in the code.
                //We are not updating but assinging a new session.
                if (context == null)
                {
                    // If no ApiContext is present, just create one.
                    context = new ApiContext();
                    Thread.SetData(apiContextSlot, context);
                }
                return context;
            }
            set
            {
                apiContextSlot = Thread.GetNamedDataSlot("ApiContext");
                Thread.SetData(apiContextSlot, value);
            }
        }

        public HttpRequestMessage Request { get; set; }

        private string domain;
        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        public ApiContext Clone()
        {
            var clonedContext = new ApiContext();
            clonedContext.Domain = Domain;

            return clonedContext;
        }

        public void Dispose()
        {
            Current = null;
        }
    }
}
