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
    [WebService(Namespace = "PHICService")]
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


        public struct paraPatientDocument
        {
            public Int64 PatientDocumentID;
            public string Name;
            public string Title;
            public string Description;

            public Int64 CategoryID;

            public string ElectronicLink;
            public string Path;
            public string PatientID;
            public string Extension;
            public byte[] attachement;
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

      

        #region Get Document Category....
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

        #region Save Document Category...
        [WebMethod]
        public int SaveDocumentCategory(paraDocumentCategory _par, string Type)
        {
            int returnValue = 0;
            string _querystring = string.Empty;
            if (Type == "EDIT")
            {
                _querystring = "Update DOC_CATEGORY SET NAME='" + _par.Name.ToString().Trim() + "',Description='" + _par.Description.ToString() + "' WHERE DOC_CATEGORY_ID=" + _par.DOC_CATEGORY_ID + " AND PARENT_ID=" + _par.Parent_ID + "";
            }
            else if (Type == "SIBLING")
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

        #endregion

        #region Delete Document Category...

        [WebMethod]
        public int DeleteDocumentCategoryByCategoryID(Int64 DocumentCategoryID)
        {
            int returnValue = 0;
            OracleDataAccess.OracleCommandData _data = new OracleDataAccess.OracleCommandData();
            try
            {


                _data._CommandType = CommandType.Text;
                _data.CommandText = "DELETE FROM DOC_CATEGORY WHERE DOC_CATEGORY_ID=" + DocumentCategoryID + "";
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


        #endregion

        #region Check Deleting Category Is Used in Patient Document OR NOT

        [WebMethod]
        public PatientData GetCategoryRecordFromPatientDocument(Int64 Doc_Category_ID)
        {
            PatientData data = new PatientData();
            OracleDataAccess.OracleCommandData _data = new OracleDataAccess.OracleCommandData();
            try
            {

                _data._CommandType = CommandType.Text;
                _data.CommandText = "Select * from Patient_Document where Doc_Category_ID in( Select Doc_Category_ID from DOC_Category where Doc_Category_ID ="+ Doc_Category_ID + " OR Parent_ID = "+ Doc_Category_ID + ")";
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

        #region Get Patient Document by Category

        [WebMethod]
        public PatientData GetPatientDocumentByCategoryID(Int64 CategoryID,string PatientID)
        {

            PatientData data = new PatientData();
            OracleDataAccess.OracleCommandData _data = new OracleDataAccess.OracleCommandData();
            try
            {

                _data._CommandType = CommandType.Text;
                _data.CommandText = "SELECT * FROM PATIENT_DOCUMENT where trim(DOC_CATEGORY_ID)=" + CategoryID + " AND trim(Patient_ID)='"+PatientID+"'";
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

        #region Save Patient Document.....

        [WebMethod]
        public int AddPatientDocument(Int64 CategoryID, paraPatientDocument para)
        {

            int returnValue = 0;
            OracleDataAccess.OracleCommandData _data = new OracleDataAccess.OracleCommandData();

            try
            {

                
                _data._CommandType = CommandType.Text;
                _data.CommandText = "INSERT INTO PATIENT_DOCUMENT (PATIENT_DOCUMENT_ID,NAME,TITLE,DESCRIPTION,DOC_CATEGORY_ID,ELECTRONIC_LINK,EXTENSION,PATH,PATIENT_ID)" +
                                                    " values(PATIENT_DOCUMENT_SEQ.NEXTVAL,'" + para.Name + "','" + para.Title + "','" + para.Description + "'," + para.CategoryID + ",'" + para.ElectronicLink + "','" + para.Extension + "','" + para.Path + "','" + para.PatientID + "')";
                _data.OpenWithOutTrans();

                //Executing Query
                object obj = _data.Execute(OracleDataAccess.ExecutionType.ExecuteNonQuery);
                returnValue = Convert.ToInt16(obj);

                if(returnValue >0)
                {
                    string FileType = para.Path.ToString();
                    int posn = FileType.IndexOf(".");
                    if (posn > 0)
                        FileType = FileType.Substring(posn + 1, FileType.Length - posn - 1);
                    else
                        FileType = "";
                    string FileName = para.Title.Trim() + "." + FileType;
                    
                    FileHelper.CheckOrCreateDirectory("C:\\inetpub\\wwwroot\\Upload\\PHICDocument");
                    //FileHelper.BytesToDisk(para.attachement, "C:\\inetpub\\wwwroot\\Upload" + "\\" + FileName);
                    FileHelper.BytesToDisk(para.attachement, para.Path.ToString());

                }


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


        #endregion

        #region Update Patient Document...
        [WebMethod]
        public int UpdatePatientDocument(Int64 CategoryID, paraPatientDocument para)
        {
            int returnValue = 0;
            OracleDataAccess.OracleCommandData _data = new OracleDataAccess.OracleCommandData();
            try
            {
                _data._CommandType = CommandType.Text;

                _data.CommandText = "UPDATE PATIENT_DOCUMENT SET  NAME='" + para.Name + "',TITLE='" + para.Title + "',DESCRIPTION='" + para.Description + "',DOC_CATEGORY_ID=" + para.CategoryID + ",ELECTRONIC_LINK='" + para.ElectronicLink + "',PATIENT_ID='" + para.PatientID + "' WHERE PATIENT_DOCUMENT_ID=" + para.PatientDocumentID + " AND DOC_CATEGORY_ID=" + CategoryID + " ";



                // _data.CommandText = "  ";
                _data.OpenWithOutTrans();

                //Executing Query
                object obj = _data.Execute(OracleDataAccess.ExecutionType.ExecuteNonQuery);
                returnValue = Convert.ToInt16(obj);
                //if (returnValue > 0)
                //{
                //    string FileType = para.Path.ToString();
                //    int posn = FileType.IndexOf(".");
                //    if (posn > 0)
                //        FileType = FileType.Substring(posn + 1, FileType.Length - posn - 1);
                //    else
                //        FileType = "";
                //    string FileName = para.Title.Trim() + "." + FileType;

                //    FileHelper.CheckOrCreateDirectory("C:\\inetpub\\wwwroot\\Upload");
                //    //   FileHelper.BytesToDisk(para.attachement, "C:\\inetpub\\wwwroot\\Upload" + "\\" + FileName);
                //    FileHelper.BytesToDisk(para.attachement, para.Path.ToString());

                //}



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

        #endregion

        #region Delete Patient Document
        [WebMethod]
        public int DeletePatientDocument(Int64 PatientDocumentID, Int64 CategoryID)
        {
            int returnValue = 0;
            OracleDataAccess.OracleCommandData _data = new OracleDataAccess.OracleCommandData();
            try
            {
                _data._CommandType = CommandType.Text;

                _data.CommandText = "DELETE FROM PATIENT_DOCUMENT WHERE PATIENT_DOCUMENT_ID=" + PatientDocumentID + " AND DOC_CATEGORY_ID=" + CategoryID + " ";



                // _data.CommandText = "  ";
                _data.OpenWithOutTrans();

                //Executing Query
                object obj = _data.Execute(OracleDataAccess.ExecutionType.ExecuteNonQuery);
                returnValue = Convert.ToInt16(obj);


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

        #endregion

        [WebMethod]
        public PatientData GetPatientDocumentByPatientDocumentID(Int64 PatientDocumentID)
        {
            PatientData data = new PatientData();
            OracleDataAccess.OracleCommandData _data = new OracleDataAccess.OracleCommandData();
            try
            {

                _data._CommandType = CommandType.Text;
                _data.CommandText = "SELECT * FROM PATIENT_DOCUMENT where Patient_Document_ID="+ PatientDocumentID + "";// trim(DOC_CATEGORY_ID)=" + CategoryID + " AND trim(Patient_ID)='" + PatientID + "'";
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
        public int DeletePatientDocumentByPatientDocumentID(Int64 PatientDocumentID)
        {
            int _ReturnValue = 0;
            PatientData data = new PatientData();
            OracleDataAccess.OracleCommandData _data = new OracleDataAccess.OracleCommandData();
            try
            {

                _data._CommandType = CommandType.Text;
                _data.CommandText = "SELECT * FROM PATIENT_DOCUMENT where  Patient_Document_ID=" + PatientDocumentID + "";// trim(DOC_CATEGORY_ID)=" + CategoryID + " AND trim(Patient_ID)='" + PatientID + "'";
                _data.OpenWithOutTrans();

                //Executing Query
                DataSet _ds = _data.Execute(OracleDataAccess.ExecutionType.ExecuteDataSet) as DataSet;

                data.dt = _ds.Tables[0];
                data.isValid = true;
                if(_ds.Tables[0].Rows.Count>0)
                {
                    foreach (DataRow item in _ds.Tables[0].Rows)
                    {
                        FileHelper.DeleteFile(item["Path"].ToString());
                        _ReturnValue = 1;
                    }
                }

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

            return _ReturnValue;
        }











    }
}
