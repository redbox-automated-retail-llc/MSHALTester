using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Redbox.HAL.Component.Model
{
    [Serializable]
    public sealed class ErrorList : List<Error>, ICloneable
    {
        public static ErrorList NewFromStrings(string[] errors)
        {
            ErrorList errorList = new ErrorList();
            if (errors == null)
                return errorList;
            foreach (string error in errors)
                errorList.Add(Error.Parse(error));
            return errorList;
        }

        public bool ContainsCode(string code)
        {
            return this.Find((Predicate<Error>)(each => each.Code == code)) != null;
        }

        public bool ContainsError() => this.Find((Predicate<Error>)(each => !each.IsWarning)) != null;

        public int RemoveCode(string code)
        {
            return this.RemoveAll((Predicate<Error>)(each => each.Code == code));
        }

        public void Dump(TextWriter writer) => writer.WriteLine(this.FormatList());

        public void DumpToLog() => LogHelper.Instance.Log(this.FormatList());

        public object Clone()
        {
            ErrorList errorList = new ErrorList();
            errorList.AddRange((IEnumerable<Error>)this);
            return (object)errorList;
        }

        public int ErrorCount => this.FindAll((Predicate<Error>)(each => !each.IsWarning)).Count;

        public int WarningCount => this.FindAll((Predicate<Error>)(each => each.IsWarning)).Count;

        private string FormatList()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("-- Errors --");
            this.ForEach((Action<Error>)(e =>
            {
                builder.AppendLine(string.Format(" Code        : {0}", (object)e.Code));
                builder.AppendLine(string.Format(" Description : {0}", (object)e.Description));
                builder.AppendLine(string.Format(" Details     : {0}", (object)e.Details));
            }));
            return builder.ToString();
        }
    }
}
