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
            var filetext = File.ReadLines("@/../../LogEventToFile.txt");
            Assert.IsTrue(filetext.ToString().Length > 1);

        }

        [Test]
        public void LogDoorLocked_LockedIdI33_datetimeprovider()
        {
            _uut.LogDoorLocked(33);
            _dateTime.Received(1).GetDateTime();
        }

    }
}
