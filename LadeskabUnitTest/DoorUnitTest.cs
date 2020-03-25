using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using LadeskabClassLibrary;

namespace LadeskabUnitTest
{
    [TestFixture]
    public class DoorUnitTest
    {
        private Door _uut;
        private DoorOCEventArgs _doorOcEventArgs;

        [SetUp]
        public void Setup()
        {
            _doorOcEventArgs = null;
            
            _uut = new Door();
            
            _uut.DoorOCEvent +=
                (o, args) => { _doorOcEventArgs = args; };
        }

        [Test]
        public void DoorOCEvent_zeroEvent_EventNotFired()
        {
            Assert.That(_doorOcEventArgs, Is.Null);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SetDoor_DoorChanged_EventFired(bool doorOpen)
        {
            _uut.SetDoor(doorOpen);
            Assert.That(_doorOcEventArgs,Is.Not.Null);
        }

        [TestCase(true)]
        public void SetDoor_DoorOpen_CorrectStatus(bool doorOpen)
        {
            _uut.SetDoor(doorOpen);
            Assert.That(_doorOcEventArgs.Open,Is.EqualTo(doorOpen));
        }

        [TestCase(false)]
        public void SetDoor_DoorClosed_CorrectStatus(bool doorOpen)
        {
            _uut.SetDoor(doorOpen);
            Assert.That(_doorOcEventArgs.Open, Is.EqualTo(doorOpen));
        }

        [Test]
        public void LockDoor_DoorLocked_DoorLockIsTrue()
        {
            _uut.LockDoor();
            Assert.That(_uut.DoorLock,Is.EqualTo(true));
        }

        [Test]
        public void UnlockDoor_DoorUnlocked_DoorLockIsFalse()
        {
            _uut.UnlockDoor();
            Assert.That(_uut.DoorLock, Is.EqualTo(false));
        }

    }
}
