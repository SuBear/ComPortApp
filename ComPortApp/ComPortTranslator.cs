using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;

namespace ComPortApp
{
    public class ComPortTranslator
    {
        private readonly SerialPort _port = new SerialPort(
            "COM1", 9600, Parity.None, 8, StopBits.One);

        private readonly IDictionary<int, int> _tableData = new Dictionary<int, int>();
        private StringBuilder _stringBuilder = new StringBuilder();
        private readonly string _resultFileName = string.Format("result{0}.txt", 
            DateTime.UtcNow).Replace(" ", "").Replace(":", "_");

        public ComPortTranslator(string portName)
        {
            ParseDataTable();
            _port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
            InitPortListerning();
        }

        public void Activate()
        {
            try
            {
                _port.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Port have failed to open with following message" + ex.Message);
            }
        }

        public void Deactivate()
        {
            _port.Close();
        }
        
        private void InitPortListerning()
        {
            _port.DataReceived += _port_DataReceived;
        }

        private void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            PrintTranslatedResults(_port.ReadExisting());
        }

        private void ParseDataTable()
        {
            string line;

            var file = new StreamReader("excel.txt");
            while ((line = file.ReadLine()) != null)
            {
                var delimiters = new [] { '	', ' ' };
                var stringArray = line.Split(delimiters);
                int pressure;
                int height;
                int.TryParse(stringArray[0], out pressure);
                int.TryParse(stringArray[1], out height);
                _tableData.Add(pressure, height);
            }

            file.Close();
        }
        
        private void PrintTranslatedResults(string portData)
        {
            using (var sw = new StreamWriter(_resultFileName))
            {
                _stringBuilder = _stringBuilder.AppendLine(portData);
                sw.Write(_stringBuilder.ToString());
                string[] parcedPortData = ParsePortData(portData);
                string result = string.Format("Time: {0} Height: {1}", parcedPortData[0], parcedPortData[1]);
                Console.WriteLine(result);
            }
        }

        private string[] ParsePortData(string portData)
        {
            var replacedData = portData.Trim();
            replacedData = replacedData.Replace(" ", "");
            replacedData = replacedData.Replace("\r", "");
            replacedData = replacedData.Replace("Time-", "");
            var stringArray = new string[1];
            stringArray[0] = "Pressure-";
            var parsedArray = replacedData.Split(stringArray, StringSplitOptions.None);
            int pressure = int.Parse(parsedArray[1]);
            if (pressure > 836)
            {
                parsedArray[1] = "0";
            }
            else if (pressure < 6)
            {
                parsedArray[1] = "16000";
            }
            else
            {
                parsedArray[1] = _tableData[pressure].ToString();
            }
            return parsedArray;
        }
    }
}
