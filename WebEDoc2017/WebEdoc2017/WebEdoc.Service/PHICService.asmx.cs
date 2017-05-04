using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using WebEdoc2017.DataLayer;

namespace WebEdoc.Service
{
    /// <summary>
    /// Summary description for PHICService
    /// </summary>
    [WebService(Namespace = "PHICService123")]
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

        public struct paraDocumentCategory
        {
            public Int64 DOC_CATEGORY_ID;
            public string Name;
            public string Description;
            public Int64 Parent_ID;
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

        [WebMethod]
        public int SaveDocumentCategory(paraDocumentCategory _par,string Type)
        {
            int returnValue = 0;
            string _querystring = string.Empty;
            if(Type=="EDIT")
            {
                _querystring = "Update DOC_CATEGORY SET NAME='" + _par.Name.ToString().Trim() + "',Description='" + _par.Description.ToString() + "' WHERE DOC_CATEGORY_ID=" + _par.DOC_CATEGORY_ID + " AND PARENT_ID=" + _par.Parent_ID + "";
            }
            else if(Type=="SIBLING")
            {
              //  _data.CommandText = "Update DOC_CATEGORY SET NAME='" + _par.Name.ToString().Trim() + "',Description='" + _par.Description.ToString() + "' WHERE DOC_CATEGORY_ID=" + _par.DOC_CATEGORY_ID + " AND PARENT_ID=" + _par.Parent_ID + "";
                _querystring = "INSERT INTO DOC_CATEGORY (DOC_CATEGORY_ID,NAME,DESCRIPTION,PARENT_ID) VALUES(DOC_CATEGORYSEQ.NEXTVAL,'" + _par.Name + "','" + _par.Description + "'," + _par.Parent_ID + ")";


            }
            else if (Type == "SUBCATEGORY")
            {
                _querystring = "INSERT INTO DOC_CATEGORY (DOC_CATEGORY_ID,NAME,DESCRIPTION,PARENT_ID) VALUES(DOC_CATEGORYSEQ.NEXTVAL,'" + _par.Name + "','" + _par.Description + "'," + _par.DOC_CATEGORY_ID + ")";

            }
            OracleDataAccess.OracleCommandData _data = new OracleDataAccess.OracleCommandData();
            try
            {

              
                _data._CommandType = CommandType.Text;
                _data.CommandText = _querystring;
                _data.OpenWithOutTrans();

                //Executing Query
                object obj = _data.Execute(OracleDataAccess.ExecutionType.ExecuteNonQuery);
                returnValue = Convert.ToInt16(obj);

             //   data.dt = _ds.Tables[0];
             //   data.isValid = true;

                //string sQuery = "SELECT * FROM PatientTable ";
                // data.dt= new DBAction().ExecuteDataSetInline(sQuery).Tables[0];
            }
            catch (Exception ex)
            {
                //data.Error = ex.Message.ToString();
                //throw;
            }
            finally
            {
                _data.Close();
            }
            return returnValue;
        }


        [WebMethod]
        public int DeleteDocumentCategoryByCategoryID(Int64 DocumentCategoryID)
        {
            int returnValue = 0;
            OracleDataAccess.OracleCommandData _data = new OracleDataAccess.OracleCommandData();
            try
            {


                _data._CommandType = CommandType.Text;
                _data.CommandText = "DELETE FROM DOC_CATEGORY WHERE DOC_CATEGORY_ID="+DocumentCategoryID+"";
                _data.OpenWithOutTrans();

                //Executing Query
                object obj = _data.Execute(OracleDataAccess.ExecutionType.ExecuteNonQuery);
                returnValue = Convert.ToInt16(obj);

                //   data.dt = _ds.Tables[0];
                //   data.isValid = true;

                //string sQuery = "SELECT * FROM PatientTable ";
                // data.dt= new DBAction().ExecuteDataSetInline(sQuery).Tables[0];
            }
            catch (Exception ex)
            {
                //data.Error = ex.Message.ToString();
                //throw;
            }
            finally
            {
                _data.Close();
            }

            return returnValue;
        }




        [WebMethod]
        public PatientData GetPatientDocumentByCategoryID(Int64 CategoryID)
        {

            PatientData data = new PatientData();
            OracleDataAccess.OracleCommandData _data = new OracleDataAccess.OracleCommandData();
            try
            {

                _data._CommandType = CommandType.Text;
                _data.CommandText = "SELECT * FROM PATIENT_DOCUMENT where trim(DOC_CATEGORY_ID)="+CategoryID+"";
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
