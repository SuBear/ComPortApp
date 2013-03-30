using System;

namespace ComPortApp.Entites
{
    public class ParsedPortInfo
    {
        public int Height { get; set; }
        public DateTime TimeStamp { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int Altitude { get; set; }
    }
}
