using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LadeskabClassLibrary
{
    public class DoorOCEventArgs : EventArgs
    {
        public bool Open { set; get; }
    }

    public interface IDoor
    {
        event EventHandler<DoorOCEventArgs> DoorOCEvent;
        
        int DoorStatus { get; }
        bool DoorLock { get; }
        void setDoor(int doorStatus);
        void LockDoor();
        void UnlockDoor();
    }
}
