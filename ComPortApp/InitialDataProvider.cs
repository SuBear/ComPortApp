using System;
using System.Configuration;
using System.Globalization;
using System.Net.Mime;
using ComPortApp.Entites;

namespace ComPortApp
{
    public static class InitialDataProvider
    {
        private const string StartLatitude = "StartLatitude";
        private const string StartLongitude = "StartLongitude";
        private const string StartHeight = "StartHeight";
        private const string ObservationPointLatitude = "ObservationPointLatitude";
        private const string ObservationPointLongitude = "ObservationPointLongitude";
        private const string ObservationPointHeight = "ObservationPointHeight";
        private const string MaxValidLatitude = "MaxValidLatitude";
        private const string MaxValidLongitude = "MaxValidLongitude";

        private const string InvalidParameterMessage = "{0} parameter is invalid. Please check config file";

        private static readonly ConfigurationInfo ConfigurationInfo = new ConfigurationInfo();

        public static ConfigurationInfo GetConfig()
        {
            return ConfigurationInfo;
        }

        public static void InitializeConfigData()
        {
            string value = ConfigurationManager.AppSettings[StartLatitude];
            float startLatitude;
            if (!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out startLatitude))
            {
                Console.WriteLine(InvalidParameterMessage, StartLatitude);
                CloseApplication();
            }
            else
            {
                ConfigurationInfo.StartLatitude = startLatitude;
            }
            value = ConfigurationManager.AppSettings[StartLongitude];
            float startLongitude;
            if (!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out startLongitude))
            {
                Console.WriteLine(InvalidParameterMessage, StartLongitude);
                CloseApplication();
            }
            else
            {
                ConfigurationInfo.StartLongitude = startLongitude;
            }
            value = ConfigurationManager.AppSettings[StartHeight];
            int startHeight;
            if (!int.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out startHeight))
            {
                Console.WriteLine(InvalidParameterMessage, StartHeight);
                CloseApplication();
            }
            else
            {
                ConfigurationInfo.StartHeight = startHeight;
            }
            value = ConfigurationManager.AppSettings[ObservationPointLatitude];
            float observationPointLatitude;
            if (!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out observationPointLatitude))
            {
                Console.WriteLine(InvalidParameterMessage, ObservationPointLatitude);
                CloseApplication();
            }
            else
            {
                ConfigurationInfo.ObservationPointLatitude = observationPointLatitude;
            }
            value = ConfigurationManager.AppSettings[ObservationPointLongitude];
            float observationPointLongitude;
            if (!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out observationPointLongitude))
            {
                Console.WriteLine(InvalidParameterMessage, ObservationPointLongitude);
                CloseApplication();
            }
            else
            {
                ConfigurationInfo.ObservationPointLongitude = observationPointLongitude;
            }
            value = ConfigurationManager.AppSettings[ObservationPointHeight];
            int observationPointHeight;
            if (!int.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out observationPointHeight))
            {
                Console.WriteLine(InvalidParameterMessage, ObservationPointHeight);
                CloseApplication();
            }
            else
            {
                ConfigurationInfo.ObservationPointHeight = observationPointHeight;
            }
            value = ConfigurationManager.AppSettings[MaxValidLatitude];
            float maxValidLatitude;
            if (!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out maxValidLatitude))
            {
                Console.WriteLine(InvalidParameterMessage, MaxValidLatitude);
                CloseApplication();
            }
            else
            {
                ConfigurationInfo.MaxValidLatitude = maxValidLatitude;
            }
            value = ConfigurationManager.AppSettings[MaxValidLongitude];
            float maxValidLongitude;
            if (!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out maxValidLongitude))
            {
                Console.WriteLine(InvalidParameterMessage, MaxValidLongitude);
                CloseApplication();
            }
            else
            {
                ConfigurationInfo.MaxValidLongitude = maxValidLongitude;
            }
        }

        private static void CloseApplication()
        {
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
