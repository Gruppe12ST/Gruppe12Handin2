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
        public void zero_log()
        {
            //Hvis der ikke er blevet logget, er der ikke kaldt til DatetimeProvider
            _dateTime.DidNotReceive().GetDateTime();
        }

        //Logget en gang ved lås
        [Test]
        public void one_LogDoorLocked_LockedIdIs33_LockedDoorLogged()
        {
            int id = 33;
            _uut.LogDoorLocked(id);

            _input = new FileStream(_path, FileMode.Open, FileAccess.Read);
            _fileReader = new StreamReader(_input);

            string inputR;
            string[] inputF;
            List<string> text = new List<string>();
            while ((inputR = _fileReader.ReadLine()) != null)
            {
                inputF = inputR.Split(':');
                text.Add(inputF[3]);
            }

            _fileReader.Close();

            int filid = Convert.ToInt32(text.Last());
            Assert.That(id,Is.EqualTo(filid));

        }

        //Logget en gang
        [Test]
        public void LogDoorLocked_LockedIdI33_datetimeprovider()
        {
            _uut.LogDoorLocked(33);
            _dateTime.Received(1).GetDateTime();
        }

        //Logget flere gange ved lås - tjek på id
        [TestCase(45,23,87)]
        public void LogDoorLocked_LockedFlereID_KorrektID(int id1, int id2, int id3)
        {
            _uut.LogDoorLocked(id1);
            _uut.LogDoorLocked(id2);
            _uut.LogDoorLocked(id3);

            _input = new FileStream(_path, FileMode.Open, FileAccess.Read);
            _fileReader = new StreamReader(_input);

            string inputR;
            string[] inputF;
            List<string> text = new List<string>();
            while ((inputR = _fileReader.ReadLine()) != null)
            {
                inputF = inputR.Split(':');
                text.Add(inputF[3]);
            }

            _fileReader.Close();

            int filid = Convert.ToInt32(text.Last());
            Assert.That(id3, Is.EqualTo(filid));
        }

        //Logget flere gange 
        [Test]
        public void LogDoorLocked_LockIdI33_datatimeproviderRecived4()
        {
            _uut.LogDoorLocked(33);
            _uut.LogDoorLocked(33); 
            _uut.LogDoorLocked(33);
            _uut.LogDoorLocked(33);

            _dateTime.Received(4).GetDateTime();
        }


        //logget en gang ved oplåsning 
        [Test]
        public void LogDoorUnLocked_LockedIdIs34_UnLockedDoorLogged()
        {
            int id = 34;
            _uut.LogDoorUnlocked(id);

            _input = new FileStream(_path, FileMode.Open, FileAccess.Read);
            _fileReader = new StreamReader(_input);

            string inputR;
            string[] inputF;
            List<string> text = new List<string>();
            while ((inputR = _fileReader.ReadLine()) != null)
            {
                inputF = inputR.Split(':'); 
                text.Add(inputF[3]);
                
            }

            _fileReader.Close();

            int filid = Convert.ToInt32(text.Last());
            Assert.That(id, Is.EqualTo(filid));
        }

        //Log en gang ved oplåsning
        [Test]
        public void LogDoorUnLocked_LockedIdI34_datetimeprovider()
        {
            _uut.LogDoorUnlocked(34);
            _dateTime.Received(1).GetDateTime();
        }

        //Logget flere gange - tjek på id
        [TestCase(45, 23, 87)]
        public void LogDoorUnLocked_LockedFlereID_KorrektID(int id1, int id2, int id3)
        {
            _uut.LogDoorUnlocked(id1);
            _uut.LogDoorUnlocked(id2);
            _uut.LogDoorUnlocked(id3);

            _input = new FileStream(_path, FileMode.Open, FileAccess.Read);
            _fileReader = new StreamReader(_input);

            string inputR;
            string[] inputF;
            List<string> text = new List<string>();
            while ((inputR = _fileReader.ReadLine()) != null)
            {
                inputF = inputR.Split(':');
                text.Add(inputF[3]);
            }

            _fileReader.Close();

            int filid = Convert.ToInt32(text.Last());
            Assert.That(id3, Is.EqualTo(filid));
        }

        //Logget flere gange 
        [Test]
        public void LogDoorUnLocked_LockIdI33_datatimeproviderRecived4()
        {
            _uut.LogDoorUnlocked(33);
            _uut.LogDoorUnlocked(33);
            _uut.LogDoorUnlocked(33); 
            _uut.LogDoorUnlocked(33);

            _dateTime.Received(4).GetDateTime();
        }


    }
}
