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

            Console.WriteLine("Kontrol menu:");
            Console.WriteLine("-------------------");
            Console.WriteLine("[E]    Exit programmet");
            Console.WriteLine("[O]    Åben døren");
            Console.WriteLine("[C]    Luk døren");
            Console.WriteLine("[R]    Indlæs RFID-tag");


            do
            {
                

                string input = Console.ReadLine();

                switch (input)
                {
                    case "e":
                    case "E":
                        done = true;
                        break;
                    case "o":
                    case "O":
                        door.SetDoor(true); 
                        break;
                    case "c":
                    case "C":
                        door.SetDoor(false);
                        break;
                    case "r":
                    case "R":
                        Console.WriteLine("Indtast RFID: ");
                        string id = Console.ReadLine();
                        rfidReader.SetRfidId(Convert.ToInt32(id));
                        break;
                }

            } while (!done);
        }
    }
}
