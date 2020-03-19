using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LadeskabClassLibrary;
using NSubstitute;
using NSubstitute.Core.Arguments;
using NSubstitute.Extensions;
using NUnit.Framework;

namespace LadeskabUnitTest
{
    [TestFixture]
    public class TestChargeControl
    {
        // Generelt: arbejd lidt med hvordan jeg laver gode navne til tests!!!!!!!!!!!!!!!

        private IUSBCharger _usbCharger;
        private IDisplay _display;

        private ChargeControl _uut;

        [SetUp]
        public void Setup()
        {
            _usbCharger = Substitute.For<IUSBCharger>();
            _display = Substitute.For<IDisplay>();

            _uut = new ChargeControl(_usbCharger, _display);
        }

        [TestCase(1)]
        [TestCase(5)]                                                       
        public void TestHandleEventDone(int current)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current });
            _display.Received(1).Show("Telefonen er fuldt opladt");
        }

        [TestCase(0)]
        [TestCase(6)]
        public void TestHandleEventNotDone(int current)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current });
            _display.DidNotReceive().Show("Telefonen er fuldt opladt");
        }

        [TestCase(1, 3, 5)]
        public void TestHandleEventDoneRepeat(int current1, int current2, int current3)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current1 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current2 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current3 });
            _display.Received(1).Show("Telefonen er fuldt opladt");
        }

        [TestCase(6)]
        [TestCase(500)]
        public void TestHandleEventCharging(int current)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current });
            _display.Received(1).Show("Telefonen lader");
        }


        [TestCase(5)]
        [TestCase(501)]
        public void TestHandleEventNotCharging(int current)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current });
            _display.DidNotReceive().Show("Telefonen lader");
        }

        [TestCase(6, 200, 500)]
        public void TestHandleEventChargingRepeat(int current1, int current2, int current3)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current1 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current2 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current3 });
            _display.Received(1).Show("Telefonen lader");
        }

        [TestCase(501)]
        [TestCase(800)]
        public void TestHandleEventError(int current)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current });
            _display.Received(1).Show("Der er sket en fejl. Frakobl straks din telefon");
        }

        [TestCase(500)]
        [TestCase(499)]
        public void TestHandleEventNotError(int current)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current });
            _display.DidNotReceive().Show("Der er sket en fejl. Frakobl straks din telefon");
        }

        [TestCase(501, 700,1200)]
        public void TestHandleEventErrorRepeat(int current1, int current2, int current3)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current1 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current2 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current3 });
            _display.Received(1).Show("Der er sket en fejl. Frakobl straks din telefon");
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TestIsConnected(bool connectionStatus)
        {
            _usbCharger.Connected.Returns(connectionStatus);
            Assert.That(_uut.IsConnected, Is.EqualTo(connectionStatus));
        }

        [Test]
        public void TestStartCharge()
        {
            _uut.StartCharge();
            _usbCharger.Received(1).StartCharge();
        }

        [Test]
        public void TestStopCharge()
        {
            _uut.StopCharge();
            _usbCharger.Received(1).StopCharge();
        }
    }
}
