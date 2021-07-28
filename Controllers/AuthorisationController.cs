using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP_Smiley.Models;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;

namespace FYP_Smiley.Controllers
{
    public class AuthorisationController : Controller
    {
        private AppDbContext _dbContext;

        public AuthorisationController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            DbSet<Authorisation> dbs = _dbContext.Authorisation;
            List<Authorisation> model = dbs.ToList();

            return View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            DbSet<Authorisation> dbs = _dbContext.Authorisation;
            var lstPokes = dbs.ToList();
            ViewData["pokes"] = new SelectList(lstPokes, "FacilityId", "Name");
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(Authorisation authorisation)
        {
            if (ModelState.IsValid)
            {
                DbSet<Authorisation> dbs = _dbContext.Authorisation;
                dbs.Add(authorisation);

                if (_dbContext.SaveChanges() == 1)
                    TempData["Msg"] = "New Authorisation added!";
                else
                    TempData["Msg"] = "Failed to update database!";
            }
            else
            {
                TempData["Msg"] = "Invalid information entered";
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Update(int id)
        {
            DbSet<Authorisation> dbs = _dbContext.Authorisation;
            Authorisation tOrder = dbs.Where(mo => mo.AccessPointId == id).FirstOrDefault();

            return View(tOrder);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Update(Authorisation authorisation)
        {
            if (ModelState.IsValid)
            {
                DbSet<Authorisation> dbs = _dbContext.Authorisation;
                Authorisation tOrder = dbs.Where(mo => mo.AccessPointId == authorisation.AccessPointId)
                                     .FirstOrDefault();

                if (tOrder != null)
                {
                    tOrder.AccessPointId = authorisation.AccessPointId;
                    tOrder.StartDate = authorisation.StartDate;
                    tOrder.EndDate = authorisation.EndDate;
                    tOrder.UserId = authorisation.UserId;

                    if (_dbContext.SaveChanges() == 1)
                        TempData["Msg"] = "Authorisation updated!";
                    else
                        TempData["Msg"] = "Failed to update database!";
                }
                else
                {
                    TempData["Msg"] = "Authorisation not found!";
                    return RedirectToAction("Index");
                }
            }

            else
            {
                TempData["Msg"] = "Invalid information entered";
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            DbSet<Authorisation> dbs = _dbContext.Authorisation;

            Authorisation tOrder = dbs.Where(mo => mo.AccessPointId == id).FirstOrDefault();

            if (tOrder != null)
            {
                dbs.Remove(tOrder);

                if (_dbContext.SaveChanges() == 1)
                    TempData["Msg"] = "Authorisation deleted!";
                else
                    TempData["Msg"] = "Failed to update database!";
            }
            else
            {
                TempData["Msg"] = "Authorisation not found!";
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Print(int id)
        {
            DbSet<Authorisation> dbs = _dbContext.Authorisation;
            List<Authorisation> model = dbs.ToList();

            if (model != null)
                return new ViewAsPdf(model)
                {
                    PageSize = Rotativa.AspNetCore.Options.Size.B5,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
                };
            else
            {
                TempData["Msg"] = "Record not found!";
                return RedirectToAction("Index");
            }

        }

    }
}
