using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Candidate()
        {
            return View();
        }

        /// <summary>
        /// Downloads files from Upload
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult Download(string file)
        {
            file = Path.Combine(Server.MapPath("~/App_Data/Uploads"), file);

            var ext = Path.GetExtension(file);

            if (!System.IO.File.Exists(file))
                return HttpNotFound();
            

            var fileBytes = System.IO.File.ReadAllBytes(file);
            var response = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = $"Resume{ext}"
               
            };
            return response;
        }

    }
}
