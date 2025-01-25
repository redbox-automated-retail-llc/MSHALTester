using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;


namespace Redbox.HAL.Component.Model.Extensions
{
    public static class StringExtensions
    {
        internal const BindingFlags GetObjectFromPathBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.GetField | BindingFlags.GetProperty;
        internal static readonly char[] PropertyPathSeparators = new char[1]
        {
      '.'
        };

        public static byte[] HexToBytes(string source)
        {
            List<byte> byteList = new List<byte>();
            int num = 0;
            int startIndex = 0;
            while (num < source.Length / 2)
            {
                byteList.Add(byte.Parse(source.Substring(startIndex, 2), NumberStyles.HexNumber));
                ++num;
                startIndex += 2;
            }
            return byteList.ToArray();
        }

        public static byte[] Base64ToBytes(string source) => Convert.FromBase64String(source);

        public static object GetValueForPath(string path, object rootObject)
        {
            return StringExtensions.GetObjectFromPath(path, rootObject, new int?());
        }

        public static void SetValueForPath(string path, object rootObject, object value)
        {
            object objectFromPath = StringExtensions.GetObjectFromPath(path, rootObject, new int?(1));
            if (objectFromPath == null)
                return;
            string[] strArray = path.Split(StringExtensions.PropertyPathSeparators);
            PropertyInfo property = objectFromPath.GetType().GetProperty(strArray[strArray.Length - 1], BindingFlags.Instance | BindingFlags.Public);
            property?.SetValue(objectFromPath, ConversionHelper.ChangeType(value, property.PropertyType), (object[])null);
        }

        public static Type ToType(string typeName) => Type.GetType(typeName);

        public static object ExtractIndex(string part)
        {
            if (part == null)
                return (object)null;
            Match match1 = Regex.Match(part, "\\[\"(?<indexer>.*?)\"\\]", RegexOptions.Singleline);
            Match match2 = Regex.Match(part, "\\[(?<indexer>[0-9]*?)\\]", RegexOptions.Singleline);
            object index = (object)null;
            if (match1.Success)
            {
                index = (object)match1.Groups["indexer"].Captures[0].Value;
            }
            else
            {
                int result;
                if (match2.Success && int.TryParse(match2.Groups["indexer"].Captures[0].Value, out result))
                    index = (object)result;
            }
            return index;
        }

        public static string ExtractCodeFromBrackets(string value, string prefix, string postfix)
        {
            int num1 = 0;
            int startIndex1 = -1;
            int num2 = -1;
            int startIndex2 = 0;
            do
            {
                string str1 = string.Empty;
                if (startIndex2 + prefix.Length < value.Length)
                    str1 = value.Substring(startIndex2, prefix.Length);
                string str2 = string.Empty;
                if (startIndex2 + str2.Length < value.Length)
                    str2 = value.Substring(startIndex2, postfix.Length);
                if (str1 == prefix)
                {
                    if (startIndex1 == -1)
                        startIndex1 = startIndex2 + prefix.Length;
                    ++num1;
                }
                else if (str2 == postfix)
                {
                    --num1;
                    if (num1 == 0)
                    {
                        num2 = startIndex2 - postfix.Length;
                        break;
                    }
                }
                ++startIndex2;
            }
            while (startIndex2 < value.Length);
            return startIndex1 == -1 || num2 == -1 ? (string)null : value.Substring(startIndex1, num2 - startIndex1 + postfix.Length);
        }

        private static object GetObjectFromPath(string path, object rootObject, int? depthAdjust)
        {
            if (path == null)
                return (object)null;
            string[] strArray = path.Split(StringExtensions.PropertyPathSeparators);
            if (strArray.Length == 0 || rootObject == null)
                return (object)null;
            int length = strArray.Length;
            if (depthAdjust.HasValue)
                length -= depthAdjust.Value;
            object target = rootObject;
            for (int index1 = 0; index1 < length; ++index1)
            {
                object[] args = (object[])null;
                string str = strArray[index1];
                object index2 = StringExtensions.ExtractIndex(str);
                if (index2 != null)
                {
                    str = str.Substring(0, str.IndexOf("["));
                    args = new object[1] { index2 };
                }
                try
                {
                    target = target.GetType().InvokeMember(str, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.GetField | BindingFlags.GetProperty, (Binder)null, target, args);
                    if (target == null)
                        break;
                }
                catch (Exception ex)
                {
                    return (object)null;
                }
            }
            return target;
        }

        internal static class ExtractIndexConstants
        {
            internal const string IndexerGroup = "indexer";
            internal const string TextIndexerRegex = "\\[\"(?<indexer>.*?)\"\\]";
            internal const string NumericIndexerRegex = "\\[(?<indexer>[0-9]*?)\\]";
        }

        internal static class NameValuePairConstants
        {
            internal const string NameGroup = "name";
            internal const string ValueGroup = "val";
            internal const string NameValuePairRegex = "(?n:((?<name>[\\w-_]*)=(?<val>[\\w-_\\\\\\/\\(\\)\\,'\\.\\s]*)))";
        }
    }
}
