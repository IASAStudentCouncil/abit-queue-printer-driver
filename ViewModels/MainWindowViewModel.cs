using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using ReactiveUI;
using abit_queue_printer_driver.Models;

namespace abit_queue_printer_driver.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            PrintTestCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                try
                {
                    var pd = new PrinterService(SelectedPrinter);
                    await pd.Print("42", "Не кнопка");
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
                // await pd.Print("42");
            });

            ButtonCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                try
                {
                    var pd = new PrinterService(SelectedPrinter);
                    await pd.Print(QueueNumber.ToString(), "Не кнопка");
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
                // await pd.Print("42");
            });

            RefreshCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                try
                {
                    Console.WriteLine("Refresh");
                    _serialService = new SerialService(SelectedSerialPort, SelectedPrinter);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            });
        }

        private SerialService _serialService;

        private List<string>? _printersList;

        public List<string> PrintersList
        {
            get
            {
                _printersList ??= PrinterService.GetPrinters();
                return _printersList;
            }
            set { }
        }

        private string? _selectedPrinter;

        public string SelectedPrinter
        {
            get
            {
                if (_selectedPrinter is null ||
                    !PrintersList.Contains(_selectedPrinter) && PrintersList.Count > 0)
                {
                    _selectedPrinter = PrintersList.FirstOrDefault();
                }

                return _selectedPrinter;
            }
            set => _selectedPrinter = value;
        }

        private List<string>? _serialPortsList;

        public List<string> SerialPortsList
        {
            get
            {
                _serialPortsList ??= SerialService.GetPortNames();
                return _serialPortsList;
            }
            set { }
        }

        private string? _selectedSerialPort;

        public string SelectedSerialPort
        {
            get
            {
                if (_selectedSerialPort is null ||
                    !SerialPortsList.Contains(_selectedSerialPort) && SerialPortsList.Count > 0)
                {
                    _selectedSerialPort = SerialPortsList.FirstOrDefault();
                }

                return _selectedSerialPort;
            }
            set => _selectedSerialPort = value;
        }

        public ReactiveCommand<Unit, Unit> PrintTestCommand { get; }
        public ReactiveCommand<Unit, Unit> ButtonCommand { get; }
        public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

        private string _apiAdress = "";

        public string ApiAdress
        {
            get => _apiAdress;
            set => _apiAdress = value;
        }

        private int _queueNumber = 1;

        public int QueueNumber
        {
            get => _queueNumber;
            set => _queueNumber = value;
        }
    }
}