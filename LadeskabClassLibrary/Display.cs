using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadeskabClassLibrary
{
    public class Display : IDisplay
    {
        public string _message { get; private set; }
        public void Show(string message)
        {
            _message = message;
            Console.WriteLine(_message);
        }
    }
}
