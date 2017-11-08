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
    public class BloodPressureController : Controller
    {
        // GET: BloodPressure
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
                    return PartialView("BloodPressure", DayAnalyse(data));
                case ChartType.WeekAnalyse:
                    return PartialView("BloodPressure", WeekAnalyse(data));
                case ChartType.MonthAnalyse:
                    return PartialView("BloodPressure", MonthAnalyse(data));
                default:
                    return Content("Chart type Not founded");
            }
        }

        public IEnumerable<BloodPressureViewModel> DayAnalyse(AnalyseViewModel data)
        {
            using (var ctx = new DensoEntities())
            {
                var dateStart = data.SelectDate.Date;
                var timeRange = TimeHelper.TimeRange(dateStart);

                var model = ctx.Analyses.Where(x => DbFunctions.TruncateTime(x.CreateDate) == dateStart)
                              .Select(s => new BloodPressureViewModel
                              {
                                  Date = s.CreateDate.Value,
                                  Systolic = s.Systolic,
                                  Diastolic = s.Diastolic
                              }).OrderBy(c => c.Date).ToList();
                var list = timeRange.GroupJoin(model, r => r.Hour, m => m.Date.Hour, (r, m) => new { r, m })
                   .SelectMany(x => x.m.DefaultIfEmpty(), (x, m) => new BloodPressureViewModel
                   {
                       Date = m == null ? x.r : m.Date,
                       Systolic = m == null ? 0 : m.Systolic,
                       Diastolic = m == null ? 0 : m.Diastolic
                   }).OrderBy(o => o.Date).ToList();

                return list;
            }
        }
        public IEnumerable<BloodPressureViewModel> WeekAnalyse(AnalyseViewModel data)
        {
            using (var ctx = new DensoEntities())
            {
                var dateStart = data.SelectDate.Date;
                var dateWeek = data.SelectDate.Date.AddDays(7);
                var weekRange = TimeHelper.DateRange(dateStart, dateWeek);

                var model = ctx.Analyses.Where(x => DbFunctions.TruncateTime(x.CreateDate) >= dateStart && DbFunctions.TruncateTime(x.CreateDate) <= dateWeek)
                              .Select(s => new BloodPressureViewModel
                              {
                                  Date = s.CreateDate.Value,
                                  Systolic = s.Systolic,
                                  Diastolic = s.Diastolic
                              }).OrderBy(c => c.Date).ToList();
                var list = weekRange.GroupJoin(model, r => r.Date, m => m.Date.Date, (r, m) => new { r, m })
                    .SelectMany(x => x.m.DefaultIfEmpty(), (x, m) => new BloodPressureViewModel
                    {
                        Date = m == null ? x.r : m.Date,
                        Systolic = m == null ? 0 : m.Systolic,
                        Diastolic = m == null ? 0 : m.Diastolic
                    }).OrderBy(o => o.Date).ToList();

                return list;
            }
        }
        public IEnumerable<BloodPressureViewModel> MonthAnalyse(AnalyseViewModel data)
        {
            using (var ctx = new DensoEntities())
            {
                var dateStart = data.SelectDate.Date;
                var dateMonth = data.SelectDate.Date.AddMonths(1);
                var monthRange = TimeHelper.DateRange(dateStart, dateMonth);

                var model = ctx.Analyses.Where(x => DbFunctions.TruncateTime(x.CreateDate) >= dateStart && DbFunctions.TruncateTime(x.CreateDate) <= dateMonth)
                              .Select(s => new BloodPressureViewModel
                              {
                                  Date = s.CreateDate.Value,
                                  Systolic = s.Systolic,
                                  Diastolic = s.Diastolic
                              }).OrderBy(c => c.Date).ToList();
                var list = monthRange.GroupJoin(model, r => r.Date, m => m.Date.Date, (r, m) => new { r, m })
                    .SelectMany(x => x.m.DefaultIfEmpty(), (x, m) => new BloodPressureViewModel
                    {
                        Date = m == null ? x.r : m.Date,
                        Systolic = m == null ? 0 : m.Systolic,
                        Diastolic = m == null ? 0 : m.Diastolic
                    }).OrderBy(o => o.Date).ToList();

                return list;
            }
        }
    }
}