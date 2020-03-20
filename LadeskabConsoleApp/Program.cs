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
                
            IDoor door = new Door();
            IUSBCharger usbCharger = new USBCharger();
            IDisplay display = new Display();
            ILogFile logfile = new LogFile();
            IChargeControl chargeControl = new ChargeControl(usbCharger,display);

            StationControl stationControl = new StationControl(rfidReader,door,chargeControl,display,logfile);
            
        }
    }
}
