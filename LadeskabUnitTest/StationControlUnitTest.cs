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
        private ILogFile _logfile;

        private StationControl _uut;

        [SetUp]
        public void Setup()
        {
            _rfidReader = Substitute.For<IRFIDReader>();
            _display = Substitute.For<IDisplay>();
            _chargeControl = Substitute.For<IChargeControl>();
            _door = Substitute.For<IDoor>();
            _logfile = Substitute.For<ILogFile>();
            _uut = new StationControl(_rfidReader,_door,_chargeControl,_display,_logfile);

        }

        [Test]
        public void ZeroDoorChangeEvent()
        {
            _display.Received(1).Show("Ladeskab ledigt");
            _display.DidNotReceive().Show("Tilslut telefon");
        }

        [Test]
        public void HandleDoorChangedEvent_Open()
        {
            _door.DoorOCEvent += Raise.EventWith(new DoorOCEventArgs {Open = true});
            _display.Received(1).Show("Tilslut telefon");
        }

        [Test]
        public void HandleDoorChangedEvent_OpenClosed()
        {
            //Døren skal have været åbnet før den kan lukkes
            _door.DoorOCEvent += Raise.EventWith(new DoorOCEventArgs {Open = true});

            _door.DoorOCEvent += Raise.EventWith(new DoorOCEventArgs { Open = false });
            _display.Received(1).Show("Indlæs RFID");
        }

        [Test]
        public void HandleDoorChangedEvent_OpenClosedOpen()
        {
            //Døren skal have været åbnet før den kan lukkes
            _door.DoorOCEvent += Raise.EventWith(new DoorOCEventArgs { Open = true });
            //Døren lukkes
            _door.DoorOCEvent += Raise.EventWith(new DoorOCEventArgs { Open = false });
            //Døren åbnes igen
            _door.DoorOCEvent += Raise.EventWith(new DoorOCEventArgs { Open = true });

            _display.Received(2).Show("Tilslut telefon");
        }


        [Test]
        public void RFIDDetected_skabAvailable_usbChargerNotConnected()
        {
            _chargeControl.IsConnected().Returns(false);
            _rfidReader.RfidDetectedEvent += Raise.EventWith(new RfidDetectedEventArgs {Id = 1});

            _display.Received(1).Show("Tilslutningsfejl");
        }

        [TestCase( 1)]
        [TestCase(0006)]
        public void RFIDDetected_skabAvailable_usbChargeConnected(int id)
        {
            _chargeControl.IsConnected().Returns(true);
            _rfidReader.RfidDetectedEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = id });


            _door.Received(1).LockDoor();
            _display.Received(1).Show("Ladeskab optaget");
            _chargeControl.Received(1).StartCharge();
            _logfile.Received(1).LogDoorLocked(id);

            int _id = _uut._id;
            Assert.That(_id,Is.EqualTo(id));

        }

        [TestCase(2, 5)]
        public void RFIDDetected_skabLocked_WrongRFID(int id, int newid)
        {
            _chargeControl.IsConnected().Returns(true);
            _rfidReader.RfidDetectedEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = id });
            _rfidReader.RfidDetectedEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = newid });
            _logfile.DidNotReceive().LogDoorUnlocked(newid);

            _display.Received(1).Show("RFID fejl");
        }

        [TestCase(5, 5)]
        [TestCase(0045,45)]
        public void RFIDDetected_skabLocked_CorrectRFID(int id, int newid)
        {
            _chargeControl.IsConnected().Returns(true);
            _rfidReader.RfidDetectedEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = id });
            _rfidReader.RfidDetectedEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = newid });
            _logfile.Received(1).LogDoorUnlocked(newid);

            _chargeControl.Received(1).StopCharge();
            _door.Received(1).UnlockDoor();
            _display.Show("Fjern telefon");

        }

    }
}
