using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DensoSystem.Models;
using DensoSystem.Helpers;
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
            switch (data.Chart)
            {
                case ChartType.DayAnalyse:
                    return PartialView("PulseRate", DayAnalyse(data));
                case ChartType.WeekAnalyse:
                    return PartialView("PulseRate", WeekAnalyse(data));
                case ChartType.MonthAnalyse:
                    return PartialView("PulseRate", MonthAnalyse(data));
                default:
                    return Content("Chart type Not founded");
            }
        }

        public IEnumerable<PulseRateViewModel> DayAnalyse(AnalyseViewModel data)
        {
            using (var ctx = new DensoEntities())
            {
                var dateStart = data.SelectDate.Date;
                var timeRange = TimeHelper.TimeRange(dateStart);

                var model = ctx.Analyses.Where(x => DbFunctions.TruncateTime(x.CreateDate) == dateStart)
                              .Select(s => new PulseRateViewModel
                              {
                                  Date = s.CreateDate.Value,
                                  PulseRate = s.PulseRate
                              }).OrderBy(c => c.Date).ToList();
                var list = timeRange.GroupJoin(model, r => r.Hour, m => m.Date.Hour, (r, m) => new { r, m })
                   .SelectMany(x => x.m.DefaultIfEmpty(), (x, m) => new PulseRateViewModel
                   {
                       Date = m == null ? x.r : m.Date,
                       PulseRate = m == null ? 0.0 : m.PulseRate,
                   }).OrderBy(o => o.Date).ToList();

                return list;
            }
        }

        public IEnumerable<PulseRateViewModel> WeekAnalyse(AnalyseViewModel data)
        {
            using (var ctx = new DensoEntities())
            {
                var dateStart = data.SelectDate.Date;
                var dateWeek = data.SelectDate.Date.AddDays(7);
                var weekRange = TimeHelper.DateRange(dateStart, dateWeek);

                var model = ctx.Analyses.Where(x => DbFunctions.TruncateTime(x.CreateDate) >= dateStart && DbFunctions.TruncateTime(x.CreateDate) <= dateWeek)
                              .Select(s => new PulseRateViewModel
                              {
                                  Date = s.CreateDate.Value,
                                  PulseRate = s.PulseRate
                              }).OrderBy(c => c.Date).ToList();
                var list = weekRange.GroupJoin(model, r => r.Date, m => m.Date.Date, (r, m) => new { r, m })
                    .SelectMany(x => x.m.DefaultIfEmpty(), (x, m) => new PulseRateViewModel
                    {
                        Date = m == null ? x.r : m.Date,
                        PulseRate = m == null ? 0.0 : m.PulseRate,
                    }).OrderBy(o => o.Date).ToList();

                return list;
            }
        }

        public IEnumerable<PulseRateViewModel> MonthAnalyse(AnalyseViewModel data)
        {
            using (var ctx = new DensoEntities())
            {
                var dateStart = data.SelectDate.Date;
                var dateMonth = data.SelectDate.Date.AddMonths(1);
                var monthRange = TimeHelper.DateRange(dateStart, dateMonth);

                var model = ctx.Analyses.Where(x => DbFunctions.TruncateTime(x.CreateDate) >= dateStart && DbFunctions.TruncateTime(x.CreateDate) <= dateMonth)
                              .Select(s => new PulseRateViewModel
                              {
                                  Date = s.CreateDate.Value,
                                  PulseRate = s.PulseRate
                              }).OrderBy(c => c.Date).ToList();
                var list = monthRange.GroupJoin(model, r => r.Date, m => m.Date.Date, (r, m) => new { r, m })
                    .SelectMany(x => x.m.DefaultIfEmpty(), (x, m) => new PulseRateViewModel
                    {
                        Date = m == null ? x.r : m.Date,
                        PulseRate = m == null ? 0.0 : m.PulseRate,
                    }).OrderBy(o => o.Date).ToList();

                return list;
            }
        }
    }
}