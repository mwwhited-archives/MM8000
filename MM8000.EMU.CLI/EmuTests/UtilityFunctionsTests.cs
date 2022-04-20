using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Linq;
using MM8000.EMU.CLI;

namespace EmuTests
{
    [TestClass]
    public class UtilityFunctionsTests
    {
        [DataTestMethod]
        [DataRow(0b000, false)]
        [DataRow(0b001, true)]
        [DataRow(0b010, true)]
        [DataRow(0b011, false)]
        [DataRow(0b100, true)]
        [DataRow(0b101, false)]
        [DataRow(0b110, false)]
        [DataRow(0b111, true)]
        public void CheckParity(int value, bool evenParity)
        {
            var ret = value.EvenParity();
            Assert.AreEqual(evenParity, ret);
        }
    }
}
