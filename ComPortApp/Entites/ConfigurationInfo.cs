using System;

namespace ComPortApp.Entites
{
    public class ConfigurationInfo
    {
        private const int LatitudeMultiplierConst = 111111;

        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public int StartHeight { get; set; }
        public double ObservationPointLatitude { get; set; }
        public double ObservationPointLongitude { get; set; }
        public int ObservationPointHeight { get; set; }
        public double MaxValidLatitude { get; set; }
        public double MaxValidLongitude { get; set; }
        public int LatitudeMultiplier 
        {
            get { return LatitudeMultiplierConst; }
        } 

        public int LongitudeMultiplier
        {
            get { return (int)(Math.Cos(ObservationPointLatitude * Math.PI / 180) * LatitudeMultiplier); }
        }
    }
}
