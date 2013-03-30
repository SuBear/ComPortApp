using System;

namespace ComPortApp
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string portName;
            if(args.Length < 1)
            {
                Console.WriteLine("Please enter the port name");
                portName = Console.ReadLine();
            }
            else
            {
                portName = args[0];
            }
            InitialDataProvider.InitializeConfigData();
            var portListener = new ComPortController(portName);
            portListener.Activate();
            Console.ReadLine();
            portListener.Deactivate();
        }
    }
}
