using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scooterki.Models;

namespace Scooterki.Controllers
{
    public class ScootersController : Controller
    {
        ScootersDatabase db = new ScootersDatabase();
        // GET: Scooters
        public ActionResult Index()
        {
            return View("index", db.Scooters_table.OrderBy(c => c.Name));
        }

        [HttpGet]
        public ActionResult AddNewScooter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewScooter(Scooters_table newScooter)
        {
            if (ModelState.IsValid == true)
            {
                db.Scooters_table.Add(newScooter);
                db.SaveChanges();
                return RedirectToAction("index");
            }
            else
            {
                return View("AddNewScooter");
            }
        }
    }
}