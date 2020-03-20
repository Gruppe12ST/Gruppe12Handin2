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
        private readonly IRFIDReader _rfidReader;
        private readonly IChargeControl _chargecontrol;
        private readonly ILogFile _logfile;

        public bool _doorOpen { get; private set; }
        public int _id { get;  private set; }
        public int _newid { get; private set; }

        public StationControl(IRFIDReader rfidReader, IDoor door, IChargeControl chargeControl,IDisplay display, ILogFile logfile)
        {
            _door = door;
            _display = display;
            _rfidReader = rfidReader;
            _chargecontrol = chargeControl;
            _logfile = logfile;

            _door.DoorOCEvent += HandleDoorChangedEvent;
             _rfidReader.RfidDetectedEvent += HandleRFIDDetectedEvent;
            _display.Show("Ladeskab ledigt");
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

            if (_state == LadeskabState.Available)
            {
                if (_doorOpen)
                {
                    _display.Show("Tilslut telefon");
                    _state = LadeskabState.DoorOpen;
                }
            }

            if (_state == LadeskabState.DoorOpen)
            {
                if (!_doorOpen)
                {
                    _display.Show("Indlæs RFID");
                    _state = LadeskabState.Available;
                }
            }

        }
        
        private void HandleRFIDDetectedEvent(object sender, RfidDetectedEventArgs e)
        {
            int id = e.Id;

            switch (_state)
            {
                case LadeskabState.Available:
                    if (_chargecontrol.IsConnected())
                    {
                        _door.LockDoor();
                        _logfile.LogDoorLocked(id);
                        _display.Show("Ladeskab optaget");
                        _chargecontrol.StartCharge();
                        _id = id;
                        _state = LadeskabState.Locked;

                    }
                    else
                    {
                        _display.Show("Tilslutningsfejl");
                    }
                    break;

                case LadeskabState.Locked:
                    _newid = id;

                    if (CheckId())
                    {
                        _chargecontrol.StopCharge();
                        _door.UnlockDoor();
                        _logfile.LogDoorUnlocked(id);
                        _display.Show("Fjern telefon");
                        _state = LadeskabState.Available;
                    }
                    else
                    {
                        _display.Show("RFID fejl");
                    }
                    break;


            }
        }

        

        private bool CheckId()
        {
            if (_id == _newid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
