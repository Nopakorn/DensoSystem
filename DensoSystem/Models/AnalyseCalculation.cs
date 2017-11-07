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

namespace DensoSystem.Models
{
    public class AnalyseCalculation
    {
        public static List<PulseRateViewModel> PulseRateCalculation(AnalyseViewModel data)
        {
            using (var ctx = new DensoEntities())
            {
                var dateWeek = data.SelectDate.Date.AddDays(7);
                var dateMonth = data.SelectDate.Date.AddMonths(1);
                var dateEnd = data.SelectDate.Date;
                Debug.WriteLine($"start week { dateWeek }, month { dateMonth } <= end { dateEnd }");
                var model = new List<PulseRateViewModel>();
                switch (data.Chart)
                {
                    case ChartType.DayAnalyse:
                        model = ctx.Analyses.Where(x => DbFunctions.TruncateTime(x.CreateDate) == dateEnd)
                               .Select(s => new PulseRateViewModel
                               {
                                   Date = s.CreateDate.Value,
                                   PulseRate = s.PulseRate
                               }).ToList();
                        return model;
                    case ChartType.WeekAnalyse:
                        model = ctx.Analyses.Where(x => DbFunctions.TruncateTime(x.CreateDate) >= dateEnd && DbFunctions.TruncateTime(x.CreateDate) <= dateWeek)
                               .Select(s => new PulseRateViewModel
                               {
                                   Date = s.CreateDate.Value,
                                   PulseRate = s.PulseRate
                               }).ToList();
                        return model;
                    case ChartType.MonthAnalyse:
                        model = ctx.Analyses.Where(x => DbFunctions.TruncateTime(x.CreateDate) >= dateEnd && DbFunctions.TruncateTime(x.CreateDate) <= dateMonth)
                                .Select(s => new PulseRateViewModel
                                {
                                    Date = s.CreateDate.Value,
                                    PulseRate = s.PulseRate
                                }).ToList();
                        return model;
                    default:
                        return null;
                }
            }
        }
    }
}