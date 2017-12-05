using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSON
{
    public enum JSONValueType
    {
        Null,
        Boolean,
        Number,
        String,
        Array,
        Object
    }

    public class JSONObject : JSONValue, IEnumerable<KeyValuePair<string, JSONValue>>
    {
        Dictionary<string, JSONValue> dic;
        public JSONObject() : base()
        {
            dic = new Dictionary<string, JSONValue>();
            value = dic;
            Type = JSONValueType.Object;
        }
        public JSONValue this[string key]
        {
            get { return dic[key]; }
            set { dic[key] = value; }
        }

        public IEnumerator<KeyValuePair<string, JSONValue>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, JSONValue>>)dic).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, JSONValue>>)dic).GetEnumerator();
        }
    }
    public class JSONArray : JSONValue, IEnumerable<JSONValue>
    {
        List<JSONValue> arr;
        public JSONArray() : base()
        {
            arr = new List<JSONValue>();
            value = arr;
            Type = JSONValueType.Array;
        }
        public JSONValue this[int i]
        {
            get { return arr[i]; }
            set { arr[i] = value; }
        }
        public void Add(JSONValue value)
        {
            arr.Add(value);
        }

        public IEnumerator<JSONValue> GetEnumerator()
        {
            return ((IEnumerable<JSONValue>)arr).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<JSONValue>)arr).GetEnumerator();
        }
    }
    public class JSONValue
    {
        public static readonly Type[] NumericTypes = new Type[] {
            typeof(byte), typeof(ushort), typeof(short), typeof(uint), typeof(int), typeof(ulong), typeof(long),
            typeof(float), typeof(double)
        };
        protected JSONValue() { }
        public JSONValue(object value)
        {
            Value = value;
        }
        protected object value;
        public object Value
        {
            get { return value; }
            set
            {
                this.value = value;
                if (value == null)
                    Type = JSONValueType.Null;
                else if (value is bool)
                    Type = JSONValueType.Boolean;
                else if (NumericTypes.Contains(value.GetType()))
                    Type = JSONValueType.Number;
                else if (value is string)
                    Type = JSONValueType.String;
                else if (value is List<JSONValue>)
                    Type = JSONValueType.Array;
                else if (value is Dictionary<string, JSONValue>)
                    Type = JSONValueType.Object;
            }
        }
        public JSONValueType Type { get; protected set; } = JSONValueType.Null;

        #region Conversions
        public static implicit operator bool(JSONValue v)
        {
            return (bool)(v.Value ?? false);
        }
        public static implicit operator JSONValue(bool v)
        {
            return new JSONValue() { value = v, Type = JSONValueType.Boolean };
        }
        public static implicit operator byte(JSONValue v)
        {
            return (byte)(v.Value ?? 0);
        }
        public static implicit operator JSONValue(byte v)
        {
            return new JSONValue() { value = v, Type = JSONValueType.Number };
        }
        public static implicit operator ushort(JSONValue v)
        {
            return (ushort)(v.Value ?? 0);
        }
        public static implicit operator JSONValue(ushort v)
        {
            return new JSONValue() { value = v, Type = JSONValueType.Number };
        }
        public static implicit operator short(JSONValue v)
        {
            return (short)(v.Value ?? 0);
        }
        public static implicit operator JSONValue(short v)
        {
            return new JSONValue() { value = v, Type = JSONValueType.Number };
        }
        public static implicit operator uint(JSONValue v)
        {
            return (uint)(v.Value ?? 0);
        }
        public static implicit operator JSONValue(uint v)
        {
            return new JSONValue() { value = v, Type = JSONValueType.Number };
        }
        public static implicit operator int(JSONValue v)
        {
            return (int)(long)v;
        }
        public static implicit operator JSONValue(int v)
        {
            return new JSONValue() { value = v, Type = JSONValueType.Number };
        }
        public static implicit operator ulong(JSONValue v)
        {
            return (ulong)Convert.ChangeType(v.Value ?? 0, typeof(ulong));
        }
        public static implicit operator JSONValue(ulong v)
        {
            return new JSONValue() { value = v, Type = JSONValueType.Number };
        }
        public static implicit operator long(JSONValue v)
        {
            return (long)Convert.ChangeType(v.Value ?? 0, typeof(long));
        }
        public static implicit operator JSONValue(long v)
        {
            return new JSONValue() { value = v, Type = JSONValueType.Number };
        }
        public static implicit operator double(JSONValue v)
        {
            return (double)(v.Value ?? 0);
        }
        public static implicit operator JSONValue(double v)
        {
            return new JSONValue() { value = v, Type = JSONValueType.Number };
        }
        public static implicit operator float(JSONValue v)
        {
            return (float)(v.Value ?? 0);
        }
        public static implicit operator JSONValue(float v)
        {
            return new JSONValue() { value = v, Type = JSONValueType.Number };
        }
        public static implicit operator string(JSONValue v)
        {
            return v.Value as string;
        }
        public static implicit operator JSONValue(string v)
        {
            return new JSONValue() { value = v, Type = JSONValueType.String };
        }
        #endregion
    }
}
