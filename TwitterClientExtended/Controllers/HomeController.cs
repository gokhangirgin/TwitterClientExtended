using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TwitterClientExtended.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            TwitterClient2 tw = new TwitterClient2("", "");
            AuthenticationResult result = tw.VerifyAuthentication(this.HttpContext);
            if (!result.IsSuccessful)
                tw.RequestAuthentication(this.HttpContext, new Uri(Request.Url.AbsoluteUri));
            else
            {
                ViewBag.Message = JsonConvert.SerializeObject(result.ExtraData).ToString();
            }
            return View();
        }
    }
}