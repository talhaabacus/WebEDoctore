using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebEdoc2017.Models;
using WebEdoc2017.ViewModels;

namespace WebEdoc2017.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Index(string patient_id, string LoginType, string Pid)
        {
          
            HomeViewModel model = new HomeViewModel();
            PHICService.PatientData documentCategory = new PHICService.PatientData();
            PHICService.PatientData patientDocumentdata = new PHICService.PatientData();

            // PHICService.PatientData data = new PHICService.PatientData();
            using (var service=new PHICService.PHICServiceSoapClient())
            {
                documentCategory = service.GetDocumentCategory();
                patientDocumentdata = service.GetPatientDocumentByPatientID(patient_id);


            }
            if (documentCategory.dt.Rows.Count > 0)
            {
                var categoryRecord = from myRow in documentCategory.dt.AsEnumerable() select myRow;
                if (documentCategory.dt.Rows.Count > 0)
                {
                    foreach (DataRow item in documentCategory.dt.Rows)
                    {
                        model.lstDocumentCategory.Add(new DocumentCategoryModel { DOC_CATEGORY_ID = Convert.ToInt64(item["DOC_CATEGORY_ID"].ToString()), Name = item["Name"].ToString(), Description = item["Description"].ToString(), Parent_ID = Convert.ToInt64(item["Parent_ID"]) });
                    }
                }
            }
            if(patientDocumentdata.dt.Rows.Count >0)
            {
                foreach (DataRow item in patientDocumentdata.dt.Rows)
                {

                    model.lstPatientDocument.Add(new PatientDocumentModel
                    {
                        PATIENT_DOCUMENT_ID = Convert.ToInt64(item["PATIENT_DOCUMENT_ID"])
                      ,
                        NAME = item["Name"].ToString(),
                        DESCRIPTION = item["Description"].ToString(),
                        DOC_CATEGORY_ID = Convert.ToInt64(item["DOC_CATEGORY_ID"].ToString()),
                        ELECTRONIC_LINK = item["ELECTRONIC_LINK"].ToString(),
                        PATH = item["PATH"].ToString(),
                        PATIENT_ID = item["Patient_ID"].ToString(),
                        EXTENSION = item["EXTENSION"].ToString(),
                        TITLE = item["TITLE"].ToString()
                    });
                }
            }


           
            ViewBag.PatientLogID = patient_id;
            ViewBag.LogInType = LoginType;

            return View(model);
        }

        #region PatientDocument By Category.........

        [HttpPost]
        public ActionResult getPatientDocumentByCategoryID(Int64 MenuID,string Patient_ID,string LoginType)
        {
            List<PatientDocumentModel> model = new List<PatientDocumentModel>();
            string returnData = string.Empty;
            PHICService.PatientData data = new PHICService.PatientData();
            using (var service = new PHICService.PHICServiceSoapClient())
            {
                data = service.GetPatientDocumentByCategoryID(MenuID, Patient_ID);

            }
            if(data.isValid)
            {
                foreach (DataRow item in data.dt.Rows)
                {
                  
                    model.Add(new PatientDocumentModel
                    {
                        PATIENT_DOCUMENT_ID = Convert.ToInt64(item["PATIENT_DOCUMENT_ID"])
                      ,
                        NAME = item["Name"].ToString(),
                        DESCRIPTION = item["Description"].ToString(),
                        DOC_CATEGORY_ID = Convert.ToInt64(item["DOC_CATEGORY_ID"].ToString()),
                        ELECTRONIC_LINK = item["ELECTRONIC_LINK"].ToString(),
                        PATH = item["PATH"].ToString(),
                        PATIENT_ID = item["Patient_ID"].ToString(),
                       EXTENSION = item["EXTENSION"].ToString(),
                        TITLE = item["TITLE"].ToString()
                    });
                }
                ViewBag.CategoryID = MenuID;
                ViewBag.LoginType = LoginType;
                returnData = RenderPartialViewToString("_patientDocumentPartial", model);
            }
            return Json(returnData);
        }


        [HttpPost]
        public ActionResult AddPatientDocument(Int64 MenuID, PatientDocumentModel _parameter,string LoginType)
        {

            var _fileName = string.Empty;
            string _Path = string.Empty;
            string _Extension = string.Empty;
            byte[] byteData=null;
            if (Request.Files.Count > 0)
            {

                HttpFileCollectionBase files = Request.Files;
                string _imgname = string.Empty;
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var pic = System.Web.HttpContext.Current.Request.Files["FileUpload"];
                    if (pic.ContentLength > 0)
                    {
                      
                        _Extension = Path.GetExtension(pic.FileName);
                        string FileName = Path.GetFileNameWithoutExtension(pic.FileName);
                        _Path = Server.MapPath("/Upload/PHICDocument/") + FileName + DateTime.Now.ToString("yyyyMMddHH24MISS") + _Extension;
                        using (var binaryReader = new BinaryReader(System.Web.HttpContext.Current.Request.Files["FileUpload"].InputStream))
                        {
                            byteData = binaryReader.ReadBytes(System.Web.HttpContext.Current.Request.Files["FileUpload"].ContentLength);
                        }
                       MemoryStream ms = new MemoryStream();
                    }
                }
            }
            bool IsValid = false;
            List<PatientDocumentModel> model = new List<PatientDocumentModel>();
            string returnData = string.Empty;
            PHICService.paraPatientDocument parm = new PHICService.paraPatientDocument();

            parm.PatientDocumentID = _parameter.PATIENT_DOCUMENT_ID;
            parm.Name = _parameter.NAME;
            parm.Title = _parameter.TITLE;
            parm.Description = _parameter.DESCRIPTION;
            parm.CategoryID = _parameter.DOC_CATEGORY_ID;
            parm.ElectronicLink = _parameter.ELECTRONIC_LINK;
            parm.Path = _Path;
            parm.Extension = _Extension;
            parm.PatientID = _parameter.PATIENT_ID;
            parm.attachement = byteData;
            using (var service = new PHICService.PHICServiceSoapClient())
            {
                int Value = service.AddPatientDocument(MenuID, parm);
                if(Value>0)
                {
                    IsValid = true;
                }

            }
            if (IsValid)
            {
               
                PHICService.PatientData data = new PHICService.PatientData();
                using (var service = new PHICService.PHICServiceSoapClient())
                {
                    if(LoginType=="Doctor")
                    data = service.GetPatientDocumentByCategoryID(MenuID,_parameter.PATIENT_ID);
                    else
                    data = service.GetPatientDocumentByPatientID(_parameter.PATIENT_ID);


                }
                if (data.isValid)
                {
                    foreach (DataRow item in data.dt.Rows)
                    {

                        model.Add(new PatientDocumentModel
                        {
                            PATIENT_DOCUMENT_ID = Convert.ToInt64(item["PATIENT_DOCUMENT_ID"])
                          ,
                            NAME = item["Name"].ToString(),
                            DESCRIPTION = item["Description"].ToString(),
                            DOC_CATEGORY_ID = Convert.ToInt64(item["DOC_CATEGORY_ID"].ToString()),
                            ELECTRONIC_LINK = item["ELECTRONIC_LINK"].ToString(),
                            PATH = item["PATH"].ToString(),
                            PATIENT_ID = item["Patient_ID"].ToString(),
                            EXTENSION = item["EXTENSION"].ToString(),
                            TITLE = item["TITLE"].ToString()
                        });
                    }
                    ViewBag.CategoryID = MenuID;
                    ViewBag.LoginType = LoginType;
                    returnData = RenderPartialViewToString("_patientDocumentPartial", model);
                }
            }
            return Json(returnData);
        }

        [HttpPost]
        public ActionResult UpdatePatientDocument(Int64 MenuID, PatientDocumentModel _parameter,string LoginType)
        {
            bool IsValid = false;
            var _fileName = string.Empty;
            string _Path = string.Empty;
            string _Extension = string.Empty;
            byte[] byteData = null;
            if (Request.Files.Count > 0)
            {
              
                HttpFileCollectionBase files = Request.Files;
                string _imgname = string.Empty;
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var pic = System.Web.HttpContext.Current.Request.Files["FileUpload"];
                    if (pic.ContentLength > 0)
                    {
                        _Extension = Path.GetExtension(pic.FileName);
                        _Path = Server.MapPath("/Upload/") + _parameter.TITLE+DateTime.Now.ToString("yyyyMMddHH24MISS") + _Extension;
                        using (var binaryReader = new BinaryReader(System.Web.HttpContext.Current.Request.Files["FileUpload"].InputStream))
                        {
                            byteData = binaryReader.ReadBytes(System.Web.HttpContext.Current.Request.Files["FileUpload"].ContentLength);

                        }
                        // Saving Image in Original Mode
                        //  pic.SaveAs(_Path);

                        // resizing image
                        MemoryStream ms = new MemoryStream();
                       // WebImage img = new WebImage(_comPath);

                        //if (img.Width > 200)
                        //    img.Resize(200, 200);
                        //img.Save(_comPath);
                        // end resize
                    }
                }
            }
            List<PatientDocumentModel> model = new List<PatientDocumentModel>();
            string returnData = string.Empty;
            PHICService.paraPatientDocument parm = new PHICService.paraPatientDocument();

            parm.PatientDocumentID = _parameter.PATIENT_DOCUMENT_ID;
            parm.Name = _parameter.NAME;
            parm.Description = _parameter.DESCRIPTION;
            parm.CategoryID = _parameter.DOC_CATEGORY_ID;
            parm.ElectronicLink = _parameter.ELECTRONIC_LINK;
            parm.Path = _Path;
            parm.Title = _parameter.TITLE;
            parm.Extension = _Extension;
            parm.PatientID = _parameter.PATIENT_ID;
            parm.attachement = byteData;
            using (var service = new PHICService.PHICServiceSoapClient())
            {
                int Value = service.UpdatePatientDocument(MenuID, parm);
                if (Value > 0)
                {
                    IsValid = true;
                }

            }
            if (IsValid)
            {

                PHICService.PatientData data = new PHICService.PatientData();
                using (var service = new PHICService.PHICServiceSoapClient())
                {
                    if(LoginType=="Doctor")
                    data = service.GetPatientDocumentByCategoryID(MenuID, _parameter.PATIENT_ID);
                    else
                        data = service.GetPatientDocumentByPatientID(_parameter.PATIENT_ID);

                }
                if (data.isValid)
                {
                    foreach (DataRow item in data.dt.Rows)
                    {

                        model.Add(new PatientDocumentModel
                        {
                            PATIENT_DOCUMENT_ID = Convert.ToInt64(item["PATIENT_DOCUMENT_ID"])
                          ,
                            NAME = item["Name"].ToString(),
                            DESCRIPTION = item["Description"].ToString(),
                            DOC_CATEGORY_ID = Convert.ToInt64(item["DOC_CATEGORY_ID"].ToString()),
                            ELECTRONIC_LINK = item["ELECTRONIC_LINK"].ToString(),
                            PATH = item["PATH"].ToString(),
                            PATIENT_ID = item["Patient_ID"].ToString(),
                            EXTENSION = item["EXTENSION"].ToString(),
                            TITLE = item["TITLE"].ToString()
                        });
                    }
                    ViewBag.CategoryID = MenuID;
                    ViewBag.LoginType = LoginType;
                    returnData = RenderPartialViewToString("_patientDocumentPartial", model);
                }
            }
            return Json(returnData);
        }


        [HttpPost]
        public ActionResult DeletePatientDocument(Int64 PatientDocumentID,Int64 CategoryID,string Patient_ID,string LoginType, Int64 SelectedCategoryID)
        {
            bool IsValid = false;
            List<PatientDocumentModel> model = new List<PatientDocumentModel>();
            string returnData = string.Empty;
            PHICService.paraPatientDocument parm = new PHICService.paraPatientDocument();

            

            using (var service = new PHICService.PHICServiceSoapClient())
            {

                int ReturnValue = 0;
              

                ReturnValue = service.DeletePatientDocument(PatientDocumentID, CategoryID);
                if (ReturnValue > 0)
                {
                    //First delete the file if exist..Then delete the record...
                    ReturnValue = service.DeletePatientDocumentByPatientDocumentID(PatientDocumentID);
                    ///
                    IsValid = true;
                }

            }
           

                PHICService.PatientData data = new PHICService.PatientData();
                using (var service = new PHICService.PHICServiceSoapClient())
                {
                    if(LoginType=="Doctor")
                    data = service.GetPatientDocumentByCategoryID(SelectedCategoryID, Patient_ID);


                else
                    data = service.GetPatientDocumentByPatientID( Patient_ID);
                }
                if (data.isValid)
                {
                    foreach (DataRow item in data.dt.Rows)
                    {

                        model.Add(new PatientDocumentModel
                        {
                            PATIENT_DOCUMENT_ID = Convert.ToInt64(item["PATIENT_DOCUMENT_ID"])
                          ,
                            NAME = item["Name"].ToString(),
                            DESCRIPTION = item["Description"].ToString(),
                            DOC_CATEGORY_ID = Convert.ToInt64(item["DOC_CATEGORY_ID"].ToString()),
                            ELECTRONIC_LINK = item["ELECTRONIC_LINK"].ToString(),
                            PATH = item["PATH"].ToString(),
                            PATIENT_ID = item["Patient_ID"].ToString(),
                            EXTENSION = item["EXTENSION"].ToString()
                        });
                    }
                    ViewBag.CategoryID = CategoryID;
                    ViewBag.LoginType = LoginType;
                    returnData = RenderPartialViewToString("_patientDocumentPartial", model);
                }
            
            return Json(returnData);
        }


        [HttpPost]
        public ActionResult SearchPatientDocument(PatientDocumentModel model)
        {
            string _returnValue = string.Empty;
            HomeViewModel homeviewModel = new HomeViewModel();
            StringBuilder filterclause = new StringBuilder();

            if(model.PATIENT_ID.ToString() !="")
            {
                filterclause.Append(" UPPER(trim(PATIENT_ID)) ='" + model.PATIENT_ID.Trim().ToUpper() + "' AND ");
            }
            if(model.TITLE !=null)
            {
                filterclause.Append(" UPPER(trim(TITLE)) like'%" + model.TITLE.Trim().ToUpper() + "%' AND ");
            }
            if(model.NAME != null)
            {
                filterclause.Append(" UPPER(trim(Name)) like'%" + model.NAME.Trim().ToUpper() + "%' AND ");

            }
            if (model.DESCRIPTION != null)
            {
                filterclause.Append(" UPPER(trim(DESCRIPTION)) like'%" + model.DESCRIPTION.Trim().ToUpper() + "%' AND ");

            }
            if (model.DOC_CATEGORY_ID !=0)
            {
                filterclause.Append(" trim(DOC_CATEGORY_ID) =" + model.DOC_CATEGORY_ID + " AND ");

            }
            if(filterclause.Length>0)
            {
                filterclause = filterclause.Remove(filterclause.Length - 4, 4);
            }
            PHICService.PatientData PatientData = new PHICService.PatientData();
            using (var service=new PHICService.PHICServiceSoapClient())
            {
                PatientData = service.SearchPatientDocument(filterclause.ToString());
            }
            if(PatientData.isValid)
            {
                foreach (DataRow item in PatientData.dt.Rows)
                {

                    homeviewModel.lstPatientDocument.Add(new PatientDocumentModel
                    {
                        PATIENT_DOCUMENT_ID = Convert.ToInt64(item["PATIENT_DOCUMENT_ID"])
                      ,
                        NAME = item["Name"].ToString(),
                        DESCRIPTION = item["Description"].ToString(),
                        DOC_CATEGORY_ID = Convert.ToInt64(item["DOC_CATEGORY_ID"].ToString()),
                        ELECTRONIC_LINK = item["ELECTRONIC_LINK"].ToString(),
                        PATH = item["PATH"].ToString(),
                        PATIENT_ID = item["Patient_ID"].ToString(),
                        EXTENSION = item["EXTENSION"].ToString(),
                        TITLE = item["TITLE"].ToString()
                    });
                }
                ViewBag.CategoryID = model.DOC_CATEGORY_ID;
                _returnValue = RenderPartialViewToString("_patientDocumentPartial", homeviewModel.lstPatientDocument);
            }
            return Json(_returnValue);
        }

        #endregion




        [HttpPost]
        public ActionResult SaveCategory(Int64 _MenuID, Int64 _ParentID, string actionType, string CategoryName, string CategoryDesc)
        {
            string HTML = string.Empty;
            string _ddlCategory = string.Empty;
            int returnValue = 0;
            PHICService.paraDocumentCategory _para = new PHICService.paraDocumentCategory();
            _para.DOC_CATEGORY_ID = _MenuID;
            _para.Name = CategoryName;
            _para.Description = CategoryDesc;
            _para.Parent_ID = _ParentID;
            using (var service = new PHICService.PHICServiceSoapClient())
            {
                 //= service.GetPatientDocumentByCategoryID(MenuID);
                  
                        returnValue = service.SaveDocumentCategory(_para,actionType.ToUpper().Trim());
                   
            }

            if(returnValue>0)
            {
                List<DocumentCategoryModel> model = new List<DocumentCategoryModel>();
                PHICService.PatientData data = new PHICService.PatientData();
                using (var service = new PHICService.PHICServiceSoapClient())
                {
                    data = service.GetDocumentCategory();

                }
                var categoryRecord = from myRow in data.dt.AsEnumerable() select myRow;
                if (data.dt.Rows.Count > 0)
                {
                    foreach (DataRow item in data.dt.Rows)
                    {
                        model.Add(new DocumentCategoryModel { DOC_CATEGORY_ID = Convert.ToInt64(item["DOC_CATEGORY_ID"].ToString()), Name = item["Name"].ToString(), Description = item["Description"].ToString(), Parent_ID = Convert.ToInt64(item["Parent_ID"]) });
                    }
                }
                HTML = RenderPartialViewToString("_treeViewPartial", model);
                _ddlCategory = RenderPartialViewToString("DropDownList/_ddlDocumentCategory", model);

                
            }

           

            return Json(new { treeView=HTML,ddlCategory=_ddlCategory });
        }

        [HttpPost]
        public ActionResult DeleteDocumentCategoryByCategoryID(Int64 CategroyID)
        {
            string returnHtml = string.Empty;
            string _ddlDocumentCategory = string.Empty;
            bool IsValid = true;
            var Error = string.Empty;
            int returnValue = 0;

            PHICService.PatientData _PatientData = new PHICService.PatientData();
            

        
            using (var service = new PHICService.PHICServiceSoapClient())
            {
                //Check is category going for deleting is user in Patient Document or Not

                _PatientData = service.GetCategoryRecordFromPatientDocument(CategroyID);
                if (_PatientData.isValid)
                {
                    if (_PatientData.dt.Rows.Count > 0)
                    {
                        IsValid = false;
                        Error = "Deleting Category is in Used";
                    }
                    else
                    {
                        returnValue = service.DeleteDocumentCategoryByCategoryID(CategroyID);
                    }
                }

            }

            if (returnValue > 0)
            {
                List<DocumentCategoryModel> model = new List<DocumentCategoryModel>();
                PHICService.PatientData data = new PHICService.PatientData();
                using (var service = new PHICService.PHICServiceSoapClient())
                {
                    data = service.GetDocumentCategory();

                }
                var categoryRecord = from myRow in data.dt.AsEnumerable() select myRow;
                if (data.dt.Rows.Count > 0)
                {
                    foreach (DataRow item in data.dt.Rows)
                    {
                        model.Add(new DocumentCategoryModel { DOC_CATEGORY_ID = Convert.ToInt64(item["DOC_CATEGORY_ID"].ToString()), Name = item["Name"].ToString(), Description = item["Description"].ToString(), Parent_ID = Convert.ToInt64(item["Parent_ID"]) });
                    }
                }
                returnHtml = RenderPartialViewToString("_treeViewPartial", model);
                _ddlDocumentCategory= RenderPartialViewToString("DropDownList/_ddlDocumentCategory", model);
            }



            return Json(new { HTML = returnHtml, IsValid=IsValid, Error = Error, ddlDocumentCategory= _ddlDocumentCategory } );
        }


        public ActionResult DownLoadPatientUploadDocument(Int64 PatientDocumentID)
        {
            PHICService.paraPatientDocument Data = new PHICService.paraPatientDocument();
            using (var _service=new PHICService.PHICServiceSoapClient())
            {
                Data = _service.GetPatientDocumentByPatientDocumentID(PatientDocumentID);
            }
            if (Data.Name.ToString() !="" & Data.Name.ToString()!=null)
            {
                           return File(Data.attachement, System.Net.Mime.MediaTypeNames.Application.Octet, Data.Name);
            }
             return  Content("No Document found!");
         
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}