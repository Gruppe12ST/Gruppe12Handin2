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

        private FileStream _input;
        private StreamReader _fileReader;

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

            _input = new FileStream(_path, FileMode.Open, FileAccess.Read);
            _fileReader = new StreamReader(_input);

            string inputR;
            string[] inputF;
            List<string> text = new List<string>();
            while ((inputR = _fileReader.ReadLine()) != null)
            {
                inputF = inputR.Split(';');
                text.Add(inputF[0]);
            }

            _fileReader.Close();

            string filetext = text.Last();
            Assert.IsTrue(filetext.Length > 1);

        }

        [Test]
        public void LogDoorLocked_LockedIdI33_datetimeprovider()
        {
            _uut.LogDoorLocked(33);
            _dateTime.Received(1).GetDateTime();
        }


        [Test]
        public void LogDoorUnLocked_LockedIdIs34_UnLockedDoorLogged()
        {
            _uut.LogDoorUnlocked(34);

            _input = new FileStream(_path, FileMode.Open, FileAccess.Read);
            _fileReader = new StreamReader(_input);

            string inputR;
            string[] inputF;
            List<string> text = new List<string>();
            while ((inputR = _fileReader.ReadLine()) != null)
            {
                inputF = inputR.Split(';');
                text.Add(inputF[0]);
            }

            _fileReader.Close();

            string fiiletext = text.Last();
            Assert.IsTrue(fiiletext.Length > 1);
        }

        [Test]
        public void LogDoorUnLocked_LockedIdI34_datetimeprovider()
        {
            _uut.LogDoorUnlocked(34);
            _dateTime.Received(1).GetDateTime();
        }

       

    }
}
