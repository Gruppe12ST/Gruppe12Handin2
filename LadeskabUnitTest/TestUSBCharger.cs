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

    class TestUSBCharger
    {
        private USBCharger _uut;
        private CurrentEventArgs _recievedEventArgs;

        [SetUp]
        public void Setup()
        {
            _recievedEventArgs = null;
            _uut = new USBCharger();

            _uut.CurrentValueEvent +=
                (o, args) =>
                {
                    _recievedEventArgs = args;
                };
        }

        [Test]
        public void Test1()
        {
            _uut.StartCharge();
            Assert.That(_recievedEventArgs, Is.Not.Null);
        }

        //Er slet ikke færdigt!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1

    }
}
