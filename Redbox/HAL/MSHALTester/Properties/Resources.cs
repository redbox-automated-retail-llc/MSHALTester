using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;


namespace Redbox.HAL.MSHALTester.Properties
{
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [DebuggerNonUserCode]
    [CompilerGenerated]
    internal class Resources
    {
        private static ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal Resources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager
        {
            get
            {
                if (Redbox.HAL.MSHALTester.Properties.Resources.resourceMan == null)
                    Redbox.HAL.MSHALTester.Properties.Resources.resourceMan = new ResourceManager("Redbox.HAL.MSHALTester.Properties.Resources", typeof(Redbox.HAL.MSHALTester.Properties.Resources).Assembly);
                return Redbox.HAL.MSHALTester.Properties.Resources.resourceMan;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get => Redbox.HAL.MSHALTester.Properties.Resources.resourceCulture;
            set => Redbox.HAL.MSHALTester.Properties.Resources.resourceCulture = value;
        }

        internal static string DestDeckSlotInvalid
        {
            get
            {
                return Redbox.HAL.MSHALTester.Properties.Resources.ResourceManager.GetString(nameof(DestDeckSlotInvalid), Redbox.HAL.MSHALTester.Properties.Resources.resourceCulture);
            }
        }

        internal static Bitmap sortAscending2
        {
            get
            {
                return (Bitmap)Redbox.HAL.MSHALTester.Properties.Resources.ResourceManager.GetObject(nameof(sortAscending2), Redbox.HAL.MSHALTester.Properties.Resources.resourceCulture);
            }
        }

        internal static Bitmap sortDescending2
        {
            get
            {
                return (Bitmap)Redbox.HAL.MSHALTester.Properties.Resources.ResourceManager.GetObject(nameof(sortDescending2), Redbox.HAL.MSHALTester.Properties.Resources.resourceCulture);
            }
        }

        internal static string SourceDeckSlotInvalid
        {
            get
            {
                return Redbox.HAL.MSHALTester.Properties.Resources.ResourceManager.GetString(nameof(SourceDeckSlotInvalid), Redbox.HAL.MSHALTester.Properties.Resources.resourceCulture);
            }
        }

        internal static Bitmap undo
        {
            get => (Bitmap)Redbox.HAL.MSHALTester.Properties.Resources.ResourceManager.GetObject(nameof(undo), Redbox.HAL.MSHALTester.Properties.Resources.resourceCulture);
        }
    }
}
