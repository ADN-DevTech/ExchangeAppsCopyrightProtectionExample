using DummyService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DummyService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(UserInfo usrInfo = null)
        {

            if (usrInfo != null && usrInfo.IsLoggedIn == true )
            {
                if (usrInfo.UserType == LoginType.AutodeskId)
                {
                    ViewBag.Message = "You are logged in with AutodeskId as " + usrInfo.UserName + ".";
                }
                else
                {
                    ViewBag.Message = "You are logged in with custom user system as " + usrInfo.UserName + ".";
                }
                
            }
            return View();

        }

        public ActionResult About()
        {
            return View();
        }
    }
}
