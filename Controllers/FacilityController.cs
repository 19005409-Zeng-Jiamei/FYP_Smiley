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
    public class FacilityController : Controller
    {
        private AppDbContext _dbContext;

        public FacilityController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            DbSet<Facility> dbs = _dbContext.Facility;
            List<Facility> model = dbs.ToList();

            return View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            DbSet<Facility> dbs = _dbContext.Facility;
            var lstPokes = dbs.ToList();
            ViewData["pokes"] = new SelectList(lstPokes, "FacilityId", "Name");
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(Facility facility)
        {
            if (ModelState.IsValid)
            {
                DbSet<Facility> dbs = _dbContext.Facility;
                dbs.Add(facility);

                if (_dbContext.SaveChanges() == 1)
                    TempData["Msg"] = "New Facility added!";
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
            DbSet<Facility> dbs = _dbContext.Facility;
            Facility tOrder = dbs.Where(mo => mo.FacilityId == id).FirstOrDefault();

            return View(tOrder);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Update(Facility facility)
        {
            if (ModelState.IsValid)
            {
                DbSet<Facility> dbs = _dbContext.Facility;
                Facility tOrder = dbs.Where(mo => mo.FacilityId == facility.FacilityId)
                                     .FirstOrDefault();

                if (tOrder != null)
                {
                    tOrder.FacilityId = facility.FacilityId;
                    tOrder.AdminId = facility.AdminId;
                    tOrder.FacilityName = facility.FacilityName;
                    tOrder.PostalCode = facility.PostalCode;
                    tOrder.BlockNumber = facility.BlockNumber;
                    tOrder.StreetName = facility.StreetName;
                    tOrder.BannerPic = facility.BannerPic;

                    if (_dbContext.SaveChanges() == 1)
                        TempData["Msg"] = "Facility updated!";
                    else
                        TempData["Msg"] = "Failed to update database!";
                }
                else
                {
                    TempData["Msg"] = "Facility not found!";
                    return RedirectToAction("Index");
                }
            }

            else
            {
                TempData["Msg"] = "Invalid information entered";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            DbSet<Facility> dbs = _dbContext.Facility;

            Facility tOrder = dbs.Where(mo => mo.FacilityId == id).FirstOrDefault();

            if (tOrder != null)
            {
                dbs.Remove(tOrder);

                if (_dbContext.SaveChanges() == 1)
                    TempData["Msg"] = "Facility deleted!";
                else
                    TempData["Msg"] = "Failed to update database!";
            }
            else
            {
                TempData["Msg"] = "Facility not found!";
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Print(int id)
        {
            DbSet<Facility> dbs = _dbContext.Facility;
            List<Facility> model = dbs.ToList();

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
