using System;

namespace ComPortApp.Entites
{
    public class ParsedPortInfo
    {
        public int Height { get; set; }
        public DateTime TimeStamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Altitude { get; set; }
    }
}
