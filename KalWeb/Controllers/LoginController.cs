using KalWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KalWeb.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
           
            return View();
        }
        //[HttpPost]
        //public ActionResult Login(Login objLogin)
        //{
        //    return RedirectToAction("Index", "Home");
        //}
    }
}