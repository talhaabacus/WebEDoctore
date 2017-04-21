using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebEdoc.Service
{
    /// <summary>
    /// Summary description for PHICService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PHICService : System.Web.Services.WebService
    {
        public struct PatientData
        {
            DataTable dt;
            bool isValid;
            string Error;
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
    
    }
}
