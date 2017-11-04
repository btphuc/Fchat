using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fchat.Models;

namespace Fchat.Controllers
{
    public class HomeController : Controller
    {
        private MTravelEntities db = new MTravelEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Chat()
        {
            if(Session["UserName"] == null)
            {
                return RedirectToAction("login");
            }

            if(int.Parse(Session["Role"].ToString()) == 1)
            {
                return RedirectToAction("TuVan");
            }
            return View();
        }

        public ActionResult TuVan ()
        {
            if (Session["UserName"] ==null && int.Parse(Session["Role"].ToString()) == 1)
            {
                LOGOUT();
                return RedirectToAction("login");
            }
            return View();
        }


        public ActionResult login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult login(FormCollection f)
        {
            var username = f["username"];
            var pass = f["password"];

            var user = db.Users.FirstOrDefault(x => x.UserName == username && x.PassWord == pass);

            if(user == null)
            {
                ViewBag.err = "Sai ten đăng nhập , mat khau";
                return View();
            }

            Session["UserName"] = user.HoTen;

            Session["UserID"] = user.IDuser;

            var role = db.CTQs.FirstOrDefault(x => x.IDuser == user.IDuser);

            Session["Role"] = role.IdQuyen;

            if(int.Parse(Session["Role"].ToString()) == 2)
                return RedirectToAction("Chat");
            else 
            {
                return RedirectToAction("TuVan");
            }
        }

         public void LOGOUT()
        {
            Session.Clear();
        }
        
    }
}