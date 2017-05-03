using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebEdoc2017.Models
{
    public class PatientDocumentModel
    {
        public Int64 PATIENT_DOCUMENT_ID { get; set; }
        public string   NAME{get;set;}
        public string   TITLE{get;set;}

        public string   DESCRIPTION{get;set;}

        public Int64   DOC_CATEGORY_ID{get;set;}

        public string   ELECTRONIC_LINK {get;set;}
        public string   PATH {get;set;}
        public Int64   PATIENT_ID {get;set;}
        public string EXTENSION { get; set; }


    
    }
}
