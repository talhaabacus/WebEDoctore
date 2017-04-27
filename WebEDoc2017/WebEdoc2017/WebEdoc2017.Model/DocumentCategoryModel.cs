using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebEdoc2017.Models
{
   public class DocumentCategoryModel
    {
        public Int64 DOC_CATEGORY_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Int64 Parent_ID { get; set; }
    }
}
