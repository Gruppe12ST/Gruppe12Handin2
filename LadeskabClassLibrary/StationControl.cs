using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadeskabClassLibrary
{
    public class StationControl
    {
        private readonly IDoor _door;
        private readonly IDisplay _display;
        public bool _doorOpen { get; private set; }
        public StationControl(IRFIDReader rfidReader, IDoor door, IUSBCharger usbCharger, IChargeControl chargeControl,IDisplay display)
        {
            _door = door;
            _display = display;
            _door.DoorOCEvent += HandleDoorChangedEvent;
        }


        private enum LadeskabState
        {
            Available,
            Locked,
            DoorOpen
        };

        private LadeskabState _state = LadeskabState.Available;

        private void HandleDoorChangedEvent(object sender, DoorOCEventArgs e)
        {
            _doorOpen = e.Open;

            if (_state!=LadeskabState.Locked)
            {
                if (_doorOpen)
                {
                    _display.Show("Tilslut telefon");
                    _state = LadeskabState.DoorOpen;
                }
            }

            if (_state != LadeskabState.Locked)
            {
                if (!_doorOpen)
                {
                    _display.Show("Indlæs RFID");

                }
            }

            
        }
    }
}
