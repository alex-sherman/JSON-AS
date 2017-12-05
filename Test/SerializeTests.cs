using JSON;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class SerializeTests
    {
        [TestMethod]
        public void SerializeNum1()
        {
            Assert.AreEqual("123", JSONConvert.Serialize(123));
        }
        [TestMethod]
        public void SerializeNum2()
        {
            Assert.AreEqual("-123", JSONConvert.Serialize(-123));
        }
        [TestMethod]
        public void SerializeNum3()
        {
            Assert.AreEqual("-0.1234", JSONConvert.Serialize(-0.1234));
        }
        [TestMethod]
        public void SerializeNull()
        {
            Assert.AreEqual("null", JSONConvert.Serialize(null));
        }
        [TestMethod]
        public void SerializeString1()
        {
            Assert.AreEqual("\"null\"", JSONConvert.Serialize("null"));
        }
        [TestMethod]
        public void SerializeArr1()
        {
            Assert.AreEqual("[1, 2, 3, 4]", JSONConvert.Serialize(new List<int>() { 1, 2, 3, 4 }));
        }
        [TestMethod]
        public void SerializeArr2()
        {
            Assert.AreEqual("[\"faff\", 2.3, 3, {derp: null}]",
                JSONConvert.Serialize(new List<object>() { "faff", 2.3, 3, new Dictionary<string, object>() { { "derp", null } } }));
        }
        [TestMethod]
        public void SerializeObj1()
        {
            Assert.AreEqual("{faff: 123}", JSONConvert.Serialize(new Dictionary<string, int>() { { "faff", 123 } }));
        }
        [TestMethod]
        public void SerializeObj2()
        {
            Assert.AreEqual("{faff: {123: 1.5}}", JSONConvert.Serialize(new Dictionary<string, Dictionary<int, float>>()
                { { "faff", new Dictionary<int, float>() { { 123, 1.5f } } } }
            ));
        }
    }
}
