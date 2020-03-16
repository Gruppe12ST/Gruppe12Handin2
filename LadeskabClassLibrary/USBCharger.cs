using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadeskabClassLibrary
{
    class USBCharger:IUsbCharger
    {
        public event EventHandler<CurrentEventArgs> CurrentValueEvent;
        public double CurrentValue { get; }
        public bool Connected { get; }
        public void StartCharge()
        {
            throw new NotImplementedException();
        }

        public void StopCharge()
        {
            throw new NotImplementedException();
        }
    }
}
