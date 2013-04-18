using System;

namespace ComPortApp
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                string portName;
                if (args.Length < 1)
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
                InitKeyCommandsHandling(portListener);
                portListener.Deactivate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private static void InitKeyCommandsHandling(ComPortController controller)
        {
            var input = Console.ReadKey(true);
            switch (input.Key)
            {
                case ConsoleKey.Enter:
                    break;
                case ConsoleKey.R:
                    controller.Reset();
                    break;
                case ConsoleKey.LeftArrow:
                    controller.MoveLeft();
                    break;
                case ConsoleKey.RightArrow:
                    controller.MoveRight();
                    break;
                case ConsoleKey.UpArrow:
                    controller.MoveUp();
                    break;
                case ConsoleKey.DownArrow:
                    controller.MoveDown();
                    break;
            }
            InitKeyCommandsHandling(controller);
        }
    }
}
