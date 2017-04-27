using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;


namespace WebEdoc2017.Controllers
{
    public class PHICControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            switch (controllerName)
            {
                case "Home": return new HomeController();
                    //default: return new AdminController();

            }
            return null;
        }
    }
}

