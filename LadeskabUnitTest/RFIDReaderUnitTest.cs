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

    public class RFIDReaderUnitTest
    {
        private RFIDReader _uut;
        private RfidDetectedEventArgs _receivedEventArgs;

        [SetUp]
        public void Setup()
        {
            _receivedEventArgs = null;

            _uut = new RFIDReader();

            // event listener
            _uut.RfidDetectedEvent +=
                (o, args) => { _receivedEventArgs = args; };
        }

        //ZOBMIES Test

        //Zero
        [Test]
        public void HandleNoEvent_NoIdSet()
        {
            Assert.That(_receivedEventArgs.Id, Is.Null);
        }

        //One - input
        public void SetRfidId_IdSet_EventFiredOne()
        {
            int id = 100;
            _uut.SetRfidId(id);
            Assert.That(_receivedEventArgs, Is.Not.Null);
        }

        //One - output
        public void SetRfidId_IdSet_CorrectNewIdRecivedOne()
        {
            int id = 100;
            _uut.SetRfidId(id);
            Assert.That(_receivedEventArgs.Id, Is.EqualTo(id));
        }

        //Many
        //Event udført
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

        //Many
        //Korrekt id genkendt
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
