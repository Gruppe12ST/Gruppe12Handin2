using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadeskabClassLibrary
{
    public class LogFile : ILogFile
    {
        private string _path = "@/../../LogEventToFile.txt";
        private string _text;
        private DateTime _dateTime;
        private int _id;
        private readonly IDateTimeProvider _dateTimeProvider = new DateTimeProvider();
        // Er det den bedste måde at "hente" tiden på??

        public void LogDoorLocked(int id)
        {
            _id = id;
            _dateTime = _dateTimeProvider.GetDateTime();
            _text = "The door is locked at "+_dateTime+", with ID number: "+_id+"\n";
            System.IO.File.AppendAllText(_path,_text);
        }

        public void LogDoorUnlocked(int id)
        {
            _id = id;
            _dateTime = _dateTimeProvider.GetDateTime();
            _text = "The door is unlocked at " + _dateTime + ", with ID number: " + _id+"\n";
            System.IO.File.AppendAllText(_path, _text);
        }
    }
}
