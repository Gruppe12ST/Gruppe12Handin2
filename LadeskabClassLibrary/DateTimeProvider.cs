using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadeskabClassLibrary
{
    public class DateTimeProvider : IDateTimeProvider
    { 
        private DateTime _gen = DateTime.Now;

        public DateTime GetDateTime()
        {
            return _gen;
        }
    }
}
