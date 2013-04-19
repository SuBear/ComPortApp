using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Text;
using ComPortApp.Entites;

namespace ComPortApp
{
    public class ComPortController
    {
        private readonly SerialPort _port = new SerialPort(
            "COM1", 9600, Parity.None, 8, StopBits.One);

        //private FakePort _fakePort;

        private readonly IDictionary<int, int> _tableData = new Dictionary<int, int>();
        private StringBuilder _stringBuilder = new StringBuilder();
        private readonly string _portDataFileName = string.Format("portData{0}.txt", 
            DateTime.UtcNow).Replace(" ", "").Replace(":", "_");
        private readonly string _translatedDataFileName = string.Format("translatedData{0}.txt",
            DateTime.UtcNow).Replace(" ", "").Replace(":", "_");

        private byte[] _lastDataSent = new byte[] {0, 0, 0};
        private ParsedPortInfo _lastParsedPortInfo = new ParsedPortInfo();

        private readonly AngleValuesProvider _angleValuesProvider = new AngleValuesProvider();

        public ComPortController(string portName)
        {
            ParseDataTable();
            _port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
            Console.WriteLine("Connectign to port " + portName);
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
                Console.WriteLine("Port have failed to open with following message: " + ex.Message);
            }
        }

        public void Deactivate()
        {
            _port.Close();
        }
        
        public void Reset()
        {
            SendBytesToPort(new byte[] { 0, 0, 1 });
        }

        public void MoveLeft()
        {
            _lastDataSent[1] = _lastDataSent[1] <= 92 || _lastDataSent[1] >= 166 
                ? (byte)(_lastDataSent[1] - 2) : _lastDataSent[1];
            SendBytesToPort(_lastDataSent);
        }

        public void MoveRight()
        {
            _lastDataSent[1] = _lastDataSent[1] <= 90 || _lastDataSent[1] >= 164 
                ? (byte)(_lastDataSent[1] + 2) : _lastDataSent[1];
            SendBytesToPort(_lastDataSent);
        }

        public void MoveUp()
        {
            _lastDataSent[0] = _lastDataSent[0] <= 48 ? (byte)(_lastDataSent[0] + 2) : _lastDataSent[0];
            SendBytesToPort(_lastDataSent);
        }

        public void MoveDown()
        {
            _lastDataSent[0] = _lastDataSent[0] >= 2 ? (byte)(_lastDataSent[0] - 2) : _lastDataSent[0];
            SendBytesToPort(_lastDataSent);
        }

        private void InitPortListerning()
        {
            _port.DataReceived += _port_DataReceived;
            //_fakePort = new FakePort(1000);
            //_fakePort.DataReceived += _fakePort_DataReceived;
        }

        //private void _fakePort_DataReceived(object sender, EventArgs e)
        //{
        //    Console.WriteLine("Got it!");
        //}

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
            using (var sw = new StreamWriter(_portDataFileName, true))
            {
                _stringBuilder = _stringBuilder.AppendLine(firstLine);
                _stringBuilder = _stringBuilder.AppendLine(secondLine);
                sw.WriteLine(_stringBuilder.ToString());
                _stringBuilder.Clear();
                var portDataParser = new PortDataParser();
                var parcedPortData = portDataParser.ParsePortData(firstLine, secondLine, _tableData);
                var currentResultIsValid = _angleValuesProvider.ValidateParsedInfo(parcedPortData);
                TranslateResults(parcedPortData, currentResultIsValid);
            }
        }

        private void TranslateResults(ParsedPortInfo parsedPortInfo, bool infoIsValid)
        {
            var bytesToSend = new byte[3];
            var angleValues = _angleValuesProvider.GetAngleValues(parsedPortInfo.Height, infoIsValid);
            _lastParsedPortInfo = parsedPortInfo;
            for (int i = 0; i < 2; i++)
            {
                bytesToSend[i] = angleValues[i];
            }
            bytesToSend[2] = 0;
            _lastDataSent[0] = bytesToSend[0];
            if (infoIsValid)
            {
                _lastDataSent[1] = bytesToSend[1];
            }
            SendBytesToPort(bytesToSend);
        }

        private void SendBytesToPort(byte[] bytesToSend)
        {
            if (!_port.IsOpen)
            {
                _port.Open();
            }
            _port.Write(bytesToSend, 0, 3);
            LogPortDataSending(bytesToSend);
        }

        private void LogPortDataSending(byte[] bytesToSend)
        {
            var timeString = _lastParsedPortInfo.TimeStamp.ToString("HH:mm:ss");
            string result = string.Format("Time: {0} Height: {1} Latitude: {2} Longitude: {3} Altitude: {4}",
                timeString, _lastParsedPortInfo.Height, _lastParsedPortInfo.Latitude,
                _lastParsedPortInfo.Longitude, _lastParsedPortInfo.Altitude);
            Console.WriteLine(result);
            using (var sw = new StreamWriter(_translatedDataFileName, true))
            {
                var bytesToSendList = new List<byte>(bytesToSend);
                bytesToSendList.ForEach(x => _stringBuilder.Append(x.ToString(CultureInfo.InvariantCulture) + " "));
                var bytesToSendString = _stringBuilder.ToString();
                _stringBuilder.Clear();
                _stringBuilder = _stringBuilder.AppendLine(result + " Bytes Sent To Controller: " + bytesToSendString);
                sw.Write(_stringBuilder.ToString());
                _stringBuilder.Clear();
            }
        }
    }
}
