//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DensoSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sensor
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public int PulseWave1 { get; set; }
        public int PulseWave2 { get; set; }
        public int PulseWave3 { get; set; }
        public int PulseWave4 { get; set; }
        public int PulseWave5 { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public bool IsGenerate { get; set; }
    }
}
