using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebEdoc2017.Models;

namespace WebEdoc2017.Controllers
{
   public static class Extension
    {
        public static List<PatientDocumentModel> ConvertToPatientDocumentList(DataTable dt)
        {

            List<PatientDocumentModel> _lstModel = new List<PatientDocumentModel>();
            foreach (DataRow item in dt.Rows)
            {

                _lstModel.Add(new PatientDocumentModel
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
            return _lstModel;
        }

        public static List<DocumentCategoryModel> ConvertToDocumentCategory(DataTable dt)
        {

            List<DocumentCategoryModel> _lstModel = new List<DocumentCategoryModel>();
            foreach (DataRow item in dt.Rows)
            {

                _lstModel.Add(new DocumentCategoryModel { DOC_CATEGORY_ID = Convert.ToInt64(item["DOC_CATEGORY_ID"].ToString()), Name = item["Name"].ToString(), Description = item["Description"].ToString(), Parent_ID = Convert.ToInt64(item["Parent_ID"]) });
            }
            return _lstModel;
        }
    }
}
