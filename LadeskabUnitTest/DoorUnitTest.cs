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

        [TestCase(1)]
        [TestCase(2)]
        public void SetDoor_DoorChanged_EventFired(int doorStatus)
        {
            _uut.SetDoor(doorStatus);
            Assert.That(_doorOcEventArgs,Is.Not.Null);
        }

        [TestCase(0)]
        [TestCase(3)]
        public void SetDoor_WrongStatus_EventNotFired(int doorStatus)
        {
            _uut.SetDoor(doorStatus);
            Assert.That(_doorOcEventArgs,Is.Null);
        }

        [TestCase(1)]
        public void SetDoor_DoorOpen_CorrectStatus(int doorStatus)
        {
            _uut.SetDoor(doorStatus);
            Assert.That(_doorOcEventArgs.Open,Is.EqualTo(true));
        }

        [TestCase(2)]
        public void SetDoor_DoorClosed_CorrectStatus(int doorStatus)
        {
            _uut.SetDoor(doorStatus);
            Assert.That(_doorOcEventArgs.Open, Is.EqualTo(false));
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
