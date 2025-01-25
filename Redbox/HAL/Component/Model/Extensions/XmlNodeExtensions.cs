using System;
using System.Xml;


namespace Redbox.HAL.Component.Model.Extensions
{
    public static class XmlNodeExtensions
    {
        public static void SelectSingleNodeAndSetValue<T>(this XmlNode node, string path, T value)
        {
            XmlNodeExtensions.GetTargetNode(node, path).InnerText = value.ToString();
        }

        public static void SelectSingleNodeAndSetInnerXml<T>(
          this XmlNode node,
          string path,
          T instance)
        {
            XmlNodeExtensions.GetTargetNode(node, path).InnerXml = instance.ToString();
        }

        public static T GetNodeValue<T>(this XmlNode node, T defaultValue)
        {
            try
            {
                if (node != null)
                {
                    if (!string.IsNullOrEmpty(node.InnerText))
                        return ConversionHelper.ChangeType<T>((object)ServiceLocator.Instance.GetService<IRuntimeService>().ExpandConstantMacros(node.InnerText));
                }
            }
            catch (ArgumentException ex)
            {
            }
            return defaultValue;
        }

        public static T GetAttributeValue<T>(this XmlNode node, string attributeName)
        {
            return node.GetAttributeValue<T>(attributeName, default(T));
        }

        public static T GetAttributeValue<T>(this XmlNode node, string attributeName, T defaultValue)
        {
            try
            {
                if (node != null)
                {
                    if (node.Attributes[attributeName] != null)
                        return ConversionHelper.ChangeType<T>((object)ServiceLocator.Instance.GetService<IRuntimeService>().ExpandConstantMacros(node.Attributes[attributeName].Value));
                }
            }
            catch (ArgumentException ex)
            {
            }
            return defaultValue;
        }

        public static void SetAttributeValue<T>(this XmlNode node, string attributeName, T value)
        {
            XmlAttribute attribute = node.Attributes[attributeName];
            if (attribute != null && !typeof(T).IsValueType && (object)value == null)
            {
                node.Attributes.Remove(attribute);
            }
            else
            {
                if (attribute == null)
                {
                    attribute = node.OwnerDocument.CreateAttribute(attributeName);
                    node.Attributes.Append(attribute);
                }
                attribute.Value = value.ToString();
            }
        }

        private static XmlNode GetTargetNode(XmlNode node, string path)
        {
            string[] strArray = path.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            XmlNode targetNode = node;
            foreach (string str in strArray)
            {
                XmlNode newChild = targetNode.SelectSingleNode(str);
                if (newChild == null)
                {
                    newChild = (XmlNode)targetNode.OwnerDocument.CreateElement(str);
                    targetNode.AppendChild(newChild);
                }
                targetNode = newChild;
            }
            return targetNode;
        }
    }
}
