using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Controllers.v1_0
{
    public class System_History : Controller
    {
        // GET: System_History
        public ActionResult Index()
        {
            return View();
        }

        // GET: System_History/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
    }
}
