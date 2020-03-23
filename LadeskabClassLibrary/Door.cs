using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadeskabClassLibrary
{
    public class Door : IDoor
    {
        public event EventHandler<DoorOCEventArgs> DoorOCEvent;

        public int DoorStatus { get; private set; } //Er den 1 er døren åben, er den 2 er døren lukket

        public bool DoorLock { get; private set; }    

        public void setDoor()
        {
            if (DoorStatus == 1)
            {
                DoorChanged(new DoorOCEventArgs() {Open = true});
            }
            else if (DoorStatus == 2)
            {
                DoorChanged(new DoorOCEventArgs() {Open = false});
            }
        }

        public void LockDoor()
        {
            DoorLock = true;
            DoorStatus = 2;
        }

        public void UnlockDoor()
        {
            DoorLock = false;
            DoorStatus = 1;
        }

        protected virtual void DoorChanged(DoorOCEventArgs e)
        {
            DoorOCEvent?.Invoke(this, e);
        }
    }
}
