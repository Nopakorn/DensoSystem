using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DensoSystem.Models;
using System.Diagnostics;
using System.Data.Entity;
using Newtonsoft.Json;

namespace DensoSystem.Controllers
{
    public class AnalyseController : Controller
    {
        // GET: Analyse
        public ActionResult Index()
        {
            var model = new AnalyseViewModel();
            model.SelectDate = DateTime.Now;

            return View(model);
        }

        public ActionResult Generate(AnalyseViewModel data)
        {
            switch (data.Chart)
            {
                case ChartType.DayAnalyse:
                    return RedirectToAction("DayAnalyse", data);
                case ChartType.WeekAnalyse:
                    return RedirectToAction("WeekAnalyse", data);
                case ChartType.MonthAnalyse:
                    return RedirectToAction("MonthAnalyse", data);
                default:
                    return Content("not founded");
            }
        }

        public ActionResult DayAnalyse(AnalyseViewModel data)
        {
            using (var ctx = new DensoEntities())
            {
                var dateStartw = data.SelectDate.Date.AddDays(-7);
                var dateStartm = data.SelectDate.Date.AddMonths(-1);
                var dateEnd = data.SelectDate.Date;
                Debug.WriteLine($"start week { dateStartw }, month { dateStartm } <= end { dateEnd }");
                var model = ctx.Analyses.Where(x => DbFunctions.TruncateTime(x.CreateDate) == data.SelectDate.Date)
                    .Select(s => new PulseRateViewModel
                    {
                        Date = s.CreateDate.Value,
                        PulseRate = s.PulseRate
                    }).ToList();

                return PartialView("Analyse", model);
            }
        }

        public ActionResult WeekAnalyse(AnalyseViewModel data)
        {
            using (var ctx = new DensoEntities())
            {
                var dateStart = data.SelectDate.Date.AddDays(-7);
                var dateEnd = data.SelectDate.Date;
                var model = ctx.Analyses.Where(x => DbFunctions.TruncateTime(x.CreateDate) >= dateStart && DbFunctions.TruncateTime(x.CreateDate) <= dateEnd)
                    .Select(s => new PulseRateViewModel
                    {
                        Date = s.CreateDate.Value,
                        PulseRate = s.PulseRate
                    }).ToList();

                return PartialView("Analyse", model);
            }
        }

        public ActionResult MonthAnalyse(AnalyseViewModel data)
        {
            using (var ctx = new DensoEntities())
            {
                var dateStart = data.SelectDate.Date.AddMonths(-1);
                var dateEnd = data.SelectDate.Date;
                var model = ctx.Analyses.Where(x => DbFunctions.TruncateTime(x.CreateDate) >= dateStart && DbFunctions.TruncateTime(x.CreateDate) <= dateEnd)
                    .Select(s => new PulseRateViewModel
                    {
                        Date = s.CreateDate.Value,
                        PulseRate = s.PulseRate
                    }).ToList();

                return PartialView("Analyse", model);
            }
        }
    }
}