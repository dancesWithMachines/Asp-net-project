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

        [HttpGet]
        public ActionResult RemoveScooter(int id)
        {
            var scooterToDelete = db.Scooters_table.Find(id);
            if (scooterToDelete==null)
                return HttpNotFound();
            else
                return View(scooterToDelete);
        }

        [HttpPost, ActionName("RemoveScooter")]
        public ActionResult RemovalConfirmed(int id)
        {
            var scooterToDelete = db.Scooters_table.Find(id);
            if (scooterToDelete == null)
                return HttpNotFound();
            db.Scooters_table.Remove(scooterToDelete);
            db.SaveChanges();
            TempData["message"] = $"Removed scooter name: {scooterToDelete.Name}";
            return RedirectToAction("Index");


        }
    }
}