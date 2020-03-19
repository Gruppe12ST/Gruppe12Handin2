using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadeskabClassLibrary
{
    class Door : IDoor
    {
        public bool DoorStatus => throw new NotImplementedException();

        public event EventHandler<DoorOCEventArgs> DoorOCEvent;

        public void LockDoor()
        {
            throw new NotImplementedException();
        }

        public void UnlockDoor()
        {
            throw new NotImplementedException();
        }
    }
}
