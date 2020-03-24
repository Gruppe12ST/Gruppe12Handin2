using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LadeskabClassLibrary;
using NSubstitute;
using NUnit.Framework;

namespace LadeskabUnitTest
{
    [TestFixture]
    class DisplayUnitTest
    {
        private Display _uut;
        [SetUp]
        public void SetUp()
        {
            _uut = new Display();
        }

        [TestCase("Ladeskab er ledigt")]
        [TestCase("123456")]
        public void test(string message)
        {
            _uut.Show(message);
            StringAssert.Contains(message,_uut._message);
        }
    }
}
