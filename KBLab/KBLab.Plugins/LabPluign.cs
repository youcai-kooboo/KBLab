using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kooboo.CMS.Toolkit.Plugins;

namespace KBLab.Plugins
{
    public class LabPluign : PluginBase
    {
        public override  ActionResult Execute()
        {
            ViewData["LabPluignText"] = "This is from the plugin";
            
            return null;
        }
    }
}
