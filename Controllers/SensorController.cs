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
    public class SensorController : Controller
    {
        private AppDbContext _dbContext;

        public SensorController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            DbSet<Sensor> dbs = _dbContext.Sensor;
            List<Sensor> model = dbs.ToList();

            return View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            DbSet<Sensor> dbs = _dbContext.Sensor;
            var lstPokes = dbs.ToList();
            ViewData["pokes"] = new SelectList(lstPokes, "FacilityId", "Name");
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(Sensor sensor)
        {
            if (ModelState.IsValid)
            {
                DbSet<Sensor> dbs = _dbContext.Sensor;
                dbs.Add(sensor);

                if (_dbContext.SaveChanges() == 1)
                    TempData["Msg"] = "New Sensor added!";
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
            DbSet<Sensor> dbs = _dbContext.Sensor;
            Sensor tOrder = dbs.Where(mo => mo.SensorId == id).FirstOrDefault();

            return View(tOrder);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Update(Sensor sensor)
        {
            if (ModelState.IsValid)
            {
                DbSet<Sensor> dbs = _dbContext.Sensor;
                Sensor tOrder = dbs.Where(mo => mo.SensorId == sensor.SensorId)
                                     .FirstOrDefault();

                if (tOrder != null)
                {
                    tOrder.SensorId = sensor.SensorId;
                    tOrder.AccessPointId = sensor.AccessPointId;
                    tOrder.SensorName = sensor.SensorName;

                    if (_dbContext.SaveChanges() == 1)
                        TempData["Msg"] = "Sensor updated!";
                    else
                        TempData["Msg"] = "Failed to update database!";
                }
                else
                {
                    TempData["Msg"] = "Sensor not found!";
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
            DbSet<Sensor> dbs = _dbContext.Sensor;

            Sensor tOrder = dbs.Where(mo => mo.SensorId == id).FirstOrDefault();

            if (tOrder != null)
            {
                dbs.Remove(tOrder);

                if (_dbContext.SaveChanges() == 1)
                    TempData["Msg"] = "Sensor deleted!";
                else
                    TempData["Msg"] = "Failed to update database!";
            }
            else
            {
                TempData["Msg"] = "Sensor not found!";
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Print(int id)
        {
            DbSet<Sensor> dbs = _dbContext.Sensor;
            List<Sensor> model = dbs.ToList();

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
