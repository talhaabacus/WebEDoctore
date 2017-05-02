using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebEdoc2017.DataLayer;

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
             OracleDataAccess.OracleCommandData _data = new OracleDataAccess.OracleCommandData();
            try
            {

                _data._CommandType = CommandType.Text;
                _data.CommandText = "SELECT * FROM PatientTable";
                _data.OpenWithOutTrans();

                //Executing Query
                DataSet _ds = _data.Execute(OracleDataAccess.ExecutionType.ExecuteDataSet) as DataSet;

                data.dt = _ds.Tables[0];
                data.isValid = true;

                //string sQuery = "SELECT * FROM PatientTable ";
                // data.dt= new DBAction().ExecuteDataSetInline(sQuery).Tables[0];
            }
            catch (Exception ex)
            {
                data.Error = ex.Message.ToString();
                //throw;
            }
            finally
            {
                _data.Close();
            }

            return data;

        }

        #region Patient Health Information Capture....


        [WebMethod]
        public PatientData GetDocumentCategory()
        {
            PatientData data = new PatientData();
            OracleDataAccess.OracleCommandData _data = new OracleDataAccess.OracleCommandData();
            try
            {

                _data._CommandType = CommandType.Text;
                _data.CommandText = "SELECT * FROM DOC_Category";
                _data.OpenWithOutTrans();

                //Executing Query
                DataSet _ds = _data.Execute(OracleDataAccess.ExecutionType.ExecuteDataSet) as DataSet;

                data.dt = _ds.Tables[0];
                data.isValid = true;

                //string sQuery = "SELECT * FROM PatientTable ";
                // data.dt= new DBAction().ExecuteDataSetInline(sQuery).Tables[0];
            }
            catch (Exception ex)
            {
                data.Error = ex.Message.ToString();
                //throw;
            }
            finally
            {
                _data.Close();
            }

            return data;
        }

        #endregion
    }
}
