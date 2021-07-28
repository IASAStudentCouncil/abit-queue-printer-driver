using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;

namespace abit_queue_printer_driver.Models
{
    public class SerialService
    {
        private SerialPort _serialPort;
        private string pn;
        public SerialService(string portName, string printerName)
        {
            pn = printerName;
            Console.WriteLine("Make serial port");
            _serialPort = new SerialPort();
            _serialPort.DataReceived += DataReceivedHandler;
            _serialPort.PortName = portName;
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.One;
            _serialPort.DataBits = 8;
            _serialPort.Handshake = Handshake.None;
            Console.WriteLine(_serialPort);
            Console.WriteLine("Open serial port");
            _serialPort.Open();
            
        }
        public static List<string> GetPortNames()
        {
            return SerialPort.GetPortNames().ToList();
        }
        
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs ev)
        {
            try
            {
                var sp = (SerialPort) sender;
                var indata = sp.ReadExisting();
                // string number = wc.UploadString("http://" + textBox1.Text + "/api/queue", "");
                Console.WriteLine("Try to print");
                var pd = new PrinterService(pn);
                pd.Print("13", "Кнопка");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}