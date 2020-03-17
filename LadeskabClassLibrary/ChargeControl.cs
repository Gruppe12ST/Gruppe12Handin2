using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadeskabClassLibrary
{
    public class ChargeControl : IChargeControl
    {
        private readonly IUSBCharger _usbCharger;
        private readonly IDisplay _display;
        
        public double _current { get; private set; }

        public ChargeControl(IUSBCharger usbCharger, IDisplay display)
        {
            _usbCharger = usbCharger; 
            _usbCharger.CurrentValueEvent += HandleCurrentChangedEvent;
            _display = display;
        }

        private enum ChargingState
        {
            NotCharging,
            Done,
            Charging,
            Error
        };

        private ChargingState _state = ChargingState.NotCharging;

        private void HandleCurrentChangedEvent(object sender, CurrentEventArgs e)       // Ideen er, at hvis strømmen ændrer sig en lille smule (fx fra 490 til 500 mA) er det ikke nødvendigt
        {                                                                               // at kalde display og metoden show igen hvis systemet fx allerede er i stadiet "Charging". Metoden
            _current = e.Current;                                                       // Show() behøver altså kun blive kaldt når opladningen rent faktisk skifter fra fx Charging til Done.

            if (_state!=ChargingState.Done)
            {
                if (_current > 0 && _current <= 5)
                {
                    _display.Show("Telefonen er fuldt opladt");
                    _state = ChargingState.Done;
                }
            }

            if (_state != ChargingState.Charging)
            {
                if (_current > 5 && _current <= 500)
                {
                    _display.Show("Telefonen lader");
                    _state = ChargingState.Charging;
                }
            }

            if (_state != ChargingState.Error)
            {
                if (_current > 500)
                {
                    _display.Show("Der er sket en fejl. Frakobl straks din telefon");
                }
            }
        }

        public bool IsConnected()
        {
            return _usbCharger.Connected;
        }

        public void StartCharge()
        {
            _usbCharger.StartCharge();
        }

        public void StopCharge()
        {
            _usbCharger.StopCharge();
        }


    }
}
