using System;
using System.IO.Ports;

namespace Serial
{
    class Program
    {
        private static string _PortName;
        private static SerialPort _SerialPort;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowArgs();
                return;
            }

            switch (args[0].ToLower())
            {
                case "/list":
                    List();
                    break;
                case "/read":
                    if (args.Length != 2)
                    {
                        Console.WriteLine("Error - missing PortName argument. E.g. /read COM3");
                        return;
                    }
                    Read(args[1]);
                    break;
                default:
                    Console.WriteLine("Error - unknown switch.");
                    ShowArgs();
                    break;
            }
        }

        static void ShowArgs()
        { 
            Console.WriteLine("\nValid arguments:");
            Console.WriteLine("/List - Displays list of serial ports on system");
            Console.WriteLine("/Read - Reads incoming data on the port");
        }

        static void List()
        {
            string[] ArrayComPortsNames = null;
            int index = -1;
            string ComPortName = null;

            ArrayComPortsNames = SerialPort.GetPortNames();
            
            do
            {
                index += 1;
                Console.WriteLine("{0}", ArrayComPortsNames[index]);
            }
            while (!((ArrayComPortsNames[index] == ComPortName) ||
                                (index == ArrayComPortsNames.GetUpperBound(0))));
        }
        
        static void Read(string portName)
        {
            Console.WriteLine("Press ESC to exit.");

            try
            {
                _PortName = portName;
                _SerialPort = new SerialPort(_PortName, 9600, Parity.None, 8, StopBits.One);
                _SerialPort.DataReceived += SerialPort_DataReceived;
                _SerialPort.Open();
            }
            catch
            {
                Console.WriteLine("Error connecting to port {0}. Is another application already connected?", _PortName);
                return;
            }

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                // Read data
            }

            _SerialPort.Close();
        }

        static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine("Incoming data: {0}", _SerialPort.ReadExisting());
        }
    }
}
