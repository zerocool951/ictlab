using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Server_WebAPI.Controllers {
    public class HomeController : Controller {

        public ActionResult Index() {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult PostKey(string key) {
            DataHelper.SetKey(key);
            if (DataHelper.GetRuleStore() == null) {
                return Redirect("index?triedkey=1");
            } else {
                return Redirect("index");
            }
        }
    }
}