using System;
using System.Text;


namespace Redbox.HAL.Component.Model
{
    [Serializable]
    public sealed class Error
    {
        private readonly string m_code;
        private readonly bool m_isWarning;
        private readonly string m_details;
        private readonly string m_description;

        public static Error Parse(string error)
        {
            string codeFromBrackets = Error.ExtractCodeFromBrackets(error, "[", "]");
            int startIndex = error.IndexOf("]");
            string[] strArray = error.Substring(startIndex).Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string str = (string)null;
            if (strArray.Length > 1)
                str = strArray[1];
            string description = strArray[0].Substring(strArray[0].IndexOf(":") + 1);
            string details = str;
            int num = strArray[0].StartsWith("WARNING") ? 1 : 0;
            return new Error(codeFromBrackets, description, details, num != 0);
        }

        public static Error NewError(string code, string description, Exception e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(e.ToString());
            if (e.InnerException != null)
                stringBuilder.AppendFormat("\r\n\r\nInner Exception:\r\n---------------------------\r\n{0}\r\n", (object)e.InnerException);
            stringBuilder.AppendFormat("\r\n\r\nStack Trace:\r\n----------------------------\r\n{0}\r\n", (object)e.StackTrace);
            return new Error(code, description, stringBuilder.ToString(), false);
        }

        public static Error NewError(string code, string description, string details)
        {
            return new Error(code, description, details, false);
        }

        public static Error NewWarning(string code, string description, string details)
        {
            return new Error(code, description, details, true);
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1}: {2}", (object)this.Code, this.IsWarning ? (object)"WARNING" : (object)"ERROR", (object)this.Description);
        }

        public string Code => this.m_code;

        public string Details => this.m_details;

        public bool IsWarning => this.m_isWarning;

        public string Description => this.m_description;

        private Error(string code, string description, string details, bool isWarning)
        {
            this.m_code = code;
            this.m_details = details;
            this.m_isWarning = isWarning;
            this.m_description = description;
        }

        private static string ExtractCodeFromBrackets(string value, string prefix, string postfix)
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
    }
}
