using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebEdoc2017.Models;
using WebEdoc2017.ViewModels;

namespace WebEdoc2017.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
          
            PHICService.PatientData data = new PHICService.PatientData();
            using (var service=new PHICService.PHICServiceSoapClient())
            {
                data = service.GetDocumentCategory();
              
            }
            var categoryRecord = from myRow in data.dt.AsEnumerable() select myRow;
            if(data.dt.Rows.Count > 0)
            {
                foreach (DataRow item in data.dt.Rows)
                {
                    model.lstDocumentCategory.Add(new DocumentCategoryModel { DOC_CATEGORY_ID = Convert.ToInt64(item["DOC_CATEGORY_ID"].ToString()), Name = item["Name"].ToString(), Description = item["Description"].ToString(), Parent_ID = Convert.ToInt64(item["Parent_ID"]) });
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult getPatientDocumentByCategoryID(Int64 MenuID, Int64 ParentID)
        {
            List<PatientDocumentModel> model = new List<PatientDocumentModel>();
            string returnData = string.Empty;
            PHICService.PatientData data = new PHICService.PatientData();
            using (var service = new PHICService.PHICServiceSoapClient())
            {
                data = service.GetPatientDocumentByCategoryID(MenuID);

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
                        PATIENT_ID = Convert.ToInt64(item["Patient_ID"].ToString()),
                       EXTENSION = item["EXTENSION"].ToString()
                    });
                }
                returnData = RenderPartialViewToString("_patientDocumentPartial", model);
            }
            return Json(returnData);
        }

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
            }

           

            return Json(HTML);
        }

        [HttpPost]
        public ActionResult DeleteDocumentCategoryByCategoryID(Int64 CategroyID)
        {
            string HTML = string.Empty;
            int returnValue = 0;
        
            using (var service = new PHICService.PHICServiceSoapClient())
            {
                //= service.GetPatientDocumentByCategoryID(MenuID);

                returnValue = service.DeleteDocumentCategoryByCategoryID(CategroyID);

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
                HTML = RenderPartialViewToString("_treeViewPartial", model);
            }



            return Json(HTML);
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