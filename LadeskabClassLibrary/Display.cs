using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadeskabClassLibrary
{
    public class Display : IDisplay
    {
        public void Show(string message)
        {
            Console.WriteLine(message);
        }
    }
}
