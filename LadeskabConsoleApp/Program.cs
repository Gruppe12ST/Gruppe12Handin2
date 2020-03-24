using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LadeskabClassLibrary;

namespace LadeskabConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IRFIDReader rfidReader = new RFIDReader();
            IDateTimeProvider dateTimeProvider = new DateTimeProvider();
                
            IDoor door = new Door();
            IUSBCharger usbCharger = new USBChargerSimulator();
            IDisplay display = new Display();
            ILogFile logfile = new LogFile(dateTimeProvider);
            IChargeControl chargeControl = new ChargeControl(usbCharger,display);

            StationControl stationControl = new StationControl(rfidReader,door,chargeControl,display,logfile);

            bool done = false;

            do
            {
                Console.WriteLine("Indtast e, o, c, r");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "e":
                        done = true;
                        break;
                    case "o":
                        door.SetDoor(true); 
                        break;
                    case "c":
                        door.SetDoor(false); 
                        break;
                    case "r":
                        Console.WriteLine("Indtast RFID: ");
                        string id = Console.ReadLine();
                        rfidReader.SetRfidId(Convert.ToInt16(id));
                        break;
                }

            } while (!done);
        }
    }
}
