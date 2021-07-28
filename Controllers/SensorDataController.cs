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
    public class SensorDataController : Controller
    {
        private AppDbContext _dbContext;

        public SensorDataController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            DbSet<SensorData> dbs = _dbContext.SensorData;
            List<SensorData> model = dbs.ToList();

            return View(model);
        }


        [Authorize]
        public IActionResult Print(int id)
        {
            DbSet<SensorData> dbs = _dbContext.SensorData;
            List<SensorData> model = dbs.ToList();

            if (model != null)
                return new ViewAsPdf(model)
                {
                    PageSize = Rotativa.AspNetCore.Options.Size.B5,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
                };
            else
            {
                TempData["Msg"] = "Sensor Data not found!";
                return RedirectToAction("Index");
            }

        }

    }
}
