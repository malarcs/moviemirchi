﻿
namespace MvcWebRole1.Controllers
{
    using System.Web.Mvc;

    public class MovieController : Controller
    {
        [HttpGet]
        public ActionResult Index(string movieid)
        {
            return View();
        }
    }
}
