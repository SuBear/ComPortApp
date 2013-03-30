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
            var firstLine = _port.ReadLine();
            var secondLine = _port.ReadLine();
            ProcessTranslatedResults(firstLine, secondLine);
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
        
        private void ProcessTranslatedResults(string firstLine, string secondLine)
        {
            using (var sw = new StreamWriter(_resultFileName))
            {
                _stringBuilder = _stringBuilder.AppendLine(firstLine);
                _stringBuilder = _stringBuilder.AppendLine(secondLine);
                sw.Write(_stringBuilder.ToString());
                var portDataParser = new PortDataParser();
                var parcedPortData = portDataParser.ParsePortData(firstLine, secondLine, _tableData);
                //string result = string.Format("Time: {0} Height: {1}", parcedPortData[0], parcedPortData[1]);
                //Console.WriteLine(result);
            }
        }
    }
}
