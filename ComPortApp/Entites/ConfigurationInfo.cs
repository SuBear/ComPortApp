using System;

namespace ComPortApp.Entites
{
    public class ConfigurationInfo
    {
        private const int LatitudeMultiplierConst = 111111;

        public float StartLatitude { get; set; }
        public float StartLongitude { get; set; }
        public int StartHeight { get; set; }
        public float ObservationPointLatitude { get; set; }
        public float ObservationPointLongitude { get; set; }
        public int ObservationPointHeight { get; set; }
        public float MaxValidLatitude { get; set; }
        public float MaxValidLongitude { get; set; }
        public int LatitudeMultiplier 
        {
            get { return LatitudeMultiplierConst; }
        } 

        public int LongitudeMultiplier
        {
            get { return (int)Math.Cos(ObservationPointLatitude*Math.PI/180); }
        }
    }
}
