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
        public int _id { get; private set; }

        public void SetRfidId(int id)                               // Denne metode skal bruges fra vores App, så vi kan finde 
        {
            //_id = id;                                                     // på værdier til RFID-tags, der "scannes" på vores ladeskab
            IdDetected(new RfidDetectedEventArgs {Id = id});
            _id = id;
        }

        protected virtual void IdDetected(RfidDetectedEventArgs e)
        {
            RfidDetectedEvent?.Invoke(this, e);               // Informerer alle observeres om, at id er blevet opdateret
        }                                                           // og sender det nye id til observeres
    }
}
