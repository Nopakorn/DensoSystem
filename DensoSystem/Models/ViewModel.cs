using System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace DensoSystem.Models
{
    public class AnalyseViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime SelectDate { get; set; }

        [Required]
        public ChartType? Chart { get; set; }
    }

    public enum ChartType
    {
        [Display(Name = @"Day")]
        DayAnalyse,

        [Display(Name = @"Week")]
        WeekAnalyse,

        [Display(Name = @"Month")]
        MonthAnalyse
    }

    public class PulseRateViewModel
    {
        public DateTime Date { get; set; }

        public double PulseRate { get; set; }

    }

    public class BloodPressureViewModel
    {
        public DateTime Date { get; set; }

        public decimal Systolic { get; set; }
        public decimal Diastolic { get; set; }

    }
}