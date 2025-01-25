using Redbox.HAL.Client;
using Redbox.HAL.Common.GUI.Functions;
using Redbox.HAL.Component.Model.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;


namespace Redbox.HAL.MSHALTester
{
    public class DeckConfigurationDetailForm : Form
    {
        private bool m_showDumpSlot;
        private bool m_allowSlotDataEdit;
        private IContainer components;
        private Button m_okButton;
        private Button m_cancelButton;
        private ErrorProvider m_errorProvider;
        private Label m_numberLabel;
        private CheckBox m_isQlmDeckCheckBox;
        private Label m_slotsPerQuadrantLabel;
        private Label m_approachOffsetLabel;
        private Label m_sellThruOffsetLabel;
        private Label m_sellThruSlotsLabel;
        private Label m_slotWidthLabel;
        private Label m_numberOfSlotsLabel;
        private Label m_yOffsetLabel;
        private Label label9;
        private TextBox m_slotsPerQuadrantTextBox;
        private TextBox m_approachOffsetTextBox;
        private TextBox m_sellThruOffsetTextBox;
        private TextBox m_sellThruSlotsTextBox;
        private TextBox m_slotWidthTextBox;
        private TextBox m_numberOfSlotsTextBox;
        private TextBox m_yoffsetTextBox;
        private TextBox m_numberTextBox;
        private Button m_computeQuadrantsButton;
        private CheckBox m_hasDumpSlotCheckBox;
        private Button m_editSlotDataButton;
        private DataGridView m_quadrantDataGridView;
        private DataGridViewTextBoxColumn Quadrant;
        private DataGridViewTextBoxColumn Offset;
        private DataGridViewTextBoxColumn Start;
        private DataGridViewTextBoxColumn End;
        private DataGridViewCheckBoxColumn Excluded;

        public DeckConfigurationDetailForm(bool allowExcludeUpdates, DecksConfigurationManager manager)
        {
            this.InitializeComponent();
            this.AllowExcludeUpdates = allowExcludeUpdates;
            this.Manager = manager;
            this.m_quadrantDataGridView.Columns[DeckConfigurationDetailColumns.Excluded].ReadOnly = !this.AllowExcludeUpdates;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AllowSlotDataEdit
        {
            get => this.m_allowSlotDataEdit;
            set
            {
                this.m_allowSlotDataEdit = value;
                this.m_editSlotDataButton.Visible = this.m_allowSlotDataEdit;
                this.m_editSlotDataButton.Enabled = this.m_allowSlotDataEdit;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? Number
        {
            get
            {
                int result;
                return int.TryParse(this.m_numberTextBox.Text, out result) ? new int?(result) : new int?();
            }
            set
            {
                this.m_numberTextBox.Clear();
                if (!value.HasValue)
                    return;
                this.m_numberTextBox.Text = value.ToString();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? YOffset
        {
            get
            {
                int result;
                return int.TryParse(this.m_yoffsetTextBox.Text, out result) ? new int?(result) : new int?();
            }
            set
            {
                if (!value.HasValue)
                    return;
                this.m_yoffsetTextBox.Clear();
                this.m_yoffsetTextBox.Text = value.ToString();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? NumberOfSlots
        {
            get
            {
                int result;
                return int.TryParse(this.m_numberOfSlotsTextBox.Text, out result) ? new int?(result) : new int?();
            }
            set
            {
                this.m_numberOfSlotsTextBox.Clear();
                if (!value.HasValue)
                    return;
                this.m_numberOfSlotsTextBox.Text = value.ToString();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Decimal? SlotWidth
        {
            get
            {
                Decimal result;
                return Decimal.TryParse(this.m_slotWidthTextBox.Text, out result) ? new Decimal?(result) : new Decimal?();
            }
            set
            {
                this.m_slotWidthTextBox.Clear();
                if (!value.HasValue)
                    return;
                this.m_slotWidthTextBox.Text = value.ToString();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? SellThruSlots
        {
            get
            {
                int result;
                return int.TryParse(this.m_sellThruSlotsTextBox.Text, out result) ? new int?(result) : new int?();
            }
            set
            {
                this.m_sellThruSlotsTextBox.Clear();
                if (!value.HasValue)
                    return;
                this.m_sellThruSlotsTextBox.Text = value.ToString();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? SellThruOffset
        {
            get
            {
                int result;
                return int.TryParse(this.m_sellThruOffsetTextBox.Text, out result) ? new int?(result) : new int?();
            }
            set
            {
                this.m_sellThruOffsetTextBox.Clear();
                if (!value.HasValue)
                    return;
                this.m_sellThruOffsetTextBox.Text = value.ToString();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? ApproachOffset
        {
            get
            {
                int result;
                return int.TryParse(this.m_approachOffsetTextBox.Text, out result) ? new int?(result) : new int?();
            }
            set
            {
                this.m_approachOffsetTextBox.Clear();
                if (!value.HasValue)
                    return;
                this.m_approachOffsetTextBox.Text = value.ToString();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? SlotsPerQuadrant
        {
            get
            {
                int result;
                return int.TryParse(this.m_slotsPerQuadrantTextBox.Text, out result) ? new int?(result) : new int?();
            }
            set
            {
                this.m_slotsPerQuadrantTextBox.Clear();
                if (!value.HasValue)
                    return;
                this.m_slotsPerQuadrantTextBox.Text = value.ToString();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsQlmDeck
        {
            get => this.m_isQlmDeckCheckBox.Checked;
            set
            {
                this.m_isQlmDeckCheckBox.Checked = value;
                this.m_computeQuadrantsButton.Enabled = !value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasDumpSlot
        {
            get => this.m_hasDumpSlotCheckBox.Checked;
            set => this.m_hasDumpSlotCheckBox.Checked = value;
        }

        public void EnableAddRemove() => this.m_isQlmDeckCheckBox.Enabled = false;

        public bool ShowDumpSlot
        {
            get => this.m_showDumpSlot;
            set
            {
                this.m_showDumpSlot = value;
                this.m_hasDumpSlotCheckBox.Visible = this.m_showDumpSlot;
                this.m_hasDumpSlotCheckBox.Checked = value;
            }
        }

        public HardwareService Service { get; set; }

        public DecksConfigurationManager Manager { get; private set; }

        private void OnLoad(object sender, EventArgs e)
        {
            if (!this.AllowExcludeUpdates)
            {
                this.m_quadrantDataGridView.Columns[DeckConfigurationDetailColumns.Excluded].DefaultCellStyle.BackColor = Color.LightGray;
                this.m_quadrantDataGridView.Columns[DeckConfigurationDetailColumns.Excluded].DefaultCellStyle.ForeColor = Color.Gray;
                ((DataGridViewCheckBoxColumn)this.m_quadrantDataGridView.Columns[DeckConfigurationDetailColumns.Excluded]).FlatStyle = FlatStyle.Flat;
            }
            if (this.m_isQlmDeckCheckBox.Checked)
            {
                this.m_quadrantDataGridView.Columns[DeckConfigurationDetailColumns.Excluded].Visible = false;
                this.m_approachOffsetLabel.Visible = true;
                this.m_approachOffsetTextBox.Visible = true;
                this.m_editSlotDataButton.Enabled = false;
            }
            this.m_hasDumpSlotCheckBox.Enabled = this.ShowDumpSlot;
            this.WorkingNode = this.Manager.FindDeckNode(this.Number.Value).CloneNode(true);
            this.RefreshView();
        }

        private void OnOK(object sender, EventArgs e)
        {
            this.m_errorProvider.SetError((Control)this.m_numberTextBox, string.Empty);
            this.m_errorProvider.SetError((Control)this.m_yoffsetTextBox, string.Empty);
            this.m_errorProvider.SetError((Control)this.m_slotWidthTextBox, string.Empty);
            this.m_errorProvider.SetError((Control)this.m_numberOfSlotsTextBox, string.Empty);
            if (!this.YOffset.HasValue)
                this.m_errorProvider.SetError((Control)this.m_yoffsetTextBox, "Y Offset is required field.");
            else if (!this.SlotWidth.HasValue)
            {
                this.m_errorProvider.SetError((Control)this.m_slotWidthTextBox, "Slot Width is a required field.");
            }
            else
            {
                List<int> intList = new List<int>();
                XmlNode deckNode = this.Manager.FindDeckNode(this.Number.Value);
                for (int index = 0; index < this.WorkingNode.ChildNodes.Count; ++index)
                {
                    XmlNode childNode1 = deckNode.ChildNodes[index];
                    XmlNode childNode2 = this.WorkingNode.ChildNodes[index];
                    if (!childNode1.GetAttributeValue<bool>("IsExcluded", false) && this.IsQuadrantExcluded(index))
                        intList.Add(index);
                }
                if (intList.Count == 0)
                {
                    this.UpdateOriginalNode(deckNode);
                    this.Manager.FlushChanges(false);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else if (DialogResult.Cancel == MessageBox.Show((IWin32Window)this, "This will update the inventory for all slots in excluded quadrants to EMPTY.  You MUST remove ALL discs in the excluded quadrants prior to excluding the quadrant(s)." + Environment.NewLine + "To cancel this operation, click ‘Cancel.’", "You are about to exclude sections of a deck.", MessageBoxButtons.OKCancel))
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
                else
                {
                    this.UpdateOriginalNode(deckNode);
                    this.Manager.FlushChanges(false);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void OnCancel(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private XmlNode UpdateOriginalNode(XmlNode originalNode)
        {
            originalNode.SetAttributeValue<int>("Number", this.Number.Value);
            originalNode.SetAttributeValue<int>("Offset", this.YOffset.Value);
            originalNode.SetAttributeValue<int>("NumberOfSlots", this.NumberOfSlots.Value);
            originalNode.SetAttributeValue<Decimal>("SlotWidth", this.SlotWidth.Value);
            originalNode.SetAttributeValue<bool>("IsQlm", this.IsQlmDeck);
            originalNode.SetAttributeValue<int?>("SellThruSlots", this.SellThruSlots);
            originalNode.SetAttributeValue<int?>("SellThruOffset", this.SellThruOffset);
            originalNode.SetAttributeValue<int?>("ApproachOffset", this.ApproachOffset);
            originalNode.SetAttributeValue<int?>("SlotsPerQuadrant", this.SlotsPerQuadrant);
            for (int index = 0; index < this.WorkingNode.ChildNodes.Count; ++index)
            {
                XmlNode childNode1 = originalNode.ChildNodes[index];
                XmlNode childNode2 = this.WorkingNode.ChildNodes[index];
                childNode1.SetAttributeValue<bool>("IsExcluded", this.IsQuadrantExcluded(index));
                childNode1.SetAttributeValue<object>("Offset", this.m_quadrantDataGridView[1, index].Value);
            }
            return originalNode;
        }

        private void RefreshView()
        {
            this.m_quadrantDataGridView.SuspendLayout();
            this.m_quadrantDataGridView.Rows.Clear();
            int num = 1;
            foreach (XmlNode childNode in this.WorkingNode.ChildNodes)
            {
                int attributeValue1 = childNode.GetAttributeValue<int>("Offset");
                int? attributeValue2 = childNode.GetAttributeValue<int?>("StartSlot");
                int? attributeValue3 = childNode.GetAttributeValue<int?>("EndSlot");
                bool attributeValue4 = childNode.GetAttributeValue<bool>("IsExcluded", false);
                this.m_quadrantDataGridView.Rows.Add((object)num++, (object)attributeValue1, (object)attributeValue2, (object)attributeValue3, (object)attributeValue4);
            }
            this.m_quadrantDataGridView.ResumeLayout();
        }

        private void OnComputeQuadrants(object sender, EventArgs e)
        {
            if (this.SlotWidth.HasValue)
            {
                Decimal? slotWidth = this.SlotWidth;
                Decimal num1 = 0M;
                if (!(slotWidth.GetValueOrDefault() <= num1 & slotWidth.HasValue))
                {
                    this.m_errorProvider.SetError((Control)this.m_slotWidthTextBox, string.Empty);
                    ComputeQuadrantsForm computeQuadrantsForm1 = new ComputeQuadrantsForm();
                    int? numberOfSlots1 = this.NumberOfSlots;
                    int num2 = 90;
                    computeQuadrantsForm1.NumberOfQuadrants = new int?(numberOfSlots1.GetValueOrDefault() == num2 & numberOfSlots1.HasValue ? 6 : 12);
                    int? numberOfSlots2 = this.NumberOfSlots;
                    int num3 = 90;
                    computeQuadrantsForm1.SlotsPerQuadrant = new int?(numberOfSlots2.GetValueOrDefault() == num3 & numberOfSlots2.HasValue ? 15 : 6);
                    ComputeQuadrantsForm computeQuadrantsForm2 = computeQuadrantsForm1;
                    if (DialogResult.Cancel == computeQuadrantsForm2.ShowDialog())
                        return;
                    CommonFunctions.ComputeQuadrants(computeQuadrantsForm2.StartOffset, computeQuadrantsForm2.NumberOfQuadrants, this.SellThruSlots, this.SellThruOffset, computeQuadrantsForm2.SlotsPerQuadrant, this.SlotWidth, this.WorkingNode);
                    this.RefreshView();
                    return;
                }
            }
            this.m_errorProvider.SetError((Control)this.m_slotWidthTextBox, "Slot Width must be a positive value.");
        }

        private void m_editSlotDataButton_Click(object sender, EventArgs e)
        {
            if (!this.m_allowSlotDataEdit)
                return;
            int num = (int)new EditSlotData(this.Service, this.Number.Value, this.Manager).ShowDialog();
            this.WorkingNode = this.Manager.FindDeckNode(this.Number.Value).CloneNode(true);
            this.RefreshView();
        }

        private void m_quadrantDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != DeckConfigurationDetailColumns.Offset || e.RowIndex == -1)
                return;
            NumberPadForm numberPadForm1 = new NumberPadForm();
            numberPadForm1.Text = "Offset";
            NumberPadForm numberPadForm2 = numberPadForm1;
            if (numberPadForm2.ShowDialog() != DialogResult.OK || string.IsNullOrEmpty(numberPadForm2.Number))
                return;
            this.m_quadrantDataGridView.EditingControl.Text = numberPadForm2.Number;
        }

        private void m_quadrantDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.AllowExcludeUpdates || e.RowIndex == -1 || e.ColumnIndex != DeckConfigurationDetailColumns.Excluded)
                return;
            if ((bool)this.m_quadrantDataGridView.CurrentCell.FormattedValue)
                this.m_quadrantDataGridView.CurrentCell.Value = (object)false;
            else
                this.m_quadrantDataGridView.CurrentCell.Value = (object)true;
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b')
                return;
            e.Handled = true;
        }

        private bool IsQuadrantExcluded(int quadrant)
        {
            DataGridViewCheckBoxCell cell = this.m_quadrantDataGridView.Rows[quadrant].Cells[DeckConfigurationDetailColumns.Excluded] as DataGridViewCheckBoxCell;
            return cell.Value.ToString().Equals((string)cell.TrueValue, StringComparison.CurrentCultureIgnoreCase);
        }

        private bool AllowExcludeUpdates { get; set; }

        private XmlNode WorkingNode { get; set; }

        private void OnEditControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            this.m_quadrantDataGridView.EditingControl.KeyPress += new KeyPressEventHandler(this.OnKeyPress);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = (IContainer)new System.ComponentModel.Container();
            DataGridViewCellStyle gridViewCellStyle = new DataGridViewCellStyle();
            this.m_okButton = new Button();
            this.m_cancelButton = new Button();
            this.m_errorProvider = new ErrorProvider(this.components);
            this.m_numberLabel = new Label();
            this.m_yOffsetLabel = new Label();
            this.m_numberOfSlotsLabel = new Label();
            this.m_slotWidthLabel = new Label();
            this.m_sellThruSlotsLabel = new Label();
            this.m_sellThruOffsetLabel = new Label();
            this.m_approachOffsetLabel = new Label();
            this.m_slotsPerQuadrantLabel = new Label();
            this.m_isQlmDeckCheckBox = new CheckBox();
            this.label9 = new Label();
            this.m_numberTextBox = new TextBox();
            this.m_yoffsetTextBox = new TextBox();
            this.m_numberOfSlotsTextBox = new TextBox();
            this.m_slotWidthTextBox = new TextBox();
            this.m_sellThruSlotsTextBox = new TextBox();
            this.m_sellThruOffsetTextBox = new TextBox();
            this.m_approachOffsetTextBox = new TextBox();
            this.m_slotsPerQuadrantTextBox = new TextBox();
            this.m_computeQuadrantsButton = new Button();
            this.m_hasDumpSlotCheckBox = new CheckBox();
            this.m_editSlotDataButton = new Button();
            this.m_quadrantDataGridView = new DataGridView();
            this.Quadrant = new DataGridViewTextBoxColumn();
            this.Offset = new DataGridViewTextBoxColumn();
            this.Start = new DataGridViewTextBoxColumn();
            this.End = new DataGridViewTextBoxColumn();
            this.Excluded = new DataGridViewCheckBoxColumn();
            ((ISupportInitialize)this.m_errorProvider).BeginInit();
            ((ISupportInitialize)this.m_quadrantDataGridView).BeginInit();
            this.SuspendLayout();
            this.m_okButton.Location = new Point(260, 456);
            this.m_okButton.Name = "m_okButton";
            this.m_okButton.Size = new Size(75, 23);
            this.m_okButton.TabIndex = 19;
            this.m_okButton.Text = "OK";
            this.m_okButton.UseVisualStyleBackColor = true;
            this.m_okButton.Click += new EventHandler(this.OnOK);
            this.m_cancelButton.DialogResult = DialogResult.Cancel;
            this.m_cancelButton.Location = new Point(341, 456);
            this.m_cancelButton.Name = "m_cancelButton";
            this.m_cancelButton.Size = new Size(75, 23);
            this.m_cancelButton.TabIndex = 20;
            this.m_cancelButton.Text = "Cancel";
            this.m_cancelButton.UseVisualStyleBackColor = true;
            this.m_cancelButton.Click += new EventHandler(this.OnCancel);
            this.m_errorProvider.ContainerControl = (ContainerControl)this;
            this.m_numberLabel.AutoSize = true;
            this.m_numberLabel.Location = new Point(12, 12);
            this.m_numberLabel.Name = "m_numberLabel";
            this.m_numberLabel.Size = new Size(47, 13);
            this.m_numberLabel.TabIndex = 0;
            this.m_numberLabel.Text = "Number:";
            this.m_yOffsetLabel.AutoSize = true;
            this.m_yOffsetLabel.Location = new Point(12, 36);
            this.m_yOffsetLabel.Name = "m_yOffsetLabel";
            this.m_yOffsetLabel.Size = new Size(48, 13);
            this.m_yOffsetLabel.TabIndex = 2;
            this.m_yOffsetLabel.Text = "Y Offset:";
            this.m_numberOfSlotsLabel.AutoSize = true;
            this.m_numberOfSlotsLabel.Location = new Point(12, 62);
            this.m_numberOfSlotsLabel.Name = "m_numberOfSlotsLabel";
            this.m_numberOfSlotsLabel.Size = new Size(87, 13);
            this.m_numberOfSlotsLabel.TabIndex = 4;
            this.m_numberOfSlotsLabel.Text = "Number Of Slots:";
            this.m_slotWidthLabel.AutoSize = true;
            this.m_slotWidthLabel.Location = new Point(12, 88);
            this.m_slotWidthLabel.Name = "m_slotWidthLabel";
            this.m_slotWidthLabel.Size = new Size(59, 13);
            this.m_slotWidthLabel.TabIndex = 6;
            this.m_slotWidthLabel.Text = "Slot Width:";
            this.m_sellThruSlotsLabel.AutoSize = true;
            this.m_sellThruSlotsLabel.Location = new Point(12, 114);
            this.m_sellThruSlotsLabel.Name = "m_sellThruSlotsLabel";
            this.m_sellThruSlotsLabel.Size = new Size(78, 13);
            this.m_sellThruSlotsLabel.TabIndex = 8;
            this.m_sellThruSlotsLabel.Text = "Sell Thru Slots:";
            this.m_sellThruOffsetLabel.AutoSize = true;
            this.m_sellThruOffsetLabel.Location = new Point(12, 140);
            this.m_sellThruOffsetLabel.Name = "m_sellThruOffsetLabel";
            this.m_sellThruOffsetLabel.Size = new Size(83, 13);
            this.m_sellThruOffsetLabel.TabIndex = 10;
            this.m_sellThruOffsetLabel.Text = "Sell Thru Offset:";
            this.m_approachOffsetLabel.AutoSize = true;
            this.m_approachOffsetLabel.Location = new Point(12, 188);
            this.m_approachOffsetLabel.Name = "m_approachOffsetLabel";
            this.m_approachOffsetLabel.Size = new Size(87, 13);
            this.m_approachOffsetLabel.TabIndex = 12;
            this.m_approachOffsetLabel.Text = "Approach Offset:";
            this.m_approachOffsetLabel.Visible = false;
            this.m_slotsPerQuadrantLabel.AutoSize = true;
            this.m_slotsPerQuadrantLabel.Location = new Point(12, 164);
            this.m_slotsPerQuadrantLabel.Name = "m_slotsPerQuadrantLabel";
            this.m_slotsPerQuadrantLabel.Size = new Size(99, 13);
            this.m_slotsPerQuadrantLabel.TabIndex = 14;
            this.m_slotsPerQuadrantLabel.Text = "Slots Per Quadrant:";
            this.m_isQlmDeckCheckBox.AutoSize = true;
            this.m_isQlmDeckCheckBox.Enabled = false;
            this.m_isQlmDeckCheckBox.Location = new Point(117, 218);
            this.m_isQlmDeckCheckBox.Name = "m_isQlmDeckCheckBox";
            this.m_isQlmDeckCheckBox.Size = new Size(95, 17);
            this.m_isQlmDeckCheckBox.TabIndex = 16;
            this.m_isQlmDeckCheckBox.Text = "Is QLM Deck?";
            this.m_isQlmDeckCheckBox.UseVisualStyleBackColor = true;
            this.label9.AutoSize = true;
            this.label9.BackColor = Color.Yellow;
            this.label9.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.label9.Location = new Point(12, 260);
            this.label9.Name = "label9";
            this.label9.Size = new Size(265, 15);
            this.label9.TabIndex = 17;
            this.label9.Text = "Double Click The Offset to Edit Slot Data";
            this.m_numberTextBox.Location = new Point(117, 9);
            this.m_numberTextBox.Name = "m_numberTextBox";
            this.m_numberTextBox.ReadOnly = true;
            this.m_numberTextBox.Size = new Size(100, 20);
            this.m_numberTextBox.TabIndex = 1;
            this.m_yoffsetTextBox.Location = new Point(117, 36);
            this.m_yoffsetTextBox.Name = "m_yoffsetTextBox";
            this.m_yoffsetTextBox.Size = new Size(100, 20);
            this.m_yoffsetTextBox.TabIndex = 3;
            this.m_numberOfSlotsTextBox.Location = new Point(117, 62);
            this.m_numberOfSlotsTextBox.Name = "m_numberOfSlotsTextBox";
            this.m_numberOfSlotsTextBox.ReadOnly = true;
            this.m_numberOfSlotsTextBox.Size = new Size(100, 20);
            this.m_numberOfSlotsTextBox.TabIndex = 5;
            this.m_slotWidthTextBox.Location = new Point(117, 88);
            this.m_slotWidthTextBox.Name = "m_slotWidthTextBox";
            this.m_slotWidthTextBox.Size = new Size(100, 20);
            this.m_slotWidthTextBox.TabIndex = 7;
            this.m_sellThruSlotsTextBox.Location = new Point(117, 114);
            this.m_sellThruSlotsTextBox.Name = "m_sellThruSlotsTextBox";
            this.m_sellThruSlotsTextBox.ReadOnly = true;
            this.m_sellThruSlotsTextBox.Size = new Size(100, 20);
            this.m_sellThruSlotsTextBox.TabIndex = 9;
            this.m_sellThruOffsetTextBox.Location = new Point(117, 140);
            this.m_sellThruOffsetTextBox.Name = "m_sellThruOffsetTextBox";
            this.m_sellThruOffsetTextBox.ReadOnly = true;
            this.m_sellThruOffsetTextBox.Size = new Size(100, 20);
            this.m_sellThruOffsetTextBox.TabIndex = 11;
            this.m_approachOffsetTextBox.Location = new Point(117, 188);
            this.m_approachOffsetTextBox.Name = "m_approachOffsetTextBox";
            this.m_approachOffsetTextBox.Size = new Size(100, 20);
            this.m_approachOffsetTextBox.TabIndex = 13;
            this.m_approachOffsetTextBox.Visible = false;
            this.m_slotsPerQuadrantTextBox.Location = new Point(117, 164);
            this.m_slotsPerQuadrantTextBox.Name = "m_slotsPerQuadrantTextBox";
            this.m_slotsPerQuadrantTextBox.ReadOnly = true;
            this.m_slotsPerQuadrantTextBox.Size = new Size(100, 20);
            this.m_slotsPerQuadrantTextBox.TabIndex = 15;
            this.m_computeQuadrantsButton.Location = new Point(287, 250);
            this.m_computeQuadrantsButton.Name = "m_computeQuadrantsButton";
            this.m_computeQuadrantsButton.Size = new Size(129, 23);
            this.m_computeQuadrantsButton.TabIndex = 24;
            this.m_computeQuadrantsButton.Text = "Compute Quadrants...";
            this.m_computeQuadrantsButton.UseVisualStyleBackColor = true;
            this.m_computeQuadrantsButton.Click += new EventHandler(this.OnComputeQuadrants);
            this.m_hasDumpSlotCheckBox.AutoSize = true;
            this.m_hasDumpSlotCheckBox.Location = new Point(117, 241);
            this.m_hasDumpSlotCheckBox.Name = "m_hasDumpSlotCheckBox";
            this.m_hasDumpSlotCheckBox.Size = new Size(103, 17);
            this.m_hasDumpSlotCheckBox.TabIndex = 25;
            this.m_hasDumpSlotCheckBox.Text = "Has Dump Slot?";
            this.m_hasDumpSlotCheckBox.UseVisualStyleBackColor = true;
            this.m_editSlotDataButton.Enabled = false;
            this.m_editSlotDataButton.Location = new Point(287, 221);
            this.m_editSlotDataButton.Name = "m_editSlotDataButton";
            this.m_editSlotDataButton.Size = new Size(129, 23);
            this.m_editSlotDataButton.TabIndex = 26;
            this.m_editSlotDataButton.Text = "Edit Slot Data";
            this.m_editSlotDataButton.UseVisualStyleBackColor = true;
            this.m_editSlotDataButton.Visible = false;
            this.m_editSlotDataButton.Click += new EventHandler(this.m_editSlotDataButton_Click);
            this.m_quadrantDataGridView.AllowUserToAddRows = false;
            this.m_quadrantDataGridView.AllowUserToDeleteRows = false;
            this.m_quadrantDataGridView.AllowUserToResizeRows = false;
            this.m_quadrantDataGridView.BackgroundColor = Color.White;
            this.m_quadrantDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_quadrantDataGridView.Columns.AddRange((DataGridViewColumn)this.Quadrant, (DataGridViewColumn)this.Offset, (DataGridViewColumn)this.Start, (DataGridViewColumn)this.End, (DataGridViewColumn)this.Excluded);
            this.m_quadrantDataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            this.m_quadrantDataGridView.Location = new Point(12, 281);
            this.m_quadrantDataGridView.Name = "m_quadrantDataGridView";
            this.m_quadrantDataGridView.RowHeadersVisible = false;
            this.m_quadrantDataGridView.RowHeadersWidth = 25;
            this.m_quadrantDataGridView.ScrollBars = ScrollBars.Vertical;
            this.m_quadrantDataGridView.Size = new Size(404, 169);
            this.m_quadrantDataGridView.TabIndex = 27;
            this.m_quadrantDataGridView.CellDoubleClick += new DataGridViewCellEventHandler(this.m_quadrantDataGridView_CellDoubleClick);
            this.m_quadrantDataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.OnEditControlShowing);
            this.m_quadrantDataGridView.CellContentClick += new DataGridViewCellEventHandler(this.m_quadrantDataGridView_CellContentClick);
            this.Quadrant.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.Quadrant.FillWeight = 75f;
            this.Quadrant.HeaderText = "Quadrant";
            this.Quadrant.Name = "Quadrant";
            this.Quadrant.ReadOnly = true;
            this.Quadrant.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.Quadrant.Width = 57;
            this.Offset.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.Offset.FillWeight = 75f;
            this.Offset.HeaderText = "Offset";
            this.Offset.Name = "Offset";
            this.Offset.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.Offset.Width = 41;
            this.Start.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.Start.FillWeight = 75f;
            this.Start.HeaderText = "Start Slot";
            this.Start.Name = "Start";
            this.Start.ReadOnly = true;
            this.Start.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.Start.Width = 56;
            this.End.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.End.FillWeight = 75f;
            this.End.HeaderText = "End Slot";
            this.End.Name = "End";
            this.End.ReadOnly = true;
            this.End.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.End.Width = 53;
            this.Excluded.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridViewCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            gridViewCellStyle.NullValue = (object)false;
            this.Excluded.DefaultCellStyle = gridViewCellStyle;
            this.Excluded.FalseValue = (object)"false";
            this.Excluded.FillWeight = 60f;
            this.Excluded.HeaderText = "Excluded?";
            this.Excluded.IndeterminateValue = (object)"null";
            this.Excluded.Name = "Excluded";
            this.Excluded.TrueValue = (object)"true";
            this.AcceptButton = (IButtonControl)this.m_okButton;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = (IButtonControl)this.m_cancelButton;
            this.ClientSize = new Size(428, 491);
            this.ControlBox = false;
            this.Controls.Add((Control)this.m_quadrantDataGridView);
            this.Controls.Add((Control)this.m_editSlotDataButton);
            this.Controls.Add((Control)this.m_hasDumpSlotCheckBox);
            this.Controls.Add((Control)this.m_computeQuadrantsButton);
            this.Controls.Add((Control)this.m_slotsPerQuadrantTextBox);
            this.Controls.Add((Control)this.m_approachOffsetTextBox);
            this.Controls.Add((Control)this.m_sellThruOffsetTextBox);
            this.Controls.Add((Control)this.m_sellThruSlotsTextBox);
            this.Controls.Add((Control)this.m_slotWidthTextBox);
            this.Controls.Add((Control)this.m_numberOfSlotsTextBox);
            this.Controls.Add((Control)this.m_yoffsetTextBox);
            this.Controls.Add((Control)this.m_numberTextBox);
            this.Controls.Add((Control)this.label9);
            this.Controls.Add((Control)this.m_isQlmDeckCheckBox);
            this.Controls.Add((Control)this.m_slotsPerQuadrantLabel);
            this.Controls.Add((Control)this.m_approachOffsetLabel);
            this.Controls.Add((Control)this.m_sellThruOffsetLabel);
            this.Controls.Add((Control)this.m_sellThruSlotsLabel);
            this.Controls.Add((Control)this.m_slotWidthLabel);
            this.Controls.Add((Control)this.m_numberOfSlotsLabel);
            this.Controls.Add((Control)this.m_yOffsetLabel);
            this.Controls.Add((Control)this.m_numberLabel);
            this.Controls.Add((Control)this.m_cancelButton);
            this.Controls.Add((Control)this.m_okButton);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Name = nameof(DeckConfigurationDetailForm);
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Edit Deck Configuration";
            this.Load += new EventHandler(this.OnLoad);
            ((ISupportInitialize)this.m_errorProvider).EndInit();
            ((ISupportInitialize)this.m_quadrantDataGridView).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
