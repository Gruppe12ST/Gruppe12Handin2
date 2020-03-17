using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadeskabClassLibrary
{
    public class USBCharger : IUSBCharger
    {
        public event EventHandler<CurrentEventArgs> CurrentValueEvent;

        public double CurrentValue { get; private set; }                //Igen OBS - Hvad er funktionen af denne????????????????????????????????

        public bool Connected { get; private set; }

        public void StartCharge()
        {
            double ChargeCurrent = 500;
            CurrentChanged(new CurrentEventArgs { Current = ChargeCurrent});
            CurrentValue = ChargeCurrent;
        }

        public void StopCharge()
        {

        }

        private void CurrentChanged(CurrentEventArgs e)                 //OBS protected virtual void???? Læs op på dette!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            CurrentValueEvent?.Invoke(this, e);
            //CurrentValue = e.Current;
        }

    }
}
