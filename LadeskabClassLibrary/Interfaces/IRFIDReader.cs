using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadeskabClassLibrary
{
    public class RfidDetectedEventArgs : EventArgs
    {
        public int Id { get; set; }
    }

    public interface IRFIDReader
    {
        event EventHandler<RfidDetectedEventArgs> RfidDetectedEvent;
    }
}
