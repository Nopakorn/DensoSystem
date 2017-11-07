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
    public class PulseRateController : Controller
    {
        // GET: PulseRate
        public ActionResult Index()
        {
            var model = new AnalyseViewModel();
            model.SelectDate = DateTime.Now;
            return View(model);
        }

        public ActionResult Generate(AnalyseViewModel data)
        {
            return  PartialView("PulseRate", PulseRateCalculation(data));
        }

        public List<PulseRateViewModel> PulseRateCalculation(AnalyseViewModel data)
        {
            using (var ctx = new DensoEntities())
            {
                var dateWeek = data.SelectDate.Date.AddDays(7);
                var dateMonth = data.SelectDate.Date.AddMonths(1);
                var dateStart = data.SelectDate.Date;
                var model = new List<PulseRateViewModel>();
                switch (data.Chart)
                {
                    case ChartType.DayAnalyse:
                        model = ctx.Analyses.Where(x => DbFunctions.TruncateTime(x.CreateDate) == dateStart)
                               .Select(s => new PulseRateViewModel
                               {
                                   Date = s.CreateDate.Value,
                                   PulseRate = s.PulseRate
                               }).OrderBy(c => c.Date).ToList();
                        return model;
                    case ChartType.WeekAnalyse:
                        model = ctx.Analyses.Where(x => DbFunctions.TruncateTime(x.CreateDate) >= dateStart && DbFunctions.TruncateTime(x.CreateDate) <= dateWeek)
                               .Select(s => new PulseRateViewModel
                               {
                                   Date = s.CreateDate.Value,
                                   PulseRate = s.PulseRate
                               }).OrderBy(c => c.Date).ToList();
                        return model;
                    case ChartType.MonthAnalyse:
                        model = ctx.Analyses.Where(x => DbFunctions.TruncateTime(x.CreateDate) >= dateStart && DbFunctions.TruncateTime(x.CreateDate) <= dateMonth)
                                .Select(s => new PulseRateViewModel
                                {
                                    Date = s.CreateDate.Value,
                                    PulseRate = s.PulseRate
                                }).OrderBy(c => c.Date).ToList();
                        return model;
                    default:
                        return null;
                }
            }
        }
    }
}