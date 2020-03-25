using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadeskabClassLibrary
{
    public class RFIDReader : IRFIDReader
    {
        public event EventHandler<RfidDetectedEventArgs> RfidDetectedEvent;

        public void SetRfidId(int id)                       // Denne metode skal bruges fra vores App, så vi kan finde 
        {                                                   // på værdier til RFID-tags, der "scannes" på vores ladeskab
            IdDetected(new RfidDetectedEventArgs {Id = id});
        }

        protected virtual void IdDetected(RfidDetectedEventArgs e)
        {
            RfidDetectedEvent?.Invoke(this, e);               // Informerer alle observeres om, at id er blevet opdateret
        }                                                           // og sender det nye id til observeres
    }
}
