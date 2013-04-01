using System;
using ComPortApp.Entites;

namespace ComPortApp
{
    public class AngleValuesProvider
    {
        private const double Epsilon = 0.00001;
        private const int MaxAngle = 50;

        private CoordinatesDifferenceInfo _validCoordinatesDifferenceInfo;
        private int _previoulySentAngle = 0;

        public byte[] GetAngleValues(int height, bool infoIsValid)
        {
            var retVal = new byte[2];
            var configuration = InitialDataProvider.GetConfig();
            byte firstAngle;
            byte secondAngle;
            double latitudeDifference;
            double longitudeDifference;
            int currentHeight;
            if (_validCoordinatesDifferenceInfo != null)
            {
                latitudeDifference = _validCoordinatesDifferenceInfo.LatitudeDifference * configuration.LatitudeMultiplier;
                longitudeDifference = _validCoordinatesDifferenceInfo.LongitudeDifference * configuration.LongitudeMultiplier;
                currentHeight = _validCoordinatesDifferenceInfo.Altitude;
            }
            else
            {
                latitudeDifference = (configuration.StartLatitude - configuration.ObservationPointLatitude)
                    * configuration.LatitudeMultiplier;
                longitudeDifference = (configuration.StartLongitude - configuration.ObservationPointLongitude)
                    * configuration.LongitudeMultiplier;
                currentHeight = configuration.StartHeight;
            }
            if (Math.Abs(latitudeDifference - 0) > Epsilon
                    || Math.Abs(longitudeDifference - 0) > Epsilon)
            {
                var altitude = infoIsValid ? currentHeight : height;
                var firstAngleDouble = Math.Atan(altitude / Math.Sqrt(Math.Pow(latitudeDifference, 2)
                    + Math.Pow(longitudeDifference, 2)));
                firstAngle = (byte)Math.Round(firstAngleDouble * 100 / Math.PI);
                var startLatitudeDifference = (configuration.StartLatitude - configuration.ObservationPointLatitude) * configuration.LatitudeMultiplier;
                var startLongitudeDifference = (configuration.StartLongitude - configuration.ObservationPointLongitude) * configuration.LongitudeMultiplier;
                var secondAngleIsPositive = (longitudeDifference
                                             * (startLatitudeDifference) - latitudeDifference
                                             * (startLongitudeDifference)) > 0;
                var secondTopSum = latitudeDifference * startLatitudeDifference
                                   + longitudeDifference * startLongitudeDifference;
                var secondBotSup = Math.Sqrt(Math.Pow(latitudeDifference, 2)
                    + Math.Pow(longitudeDifference, 2))
                    * Math.Sqrt(Math.Pow(startLatitudeDifference, 2) + Math.Pow(startLongitudeDifference, 2));
                var secondAngleDoulble = Math.Acos(secondTopSum/secondBotSup);
                if (!secondAngleIsPositive)
                {
                    secondAngleDoulble = -secondAngleDoulble;
                }
                secondAngle = (byte)Math.Round(secondAngleDoulble * 92 / Math.PI);
            }
            else
            {
                firstAngle = MaxAngle;
                secondAngle = (byte)_previoulySentAngle;
            }
            retVal[0] = firstAngle;
            retVal[1] = secondAngle;
            return retVal;
        }

        public bool ValidateParsedInfo(ParsedPortInfo parsedInfo)
        {
            bool retVal = false;
            var configuration = InitialDataProvider.GetConfig();
            var latitudeDifference = parsedInfo.Latitude - configuration.ObservationPointLatitude;
            var longitudeDifference = parsedInfo.Longitude - configuration.ObservationPointLongitude;
            var latitudeIsValid = latitudeDifference <= configuration.MaxValidLatitude;
            var longitudeIsValid = longitudeDifference <= configuration.MaxValidLongitude;
            int altitude;
            if (parsedInfo.Altitude > configuration.StartHeight)
            {
                altitude = parsedInfo.Altitude;
            }
            else
            {
                altitude = parsedInfo.Height < configuration.StartHeight ? configuration.StartHeight : parsedInfo.Height;
            }
            if (latitudeIsValid && longitudeIsValid)
            {
                retVal = true;
                _validCoordinatesDifferenceInfo = new CoordinatesDifferenceInfo
                    {
                        Altitude = altitude,
                        LatitudeDifference = latitudeDifference,
                        LongitudeDifference = longitudeDifference
                    };
            }
            return retVal;
        }
    }
}
