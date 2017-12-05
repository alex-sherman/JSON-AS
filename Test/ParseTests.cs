using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JSON;

namespace Test
{
    [TestClass]
    public class ParseTests
    {
        [TestMethod]
        public void Null1()
        {
            JSONValue v = JSONConvert.Parse("null");
            Assert.AreEqual(JSONValueType.Null, v.Type);
            Assert.AreEqual(null, v.Value);
        }
        [TestMethod]
        public void Bool1()
        {
            JSONValue v = JSONConvert.Parse("true");
            Assert.AreEqual(JSONValueType.Boolean, v.Type);
            Assert.AreEqual(true, v.Value);
        }
        [TestMethod]
        public void Number1()
        {
            JSONValue v = JSONConvert.Parse("1.23");
            Assert.AreEqual(JSONValueType.Number, v.Type);
            Assert.AreEqual(1.23, v.Value);
        }
        [TestMethod]
        public void Number2()
        {
            JSONValue v = JSONConvert.Parse("-1.23");
            Assert.AreEqual(JSONValueType.Number, v.Type);
            Assert.AreEqual(-1.23, v.Value);
        }
        [TestMethod]
        public void Number3()
        {
            long value = ((long)int.MaxValue + 1);
            JSONValue v = JSONConvert.Parse("" + ((long)int.MaxValue + 1));
            Assert.AreEqual(JSONValueType.Number, v.Type);
            Assert.AreEqual(value, (long)v);
        }
        [TestMethod]
        public void Number4()
        {
            JSONValue v = JSONConvert.Parse("-1");
            Assert.AreEqual(JSONValueType.Number, v.Type);
            Assert.AreEqual(-1, (int)v);
        }
        [TestMethod]
        public void String1()
        {
            JSONValue v = JSONConvert.Parse("\"\\\"\"");
            Assert.AreEqual(JSONValueType.String, v.Type);
            Assert.AreEqual("\"", v.Value);
        }
        [TestMethod]
        public void String2()
        {
            JSONValue v = JSONConvert.Parse("\"\\n\"");
            Assert.AreEqual(JSONValueType.String, v.Type);
            Assert.AreEqual("\n", v.Value);
        }
        [TestMethod]
        public void String3()
        {
            JSONValue v = JSONConvert.Parse("\"itisjsonmydudes\"");
            Assert.AreEqual(JSONValueType.String, v.Type);
            Assert.AreEqual("itisjsonmydudes", v.Value);
        }
        [TestMethod]
        public void PrimArray1()
        {
            JSONArray arr = JSONConvert.Parse("[true, false, false, true]") as JSONArray;
            Assert.AreEqual(JSONValueType.Array, arr.Type);
            Assert.AreEqual(true, (bool)(arr)[0]);
            Assert.AreEqual(false, (bool)(arr)[1]);
            Assert.AreEqual(false, (bool)(arr)[2]);
            Assert.AreEqual(true, (bool)(arr)[3]);
        }
        [TestMethod]
        public void PrimArray2()
        {
            JSONArray arr = JSONConvert.Parse("[true, 3, \"faff\", -3.123]") as JSONArray;
            Assert.AreEqual(JSONValueType.Array, arr.Type);
            Assert.AreEqual(true, (bool)arr[0]);
            Assert.AreEqual(3, (int)arr[1]);
            Assert.AreEqual("faff", (string)arr[2]);
            Assert.AreEqual(-3.123, (double)arr[3]);
        }
        [TestMethod]
        public void Obj1()
        {
            JSONObject obj = JSONConvert.Parse("{faff: 123}") as JSONObject;
            Assert.AreEqual(JSONValueType.Object, obj.Type);
            Assert.AreEqual(123, (int)obj["faff"]);
        }
    }
}
