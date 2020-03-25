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
    public class ChargeControlUnitTest
    {
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

        //Zero-test
        [Test]
        public void HandleNoEventDoneCharging()
        {
            _display.DidNotReceive().Show("Telefonen er fuldt opladt");
        }

        //Zero-test
        [Test]
        public void HandleNoEventCharging()
        {
            _display.DidNotReceive().Show("Telefonen lader");
        }

        //Zero-test
        [Test]
        public void HandleNoEventErrorCharging()
        {
            _display.DidNotReceive().Show("Der er sket en fejl. Frakobl straks din telefon");
        }

        //One-test
        [TestCase(0)]   //BVA analyse er brugt til at finde værdier til test-cases
        [TestCase(6)] 
        public void HandleEventNotDoneCharging(int current)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current });
            _display.DidNotReceive().Show("Telefonen er fuldt opladt");
        }

        //One-test
        [TestCase(1)]   //BVA analyse er brugt til at finde værdier til test-cases 
        [TestCase(5)]
        public void HandleEventDoneCharging(int current)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current });
            _display.Received(1).Show("Telefonen er fuldt opladt");
        }

        //Many-test
        [TestCase(1, 3, 5)]
        public void HandleEventDoneChargingRepeat(int current1, int current2, int current3)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current1 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current2 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current3 });
            _display.Received(1).Show("Telefonen er fuldt opladt");
        }

        //Many-test
        [TestCase(5,500,5)]
        public void HandleEventDoneChargingMany(int current1, int current2, int current3)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current1 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current2 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current3 });
            _display.Received(2).Show("Telefonen er fuldt opladt");
        }

        //One-test
        [TestCase(6)] //BVA analyse er brugt til at finde værdier til test-cases
        [TestCase(500)]
        public void HandleEventCharging(int current)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current });
            _display.Received(1).Show("Telefonen lader");
        }

        //One-test
        [TestCase(5)] //BVA analyse er brugt til at finde værdier til test-cases
        [TestCase(501)]
        public void HandleEventNotCharging(int current)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current });
            _display.DidNotReceive().Show("Telefonen lader");
        }

        //Many-test
        [TestCase(6, 200, 500)]
        public void HandleEventChargingRepeat(int current1, int current2, int current3)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current1 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current2 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current3 });
            _display.Received(1).Show("Telefonen lader");
        }

        //Many test
        [TestCase(6, 501, 500)]
        public void HandleEventChargingMany(int current1, int current2, int current3)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current1 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current2 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current3 });
            _display.Received(2).Show("Telefonen lader");
        }

        //One-test
        [TestCase(501)]
        [TestCase(800)]
        public void HandleEventErrorCharging(int current)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current });
            _display.Received(1).Show("Der er sket en fejl. Frakobl straks din telefon");
        }

        //One-test
        [TestCase(500)] 
        [TestCase(499)]
        public void HandleEventNotErrorCharging(int current)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current });
            _display.DidNotReceive().Show("Der er sket en fejl. Frakobl straks din telefon");
        }

        //Many-test
        [TestCase(501, 700,1200)]
        public void HandleEventErrorChargingRepeat(int current1, int current2, int current3)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current1 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current2 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current3 });
            _display.Received(1).Show("Der er sket en fejl. Frakobl straks din telefon");
        }

        //Many-test
        [TestCase(501, 500, 501)]
        public void HandleEventErrorChargingMany(int current1, int current2, int current3)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current1 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current2 });
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current3 });
            _display.Received(2).Show("Der er sket en fejl. Frakobl straks din telefon");
        }

        //Test af connection
        [TestCase(true)]
        [TestCase(false)]
        public void TestIsConnected(bool connectionStatus)
        {
            _usbCharger.Connected.Returns(connectionStatus);
            Assert.That(_uut.IsConnected, Is.EqualTo(connectionStatus));
        }

        //Zero
        [Test]
        public void TestStartChargeZero()
        {
            _usbCharger.Received(0).StartCharge();
        }

        //One
        [Test]
        public void TestStartChargeOne()
        {
            _uut.StartCharge();
            _usbCharger.Received(1).StartCharge();
        }

        //Many
        [Test]
        public void TestStartChargemany()
        {
            _uut.StartCharge();
            _uut.StartCharge();
            _usbCharger.Received(2).StartCharge();
        }

        //Zero
        [Test]
        public void TestStopChargeZero()
        {
            _usbCharger.Received(0).StopCharge();
        }

        //One
        [Test]
        public void TestStopChargeOne()
        {
            _uut.StopCharge();
            _usbCharger.Received(1).StopCharge();
        }

        //Many
        [Test]
        public void TestStopChargeMany()
        {
            _uut.StopCharge();
            _uut.StopCharge();
            _usbCharger.Received(2).StopCharge();
        }

    }
}
