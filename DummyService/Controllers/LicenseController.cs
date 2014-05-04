using DummyService.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DummyService.Controllers
{
    public class LicenseController : Controller
    {


        //
        // GET: /LicenseManager/

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult CheckEntitlement(OAuthResult result)
        {
            //get the appId from web.config
            string appId = ConfigurationManager.AppSettings["thisAppId"] == null 
                ? "" 
                : ConfigurationManager.AppSettings["thisAppId"];
           
            bool entitled = IsEntitledUser(result.oauth_user_guid, appId);

            if (entitled)
            {
                ViewBag.Message = "you are entitled to use this app.";

                UserInfo usrInfo = new UserInfo();
                usrInfo.IsLoggedIn = true;
                usrInfo.UserType = LoginType.AutodeskId;
                usrInfo.UserId = result.oauth_user_guid;
                usrInfo.UserName = result.oauth_user_name;
                return RedirectToAction("index", "Home", usrInfo);
            }
            else
            {
                ViewBag.Message = "you are not entitled to use this app. " +
                "please buy it from Autodesk Exchange store.";
            }

            return View();
        }

        private bool IsEntitledUser(string userId, string appId)
        {
                RestClient client = new RestClient(Constants.AUTODESK_EXCHANGE_URL);
            RestRequest req = new RestRequest(Constants.CHECK_ENTITLEMENT_ENDPOINT);
            req.Method = Method.GET;
            req.AddParameter("userid", userId);
            req.AddParameter("appid", appId);
            #region Required for Autodesk Exchange staging server
            req.AddCookie("RequestAccessCookie", "Aut0d3sk");
            #endregion

            IRestResponse<EntitlementResult> resp= client.Execute<EntitlementResult>(req);

            if (resp.Data != null && resp.Data.IsValid)
            {
                return true;
            }
            else
            {
                return false;
            }

        }




    }
}
