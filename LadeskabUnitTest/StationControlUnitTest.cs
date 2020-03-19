using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LadeskabClassLibrary;
using NSubstitute;
using NUnit.Framework;

namespace LadeskabUnitTest
{
    [TestFixture]
    class StationControlUnitTest
    {
        private IRFIDReader _rfidReader;
        private IDisplay _display;
        private IChargeControl _chargeControl;
        private IDoor _door;
        private IUSBCharger _usbCharger;

        private StationControl _uut;

        [SetUp]
        public void Setup()
        {
            _rfidReader = Substitute.For<IRFIDReader>();
            _display = Substitute.For<IDisplay>();
            _chargeControl = Substitute.For<IChargeControl>();
            _door = Substitute.For<IDoor>();
            _usbCharger = Substitute.For<IUSBCharger>();

            _uut = new StationControl(_rfidReader,_door,_usbCharger,_chargeControl,_display);

        }

        [TestCase(true)]
        public void DoorOpenEvent(bool DoorState)
        {
            _door.DoorOCEvent += Raise.EventWith(new DoorOCEventArgs {Open = DoorState});
            _display.Received(1).Show("Tilslut telefon");
        }

        [TestCase(false)]
        public void DoorClosedEvent(bool DoorState)
        {
            _door.DoorOCEvent += Raise.EventWith(new DoorOCEventArgs { Open = DoorState });
            _display.Received(1).Show("Indlæs RFID");
        }
    }
}
