﻿using System;

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
            var portListener = new ComPortTranslator(portName);
            portListener.Activate();
            Console.ReadLine();
            portListener.Deactivate();
        }
    }
}
