using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LadeskabClassLibrary;
using NSubstitute;
using NUnit.Framework;

namespace LadeskabUnitTest
{
    [TestFixture]
    public class LogFileUnitTest
    {
        private IDateTimeProvider _dateTime;
        private ILogFile _uut;
        private string _path = @"..\..\LoggedEventsDocument.txt";


        [SetUp]
        public void Setup()
        {
            _dateTime = Substitute.For<IDateTimeProvider>(); 
            _uut = new LogFile(_dateTime);
        }

        [Test]
        public void LogDoorLocked_LockedIdIs33_LockedDoorLogged()
        {
            _uut.LogDoorLocked(33);
            var filetext = File.ReadLines(_path);
            Assert.IsTrue(filetext.ToString().Length > 1);
        }

        [Test]
        public void LogDoorLocked_LockedIdI33_datetimeprovider()
        {
            _uut.LogDoorLocked(33);
            _dateTime.Received(1).GetDateTime();
        }

        //[Test]
        //public void LogDoorUnLocked_LockedIdIs34_UnLockedDoorLogged()
        //{
        //    _uut.LogDoorUnlocked(34);
        //    var filetext = File.ReadLines(_path);
        //    Assert.IsTrue(filetext.ToString().Length > 1);
        //}

        //[Test]
        //public void LogDoorUnLocked_LockedIdI34_datetimeprovider()
        //{
        //    _uut.LogDoorUnlocked(34);
        //    _dateTime.Received(1).GetDateTime();
        //}
    }
}
