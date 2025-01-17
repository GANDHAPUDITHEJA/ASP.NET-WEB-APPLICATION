using BusinessLogic;
using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebAppUser.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(UserBO objUserBO)
        {
            if (ModelState.IsValid)
            {
                UserBL objUserBl = new UserBL();
                CustomBO objCustomBo = objUserBl.AddUser(objUserBO);
                return RedirectToAction("Index");
            }
            return View(objUserBO);
        }
    }
}