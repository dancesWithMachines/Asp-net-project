using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scooterki.Models;
using Microsoft.AspNet.Identity;

namespace Scooterki.Controllers
{
    public class ScootersController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            var errorMessage = "";
            var routingText = "Routing data: ";
            var routing = filterContext.RouteData;

            foreach (var element in routing.Values)
                routingText += $"{element.Key} - {element.Value}, ";

            errorMessage = routingText;
            var ex = filterContext.Exception;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            errorMessage += $" Błąd: {ex.Message}";

            var result = new ViewResult()
            {
                ViewName = "Error"
            };
            result.ViewBag.Message = errorMessage;

            filterContext.Result = result;
            filterContext.ExceptionHandled = true;
        }

        ScootersDatabase db = new ScootersDatabase();
        // GET: Scooters
        public ActionResult Index()
        {
            return View("index", db.Scooters_table.OrderBy(c => c.Name));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AddNewScooter()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult RemoveScooter(int id)
        {
            var scooterToDelete = db.Scooters_table.Find(id);
            if (scooterToDelete==null)
                return HttpNotFound();
            else
                return View(scooterToDelete);
        }

        [HttpPost, ActionName("RemoveScooter")]
        [Authorize(Roles = "Admin")]
        public ActionResult RemovalConfirmed(int id)
        {
            var scooterToDelete = db.Scooters_table.Find(id);
            if (scooterToDelete.Equals(null))
                return HttpNotFound();
            if (!scooterToDelete.UserId.Equals(null))
            {
                TempData["message"] = $"Scooter is reserved, cannot delete nor modify";
                return RedirectToAction("RemoveScooter");               
            }
            db.Scooters_table.Remove(scooterToDelete);
            db.SaveChanges();
            TempData["message"] = $"Removed scooter name: {scooterToDelete.Name}";
            return RedirectToAction("Index");


        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                var scooterDetails = db.Scooters_table.Find(id);
                if (scooterDetails == null)
                    throw new HttpException(404, "Couldn't find scooter with this id");
                else
                    return View(scooterDetails);
            }
            catch (SqlException ex)
            {
                Exception lastException = ex;
                while (lastException.InnerException != null)
                {
                    lastException = lastException.InnerException;
                }

                var sqlException = lastException as SqlException;
                if (sqlException != null)
                {
                    switch (sqlException.Number)
                    {
                        case 4060:
                            ViewBag.Message = "Błąd połączenia z bazą danych.";
                            break;
                        default:
                            ViewBag.Message = lastException.Message;
                            break;
                    }
                }
                else
                {
                    ViewBag.Message = lastException.Message;
                }
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditScooter(int id)
        {
            try
            {
                var modifiedScooter = db.Scooters_table.Find(id);
                if (modifiedScooter == null)
                    throw new HttpException(404, "Couldn't find scooter with this id");
                else
                    return View(modifiedScooter);
            }
            catch (SqlException ex)
            {
                Exception lastException = ex;
                while (lastException.InnerException != null)
                {
                    lastException = lastException.InnerException;
                }

                var sqlException = lastException as SqlException;
                if (sqlException != null)
                {
                    switch (sqlException.Number)
                    {
                        case 4060:
                            ViewBag.Message = "Błąd połączenia z bazą danych.";
                            break;
                        default:
                            ViewBag.Message = lastException.Message;
                            break;
                    }
                }
                else
                {
                    ViewBag.Message = lastException.Message;
                }
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditScooter(Scooters_table editScooter)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(editScooter).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("index");
                }
                else
                    return View("EditScooter");
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                Exception lastException = ex;
                while (lastException.InnerException != null)
                {
                    lastException = lastException.InnerException;
                }
                var sqlException = lastException as System.Data.SqlClient.SqlException;
                if (sqlException != null)
                {
                    switch (sqlException.Number)
                    {
                        case 4060:
                            ViewBag.Message = "Błąd połączenia z bazą danych.";
                            break;
                        default:
                            ViewBag.Message = lastException.Message;
                            break;
                    }
                }
                else
                {
                    ViewBag.Message = lastException.Message;
                }
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public ActionResult RentScooter(int id)
        {
            var scooterToRent = db.Scooters_table.Find(id);
            if (scooterToRent == null)
                throw new HttpException(404, "Couldn't find scooter with this id");
            else if (scooterToRent.IsAvilable.Equals(0))
            {
                TempData["message"] = $"This scooter is unavilable";
                return RedirectToAction("index");
            }
            else if (scooterToRent.UserId != null)
            {
                TempData["message"] = $"This scooter is reserved";
                return RedirectToAction("index");
            }
            else
                return View(scooterToRent);
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public ActionResult RentScooter(Scooters_table rentScooter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rentScooter).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("index");
            }
            else
            {
                TempData["message"] = $"Unknown error";
                return View("RentScooter");
            }
        }




        
    }
}