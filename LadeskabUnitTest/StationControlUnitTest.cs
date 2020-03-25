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

        //Zero test hvor der ikke har været en påvirkning af klassen endnu
        [Test]
        public void ZeroDoorChangeEvent_Udskriv_LadeskabLedigt()
        {
            _display.Received(1).Show("Ladeskab ledigt");
            _display.DidNotReceive().Show("Tilslut telefon");
        }


        //Test af metoden der håndterer Door events, efter et event er kaldt, som har været at døren åbner 
        [Test]
        public void One_HandleDoorChangedEvent_Open_Udskriv_TilslutTelefon()
        {
            _door.DoorOCEvent += Raise.EventWith(new DoorOCEventArgs {Open = true});
            _display.Received(1).Show("Tilslut telefon");
        }


        [Test]
        public void Two_HandleDoorChangedEvent_OpenClosed_Udskriv_Tilslut_IndlæsRFID()
        {
            //Døren skal have været åbnet før den kan lukkes
            _door.DoorOCEvent += Raise.EventWith(new DoorOCEventArgs {Open = true});

            _door.DoorOCEvent += Raise.EventWith(new DoorOCEventArgs { Open = false });
            _display.Received(1).Show("Tilslut telefon");
            _display.Received(1).Show("Indlæs RFID");
        }

        [Test]
        public void Many_HandleDoorChangedEvent_OpenClosedOpen()
        {
            //Døren skal have været åbnet før den kan lukkes
            _door.DoorOCEvent += Raise.EventWith(new DoorOCEventArgs { Open = true });
            //Døren lukkes
            _door.DoorOCEvent += Raise.EventWith(new DoorOCEventArgs { Open = false });
            //Døren åbnes igen
            _door.DoorOCEvent += Raise.EventWith(new DoorOCEventArgs { Open = true });

            _display.Received(2).Show("Tilslut telefon");
            _display.Received(1).Show("Indlæs RFID");
        }

        [Test]
        public void HandleDoorChangedEvent_Taken_Udskriv_LadeskabLedigt()
        {
            _chargeControl.IsConnected().Returns(true);
            //Skab er blevet låst og låst op igen
            _rfidReader.RfidDetectedEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1 });
            _rfidReader.RfidDetectedEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1 });

            //Dør åbnes 
            _door.DoorOCEvent += Raise.EventWith(new DoorOCEventArgs {Open = true});
            
            _display.Received(2).Show("Ladeskab ledigt");//En fra constructoren, og en fordi skabet nu er ledigt igen
        }


        //Test af stationkontrols tilstand, når der ikke har været en rfid event endnu

        [Test]
        public void zerorfidDetected_DefaultIntIdVaerdi()
        {
            int id = _uut._id;
            Assert.That(id,Is.EqualTo(0));
        }



        //Her er det en test efter én rfid event påvirkning, i den situation hvor telefonen ikke er sat til at lade
        [Test]
        public void One_RFIDDetected_skabAvailable_usbChargerNotConnected_Udskriv_Tilslutningsfejl()
        {
            _chargeControl.IsConnected().Returns(false);
            _rfidReader.RfidDetectedEvent += Raise.EventWith(new RfidDetectedEventArgs {Id = 1});

            _display.Received(1).Show("Tilslutningsfejl");
        }

        //Her er det en test efter én rfid event påvirkning, i den situation hvor telefonen er sat til at oplade
        [TestCase( 1)]
        [TestCase(0006)]
        public void One_RFIDDetected_skabAvailable_usbChargeConnected_Udskriv_LadeskabOptaget(int id)
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



        //Har testes funktionaliteten når der sker flere, her to, rfid events - med forkert id
        [TestCase(2, 5)]
        public void Two_RFIDDetected_skabLocked_WrongRFID_Udskriv_RFIDFejl(int id, int newid)
        {
            _chargeControl.IsConnected().Returns(true);
            _rfidReader.RfidDetectedEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = id });
            _rfidReader.RfidDetectedEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = newid });
            _logfile.DidNotReceive().LogDoorUnlocked(newid);

            _display.Received(1).Show("RFID fejl");
        }

        

        //Har testes funktionaliteten når der sker flere, her to, rfid events - med ridgtigt id
        [TestCase(5, 5)]
        [TestCase(0045,45)]
        public void Two_RFIDDetected_skabLocked_CorrectRFID_Udskriv_FjernTelefon(int id, int newid)
        {
            _chargeControl.IsConnected().Returns(true);
            _rfidReader.RfidDetectedEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = id });
            _rfidReader.RfidDetectedEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = newid });
            _logfile.Received(1).LogDoorUnlocked(newid);

            _chargeControl.Received(1).StopCharge();
            _door.Received(1).UnlockDoor();
            _display.Received(1).Show("Fjern telefon");

        }

        //Her er en test af flere brug af ladeskabet, hvor det låses, låses op. Låses og låses op igen
        [TestCase(5, 5)]
        [TestCase(0045, 45)]
        public void TwoUsesOfSKab_RFIDDetected_LockUnloclLockUnlock_Udskriv_(int id, int newid)
        {
            _chargeControl.IsConnected().Returns(true);
            _rfidReader.RfidDetectedEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = id });
            _rfidReader.RfidDetectedEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = newid });
            _door.DoorOCEvent += Raise.EventWith(new DoorOCEventArgs {Open = true});
            _rfidReader.RfidDetectedEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = id });
            _rfidReader.RfidDetectedEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = newid });

            _logfile.Received(2).LogDoorUnlocked(newid);
            _chargeControl.Received(2).StopCharge();
            _door.Received(2).LockDoor();
            _door.Received(2).UnlockDoor();
            _display.Received(2).Show("Fjern telefon");

        }


    }
}
