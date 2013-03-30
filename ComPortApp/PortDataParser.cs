using System;
using System.Collections.Generic;
using System.Globalization;
using ComPortApp.Entites;

namespace ComPortApp
{
    public class PortDataParser
    {
        public ParsedPortInfo ParsePortData(string firstLine, string secondLine, IDictionary<int, int> tableData)
        {
            var retVal = new ParsedPortInfo {Height = GetParsedHeight(firstLine, tableData)};
            var parsedSecondLineArray = secondLine.Split(',');
            retVal.TimeStamp = GetTimeStamp(parsedSecondLineArray[1]);
            retVal.Latitude = GetLatitude(parsedSecondLineArray[2]);
            retVal.Longitude = GetLongitude(parsedSecondLineArray[4]);
            retVal.Altitude = GetAltutude(parsedSecondLineArray[9]);
            return retVal;
        }

        private int GetParsedHeight(string firstLine, IDictionary<int, int> tableData)
        {
            var retVal = 0;
            try
            {
                int pressure;
                var stringArray = firstLine.Trim().Split(',');
                var resultString = stringArray[1];
                resultString = resultString.Replace("Pressure=", string.Empty);
                resultString = resultString.Split('*')[0];
                Int32.TryParse(resultString, out pressure);
                if (pressure > 836)
                {
                    retVal = 0;
                }
                else if (pressure < 6)
                {
                    retVal = 16000;
                }
                else
                {
                    retVal = tableData[pressure];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return retVal;
        }

        private DateTime GetTimeStamp(string timeString)
        {
            var retVal = DateTime.UtcNow;
            try
            {
                timeString = timeString.Split('.')[0];
                retVal = DateTime.ParseExact(timeString, "HHmmss", null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return retVal;
        }

        private float GetLatitude(string latitudeString)
        {
            float retVal = 0;
            try
            {
                float.TryParse(latitudeString.Substring(0, 2), NumberStyles.Float, CultureInfo.InvariantCulture, out retVal);
                float partitivePart;
                float.TryParse(latitudeString.Substring(2), NumberStyles.Float, CultureInfo.InvariantCulture, out partitivePart);
                retVal = retVal + partitivePart / 60;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return retVal;
        }

        private float GetLongitude(string longitudeString)
        {
            float retVal = 0;
            try
            {
                float.TryParse(longitudeString.Substring(0, 3), NumberStyles.Float, CultureInfo.InvariantCulture, out retVal);
                float partitivePart;
                float.TryParse(longitudeString.Substring(3), NumberStyles.Float, CultureInfo.InvariantCulture, out partitivePart);
                retVal = retVal + partitivePart / 60;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return retVal;
        }

        private int GetAltutude(string altitudeString)
        {
            float retVal = 0;
            try
            {
                float.TryParse(altitudeString, NumberStyles.Float, CultureInfo.InvariantCulture, out retVal);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (int)retVal;
        }
    }
}
