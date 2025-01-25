using System;


namespace Redbox.HAL.Component.Model.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class UsageAttribute : Attribute
    {
        public UsageAttribute(string template) => this.Template = template;

        public string Template { get; private set; }
    }
}
