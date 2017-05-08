using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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
        public ActionResult Index(string patient_id, string Visit_key, string Pid)
        {
          
            HomeViewModel model = new HomeViewModel();
            PHICService.PatientData data = new PHICService.PatientData();
           // PHICService.PatientData data = new PHICService.PatientData();
            using (var service=new PHICService.PHICService())
            {
                data = service.GetDocumentCategory();
              
            }
            if (data.dt.Rows.Count > 0)
            {
                var categoryRecord = from myRow in data.dt.AsEnumerable() select myRow;
                if (data.dt.Rows.Count > 0)
                {
                    foreach (DataRow item in data.dt.Rows)
                    {
                        model.lstDocumentCategory.Add(new DocumentCategoryModel { DOC_CATEGORY_ID = Convert.ToInt64(item["DOC_CATEGORY_ID"].ToString()), Name = item["Name"].ToString(), Description = item["Description"].ToString(), Parent_ID = Convert.ToInt64(item["Parent_ID"]) });
                    }
                }
            }
           
            ViewBag.PatientLogID = patient_id;

            return View(model);
        }

        #region PatientDocument By Category.........

        [HttpPost]
        public ActionResult getPatientDocumentByCategoryID(Int64 MenuID,string Patient_ID)
        {
            List<PatientDocumentModel> model = new List<PatientDocumentModel>();
            string returnData = string.Empty;
            PHICService.PatientData data = new PHICService.PatientData();
            using (var service = new PHICService.PHICService())
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
                returnData = RenderPartialViewToString("_patientDocumentPartial", model);
            }
            return Json(returnData);
        }


        [HttpPost]
        public ActionResult AddPatientDocument(Int64 MenuID, PatientDocumentModel _parameter)
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
            using (var service = new PHICService.PHICService())
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
                using (var service = new PHICService.PHICService())
                {
                    data = service.GetPatientDocumentByCategoryID(MenuID,_parameter.PATIENT_ID);

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
                    returnData = RenderPartialViewToString("_patientDocumentPartial", model);
                }
            }
            return Json(returnData);
        }

        [HttpPost]
        public ActionResult UpdatePatientDocument(Int64 MenuID, PatientDocumentModel _parameter)
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
            using (var service = new PHICService.PHICService())
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
                using (var service = new PHICService.PHICService())
                {
                    data = service.GetPatientDocumentByCategoryID(MenuID, _parameter.PATIENT_ID);

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
                    returnData = RenderPartialViewToString("_patientDocumentPartial", model);
                }
            }
            return Json(returnData);
        }


        [HttpPost]
        public ActionResult DeletePatientDocument(Int64 PatientDocumentID,Int64 CategoryID,string Patient_ID)
        {
            bool IsValid = false;
            List<PatientDocumentModel> model = new List<PatientDocumentModel>();
            string returnData = string.Empty;
            PHICService.paraPatientDocument parm = new PHICService.paraPatientDocument();

            

            using (var service = new PHICService.PHICService())
            {

                int ReturnValue = 0;
                //First delete the file if exist..Then delete the record...
                ReturnValue = service.DeletePatientDocumentByPatientDocumentID(PatientDocumentID);
                ///

                ReturnValue = service.DeletePatientDocument(PatientDocumentID, CategoryID);
                if (ReturnValue > 0)
                {
                    IsValid = true;
                }

            }
            if (IsValid)
            {

                PHICService.PatientData data = new PHICService.PatientData();
                using (var service = new PHICService.PHICService())
                {
                    data = service.GetPatientDocumentByCategoryID(CategoryID, Patient_ID);

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
                    returnData = RenderPartialViewToString("_patientDocumentPartial", model);
                }
            }
            return Json(returnData);
        }




        #endregion




        [HttpPost]
        public ActionResult SaveCategory(Int64 _MenuID, Int64 _ParentID, string actionType, string CategoryName, string CategoryDesc)
        {
            string HTML = string.Empty;
            int returnValue = 0;
            PHICService.paraDocumentCategory _para = new PHICService.paraDocumentCategory();
            _para.DOC_CATEGORY_ID = _MenuID;
            _para.Name = CategoryName;
            _para.Description = CategoryDesc;
            _para.Parent_ID = _ParentID;
            using (var service = new PHICService.PHICService())
            {
                 //= service.GetPatientDocumentByCategoryID(MenuID);
                  
                        returnValue = service.SaveDocumentCategory(_para,actionType.ToUpper().Trim());
                   
            }

            if(returnValue>0)
            {
                List<DocumentCategoryModel> model = new List<DocumentCategoryModel>();
                PHICService.PatientData data = new PHICService.PatientData();
                using (var service = new PHICService.PHICService())
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
            }

           

            return Json(HTML);
        }

        [HttpPost]
        public ActionResult DeleteDocumentCategoryByCategoryID(Int64 CategroyID)
        {
            string returnHtml = string.Empty;
            bool IsValid = true;
            var Error = string.Empty;
            int returnValue = 0;

            PHICService.PatientData _PatientData = new PHICService.PatientData();
            

        
            using (var service = new PHICService.PHICService())
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
                using (var service = new PHICService.PHICService())
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
            }



            return Json(new { HTML = returnHtml, IsValid=IsValid, Error = Error } );
        }


        public ActionResult DownLoadPatientUploadDocument(Int64 PatientDocumentID)
        {

            PHICService.PatientData Data = new PHICService.PatientData();
            PHICService.PHICService _service = new PHICService.PHICService();
            Data = _service.GetPatientDocumentByPatientDocumentID(PatientDocumentID);
            if(Data.isValid)
            {
                if(Data.dt.Rows.Count>0)
                {
                    foreach (DataRow item in Data.dt.Rows)
                    {
                        string FileName = Path.GetFileName(item["Path"].ToString()); 
                        string _path = Server.MapPath("/Upload/PHICDocument/"+ FileName);
                       
                        byte[] fileBytes = System.IO.File.ReadAllBytes(_path);

                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
                    }
                }
            }
            return View();
           // return File(abc, "application/octet-stream");
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