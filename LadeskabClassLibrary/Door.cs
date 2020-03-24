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

        public bool DoorOpen { get; private set; } 

        public bool DoorLock { get; private set; } //Døren er låst ved true og åben ved false

        public void SetDoor(bool doorOpen)
        {
            if (doorOpen)
            {
                DoorOpen = true;
                DoorChanged(new DoorOCEventArgs() { Open = DoorOpen });
            }
            else
            {
                DoorOpen = false;
                DoorChanged(new DoorOCEventArgs() { Open = DoorOpen });
            }
        }

        public void LockDoor()
        {
            DoorLock = true;
            DoorOpen = false;
        }

        public void UnlockDoor()
        {
            DoorLock = false;
            DoorOpen = true;
        }

        protected virtual void DoorChanged(DoorOCEventArgs e)
        {
            DoorOCEvent?.Invoke(this, e);
        }
    }
}
