using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Kutse_App.Models;

namespace Kutse_App.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string month = DateTime.Now.ToString("MMMM");

            ViewBag.Message = month == "Март" ? "Ootan sind minu peole! Palun tule!!! " + "С 8 Марта, милые дамы!" :
                month == "Апрель" ? "Ootan sind minu peole! Palun tule!!!" + "Апрель принёс нам радость и веселье": 
                "Ootan sind minu peole! Palun tule!!!";
            int hour = DateTime.Now.Hour;
            

            ViewBag.Greeting = 
                hour < 10 & hour > 6 ? "Tere hommikust!" :
                hour < 17 & hour > 10 ? "Tere päevast!" : 
                hour < 23 & hour > 17 ? "Head õhtut!" : 
                "Head ööd";
            return View();
        }

        [HttpGet]
        public ViewResult Ankeet()
        {
            return View();
        }
        
        [HttpGet]
        public ViewResult Meeldetuletus(Guest guest)
        {
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "iljaharbi@gmail.com";
                WebMail.Password = "";
                WebMail.From = "iljaharbi@gmail.com";

                WebMail.Send("ilja200303@gmail.com", "Meeldetuletus",  guest.Name + ", ara unusta. Pidu toimub 12.03.20! Sind ootavad väga!",
                    "iljaharbi@gmail.com", "ilja200303@gmail.com",
                    filesToAttach: new String[] { Path.Combine(Server.MapPath("~/Images/"), Path.GetFileName("ran.jpeg")) }
                );
                
                ViewBag.Message = "saatnud";
            }
            catch (Exception e)
            {
                ViewBag.Message = e;
            }
            return View();
        }

        [HttpPost]
        public ViewResult Ankeet(Guest guest)
        {
            E_mail(guest);
            if (ModelState.IsValid)
            {
                return View("Thanks", guest);
            }
            else
            {
                return View();
            }
        }

        public void E_mail(Guest guest)
        {
            
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "iljaharbi@gmail.com";
                WebMail.Password = "";
                WebMail.From = "iljaharbi@gmail.com";
                WebMail.Send(guest.Email, "Vastus kutsele", guest.Name + " vastus " + ((guest.WillAttend ?? false) ?
                    "tuleb peole ": "ei tule peole"));

                ViewBag.Message = "kiri on saatnud";
            }
            catch (Exception)
            {
                ViewBag.Message = "Mul on kahjuks! Ei saa kirja saada";
            }
        }
    }
}