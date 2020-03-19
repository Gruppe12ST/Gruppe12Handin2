using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LadeskabClassLibrary;
using NUnit.Framework;

namespace LadeskabUnitTest
{
    [TestFixture]

    public class RFIDReaderUnitTest
    {
        private RFIDReader _uut;
        private RfidDetectedEventArgs _receivedEventArgs;

        [SetUp]
        public void Setup()
        {
            _receivedEventArgs = null;

            _uut = new RFIDReader();
            _uut.SetRfidId(234);

            // event listener
            _uut.RfidDetectedEvent +=
                (o, args) => { _receivedEventArgs = args; };
        }

        [TestCase(222)]
        [TestCase(3838)]
        [TestCase(2)]
        [TestCase(098)]
        [TestCase(000)]
        [TestCase(9)]
        public void SetRfidId_IdSetToNewValue_EventFired(int id)
        {
            _uut.SetRfidId(id);
            Assert.That(_receivedEventArgs, Is.Not.Null);
        }

        [TestCase(222)]
        [TestCase(3838)]
        [TestCase(2)]
        [TestCase(098)]
        [TestCase(000)]
        [TestCase(9)]
        public void SetRfidId_IdSetToNewValue_CorrectNewIdRecived(int id)
        {
            _uut.SetRfidId(id);
            Assert.That(_receivedEventArgs.Id, Is.EqualTo(id));
        }
    }
}
