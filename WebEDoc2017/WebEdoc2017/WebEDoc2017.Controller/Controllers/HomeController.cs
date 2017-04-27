﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebEdoc2017.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            PHICService.PatientData data = new PHICService.PatientData();
            using (var service=new PHICService.PHICServiceSoapClient())
            {
                data = service.GetDocumentCategory();
            }
            var returnData = data.dt;
            return View(returnData);
        }
    }
}