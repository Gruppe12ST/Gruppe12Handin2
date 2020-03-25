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
        private string _path = @"..\..\LoggedEventsDocument.txt";
        private string _text;
        private int _id;
        private readonly IDateTimeProvider _dateTimeProvider;

        public LogFile(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public void LogDoorLocked(int id)
        {
            _id = id;
            _text = "The door is locked at "+ _dateTimeProvider.GetDateTime() + ", with ID number: "+_id+"\n";
            File.AppendAllText(_path,_text);

            //FileStream output = new FileStream(_path,FileMode.OpenOrCreate,FileAccess.Write);
            //StreamWriter fileWriter = new StreamWriter(output);
            //fileWriter.WriteLine(_text);
            //fileWriter.Close();

            //using (StreamWriter sw = new StreamWriter(_path))
            //{
            //    sw.WriteLine(_text);
            //}
        }

        public void LogDoorUnlocked(int id)
        {
            _id = id;
            _text = "The door is unlocked at " + _dateTimeProvider.GetDateTime() + ", with ID number: " + _id+"\n";
            File.AppendAllText(_path, _text);
        }
    }
}
