using Redbox.HAL.Client;
using Redbox.HAL.Component.Model.Extensions;
using System;
using System.Xml;


namespace Redbox.HAL.MSHALTester
{
    public sealed class DecksConfigurationManager
    {
        private XmlNode DecksNode;
        private readonly HardwareService Service;

        public DecksConfigurationManager(HardwareService service)
        {
            this.Service = service != null ? service : throw new ArgumentException("HardwareService object cannot be null.");
            this.LoadConfiguration();
        }

        public XmlNodeList FindAllDeckNodes() => this.DecksNode.ChildNodes;

        public XmlNode FindDeckNode(int deckNumber)
        {
            foreach (XmlNode childNode in this.DecksNode.ChildNodes)
            {
                if (childNode.GetAttributeValue<int>("Number") == deckNumber)
                    return childNode;
            }
            return (XmlNode)null;
        }

        public bool FlushChanges(bool refresh)
        {
            HardwareCommandResult hardwareCommandResult = this.Service.SetConfiguration("controller", this.Document.OuterXml);
            if (hardwareCommandResult.Success & refresh)
                this.LoadConfiguration();
            return hardwareCommandResult.Success;
        }

        public XmlDocument Document { get; private set; }

        private void LoadConfiguration()
        {
            HardwareCommandResult configuration = this.Service.GetConfiguration("controller");
            if (!configuration.Success)
                throw new Exception("Unable to get configuration from HAL.");
            this.Document = new XmlDocument();
            this.Document.LoadXml(configuration.CommandMessages[0]);
            this.DecksNode = this.Document.DocumentElement.SelectSingleNode("property[@display-name='Decks']");
        }
    }
}
