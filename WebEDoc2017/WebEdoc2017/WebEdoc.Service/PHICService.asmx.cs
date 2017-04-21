using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using OracleDataLib.Actions;
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
           public DataTable dt;
           public bool isValid;
           public string Error;
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
         [WebMethod]
        public PatientData GetPatientRecords()
        {
            PatientData data = new PatientData();
         
            try
            {
                string sQuery = "SELECT ETHNICITY, ETHNICITY_CODE FROM PATIENT_ETH ORDER BY ETHNICITY ASC";
                data.dt= new DBAction().ExecuteDataSetInline(sQuery).Tables[0];
            }
            catch (Exception ex)
            {
                data.Error = ex.Message.ToString();  
                //throw;
            }
            finally
            {

            }
            
            return data;

        }
    }
}
