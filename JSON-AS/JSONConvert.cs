using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSON
{
    public class JSONParseError : Exception
    {
        public JSONParseError(int characterIndex) { }
    }
    class JSONParser : IEnumerator<char>
    {
        static char[] NumberCharacters = Enumerable.Range(0, 9).Select(i => (char)('0' + i)).Union(new char[] { '-', '.' }).ToArray();
        static Dictionary<char, char> EscapeReplace = new Dictionary<char, char>()
        {
            { '"', '"' },
            { '\\', '\\' },
            { '/', '/' },
            { 'b', '\b' },
            { 'f', '\f' },
            { 'n', '\n' },
            { 'r', '\r' },
            { 't', '\t' },
        };
        public JSONParser(IEnumerable<char> input)
        {
            en = input.GetEnumerator();
            if (!en.MoveNext())
                throw new JSONParseError(0);
            Index = 0;
        }
        #region Enumeration
        IEnumerator<char> en;
        public int Index { get; private set; }

        public char Current => en.Current;
        public char Previous { get; private set; }

        public char CurrentNWS { get { SkipWhiteSpace(); return Current; } }

        object IEnumerator.Current => en.Current;

        public void Dispose()
        {
            en.Dispose();
        }

        public bool MoveNext()
        {
            Index++;
            Previous = Current;
            return en.MoveNext();
        }

        public void Reset()
        {
            en.Reset();
        }
        #endregion

        #region Parsing
        static char[] whitespace = { ' ', '\t', '\r', '\n' };
        bool SkipWhiteSpace(bool throwOnEnd = true)
        {
            bool success = true;
            while (whitespace.Contains(Current) && (success = en.MoveNext())) { }
            if (!success && throwOnEnd)
                throw new JSONParseError(Index);
            return success;
        }
        void Match(char character, bool throwOnFail = true)
        {
            if (CurrentNWS == character)
                MoveNext();
            else if (throwOnFail)
                throw new JSONParseError(Index);
        }
        void MatchOrThrow(string str)
        {
            foreach (var c in str)
                Match(c);
        }
        JSONValue ParseNumber()
        {
            int index = Index;
            string value = BuildWhile(() => NumberCharacters.Contains(Current));
            if (ulong.TryParse(value, out ulong ulval))
                return ulval;
            if (long.TryParse(value, out long lval))
                return lval;
            if (double.TryParse(value, out double dval))
                return dval;
            throw new JSONParseError(index);
        }
        JSONValue ParseString()
        {
            Match('\"');
            string output = BuildWhile(() => Previous == '\\' || Current != '\"',
                () => Previous == '\\' ? EscapeReplace[Current] : (Current == '\\' ? null : (char?)Current));
            Match('\"');
            return output;
        }
        JSONArray ParseArray()
        {
            JSONArray output = new JSONArray();
            Match('[');
            while (CurrentNWS != ']')
            {
                output.Add(ParseValue());
                Match(',', false);
            }
            return output;
        }
        JSONObject ParseObject()
        {
            JSONObject output = new JSONObject();
            Match('{');
            while (CurrentNWS != '}')
            {
                string key = BuildWhile(() => CurrentNWS != ':');
                Match(':');
                output[key] = ParseValue();
            }
            return output;
        }
        string BuildWhile(Func<bool> pred, Func<char?> trans = null)
        {
            StringBuilder b = new StringBuilder();
            while (pred())
            {
                if (trans == null)
                    b.Append(Current);
                else
                    b.Append(trans.Invoke());
                if (!MoveNext())
                    break;
            }
            return b.ToString();
        }
        public JSONValue ParseValue()
        {
            SkipWhiteSpace();
            switch (CurrentNWS)
            {
                case '{':
                    return ParseObject();
                case '[':
                    return ParseArray();
                case '\"':
                    return ParseString();
                case 't':
                    MatchOrThrow("true");
                    return true;
                case 'f':
                    MatchOrThrow("false");
                    return false;
                case 'n':
                    MatchOrThrow("null");
                    return new JSONValue(null);
                default:
                    if (NumberCharacters.Contains(CurrentNWS))
                        return ParseNumber();
                    throw new JSONParseError(Index);
            }
        }
        #endregion
    }

    public static class JSONConvert
    {
        public static JSONValue Parse(IEnumerable<char> input)
        {
            var en = new JSONParser(input);
            return en.ParseValue();
        }
        public static string Serialize(object input, StringBuilder sb = null)
        {
            sb = sb ?? new StringBuilder();
            if (input == null)
            {
                return "null";
            }
            else if (input is bool)
                return ((bool)input) ? "true" : "false";
            else if (JSONValue.NumericTypes.Contains(input.GetType()))
                return input.ToString();
            else if (input is string)
            {
                sb.Append('\"');
                sb.Append(input as string);
                sb.Append('\"');
            }
            else if (input is IDictionary)
            {
                sb.Append("{");
                var dic = (input as IDictionary).GetEnumerator();
                while (dic.MoveNext())
                {
                    sb.Append(dic.Key);
                    sb.Append(": ");
                    sb.Append(Serialize(dic.Value));
                }
                sb.Append("}");
            }
            else if (input is ICollection)
            {
                sb.Append("[");
                List<string> elements = new List<string>();
                foreach (var obj in input as ICollection)
                {
                    elements.Add(Serialize(obj));
                }
                sb.Append(string.Join(", ", elements.ToArray()));
                sb.Append("]");
            }
            return sb.ToString();
        }
    }
}
