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
    public class AccessPointController : Controller
    {
        private AppDbContext _dbContext;

        public AccessPointController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            DbSet<AccessPoint> dbs = _dbContext.AccessPoint;
            List<AccessPoint> model = dbs.ToList();
       
            return View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            DbSet<AccessPoint> dbs = _dbContext.AccessPoint;
            var lstPokes = dbs.ToList();
            ViewData["pokes"] = new SelectList(lstPokes, "FacilityId", "Name");
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(AccessPoint accessPoint)
        {
            if (ModelState.IsValid)
            {
                DbSet<AccessPoint> dbs = _dbContext.AccessPoint;
                dbs.Add(accessPoint);

                if (_dbContext.SaveChanges() == 1)
                    TempData["Msg"] = "New access point added!";
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
            DbSet<AccessPoint> dbs = _dbContext.AccessPoint;
            AccessPoint tOrder = dbs.Where(mo => mo.AccessPointId == id).FirstOrDefault();

                return View(tOrder);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Update(AccessPoint accessPoint)
        {
            if (ModelState.IsValid)
            {
                DbSet<AccessPoint> dbs = _dbContext.AccessPoint;
                AccessPoint tOrder = dbs.Where(mo => mo.AccessPointId == accessPoint.AccessPointId).FirstOrDefault();

                if (tOrder != null)
                {
                    tOrder.AccessPointId = accessPoint.AccessPointId;
                    tOrder.FacilityId = accessPoint.FacilityId;
                    tOrder.Description = accessPoint.Description;

                    if (_dbContext.SaveChanges() == 1)
                        TempData["Msg"] = "Access Point updated!";
                    else
                        TempData["Msg"] = "Failed to update database!";
                }

                else
                {
                    TempData["Msg"] = "Access Point not found!";
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
            DbSet<AccessPoint> dbs = _dbContext.AccessPoint;

            AccessPoint tOrder = dbs.Where(mo => mo.AccessPointId == id).FirstOrDefault();

            if (tOrder != null)
            {
                dbs.Remove(tOrder);

                if (_dbContext.SaveChanges() == 1)
                    TempData["Msg"] = "Access Point deleted!";
                else
                    TempData["Msg"] = "Failed to update database!";
            }
            else
            {
                TempData["Msg"] = "Access Point not found!";
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Print(int id)
        {
            DbSet<AccessPoint> dbs = _dbContext.AccessPoint;
            List<AccessPoint> model = dbs.ToList() ;

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
