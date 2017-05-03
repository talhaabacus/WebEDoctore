using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebEdoc2017.Models;

namespace WebEdoc2017.ViewModels
{
    public class HomeViewModel
    {
        public List<DocumentCategoryModel> lstDocumentCategory;
        public List<PatientDocumentModel> lstPatientDocument;
        public HomeViewModel()
        {
            lstDocumentCategory = new List<DocumentCategoryModel>();
            lstPatientDocument = new List<PatientDocumentModel>();
        }
    }
}
