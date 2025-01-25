using Redbox.HAL.Client;
using Redbox.HAL.Component.Model.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;


namespace Redbox.HAL.MSHALTester
{
    public class DeckConfigurationForm : Form
    {
        private readonly HardwareService m_hardwareService;
        private IContainer components;
        private Button m_okButton;
        private DataGridView m_sandGrid;
        private Button m_propertiesButton;
        private Button m_fixSlotAlignment;
        private DataGridViewTextBoxColumn m_deckTextColumn;
        private DataGridViewTextBoxColumn m_offsetTextColumn;
        private DataGridViewTextBoxColumn m_slotWidthTextColumn;
        private DataGridViewTextBoxColumn m_numSlotsTextColumn;
        private DataGridViewTextBoxColumn m_isQlmTextColumn;
        private DataGridViewTextBoxColumn m_fillerColumn;
        private Button m_exitButton;

        public DeckConfigurationForm(HardwareService service)
        {
            this.Manager = new DecksConfigurationManager(service);
            this.m_hardwareService = service;
            this.InitializeComponent();
            this.RefreshList();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DecksConfigurationManager Manager { get; private set; }

        private void OnOK(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnProperties(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = this.m_sandGrid.SelectedRows[0];
            if (selectedRow == null || !(selectedRow.Tag is XmlNode tag))
                return;
            bool attributeValue = tag.GetAttributeValue<bool>("IsQlm");
            if (new DeckConfigurationDetailForm(true, this.Manager)
            {
                Number = tag.GetAttributeValue<int?>("Number"),
                YOffset = tag.GetAttributeValue<int?>("Offset"),
                NumberOfSlots = tag.GetAttributeValue<int?>("NumberOfSlots"),
                SlotWidth = tag.GetAttributeValue<Decimal?>("SlotWidth"),
                SellThruSlots = tag.GetAttributeValue<int?>("SellThruSlots"),
                SellThruOffset = tag.GetAttributeValue<int?>("SellThruOffset"),
                ApproachOffset = tag.GetAttributeValue<int?>("ApproachOffset"),
                SlotsPerQuadrant = tag.GetAttributeValue<int?>("SlotsPerQuadrant"),
                IsQlmDeck = attributeValue,
                Service = this.m_hardwareService,
                ShowDumpSlot = false
            }.ShowDialog() != DialogResult.OK)
                return;
            this.RefreshList();
            this.m_okButton.Enabled = true;
        }

        private void OnSelectedRowChanged(object sender, EventArgs e)
        {
            this.m_propertiesButton.Enabled = this.m_sandGrid.SelectedRows.Count > 0;
        }

        private void RefreshList()
        {
            this.m_sandGrid.SuspendLayout();
            this.m_sandGrid.Rows.Clear();
            foreach (XmlNode allDeckNode in this.Manager.FindAllDeckNodes())
            {
                int attributeValue1 = allDeckNode.GetAttributeValue<int>("Offset");
                int attributeValue2 = allDeckNode.GetAttributeValue<int>("Number");
                bool attributeValue3 = allDeckNode.GetAttributeValue<bool>("IsQlm");
                int attributeValue4 = allDeckNode.GetAttributeValue<int>("NumberOfSlots");
                Decimal attributeValue5 = allDeckNode.GetAttributeValue<Decimal>("SlotWidth");
                DataGridViewRow dataGridViewRow1 = new DataGridViewRow();
                dataGridViewRow1.Tag = (object)allDeckNode;
                DataGridViewRow dataGridViewRow2 = dataGridViewRow1;
                dataGridViewRow2.Cells.AddRange((DataGridViewCell[])new DataGridViewTextBoxCell[5]
                {
          new DataGridViewTextBoxCell(),
          new DataGridViewTextBoxCell(),
          new DataGridViewTextBoxCell(),
          new DataGridViewTextBoxCell(),
          new DataGridViewTextBoxCell()
                });
                int index = this.m_sandGrid.Rows.Add(dataGridViewRow2);
                this.m_sandGrid.Rows[index].Cells[0].Value = (object)attributeValue2;
                this.m_sandGrid.Rows[index].Cells[1].Value = (object)attributeValue1;
                this.m_sandGrid.Rows[index].Cells[2].Value = (object)attributeValue5;
                this.m_sandGrid.Rows[index].Cells[3].Value = (object)attributeValue4;
                this.m_sandGrid.Rows[index].Cells[4].Value = (object)attributeValue3;
            }
            this.m_sandGrid.ResumeLayout();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (new EditSlotData(this.m_hardwareService, this.Manager).ShowDialog() != DialogResult.OK)
                return;
            this.RefreshList();
        }

        private void m_exitButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.m_okButton = new Button();
            this.m_sandGrid = new DataGridView();
            this.m_deckTextColumn = new DataGridViewTextBoxColumn();
            this.m_offsetTextColumn = new DataGridViewTextBoxColumn();
            this.m_slotWidthTextColumn = new DataGridViewTextBoxColumn();
            this.m_numSlotsTextColumn = new DataGridViewTextBoxColumn();
            this.m_isQlmTextColumn = new DataGridViewTextBoxColumn();
            this.m_fillerColumn = new DataGridViewTextBoxColumn();
            this.m_propertiesButton = new Button();
            this.m_fixSlotAlignment = new Button();
            this.m_exitButton = new Button();
            ((ISupportInitialize)this.m_sandGrid).BeginInit();
            this.SuspendLayout();
            this.m_okButton.BackColor = Color.LightGray;
            this.m_okButton.Enabled = false;
            this.m_okButton.Location = new Point(434, 12);
            this.m_okButton.Name = "m_okButton";
            this.m_okButton.Size = new Size(75, 23);
            this.m_okButton.TabIndex = 5;
            this.m_okButton.Text = "Save";
            this.m_okButton.UseVisualStyleBackColor = false;
            this.m_okButton.Visible = false;
            this.m_okButton.Click += new EventHandler(this.OnOK);
            this.m_sandGrid.AllowUserToAddRows = false;
            this.m_sandGrid.AllowUserToDeleteRows = false;
            this.m_sandGrid.BackgroundColor = SystemColors.Window;
            this.m_sandGrid.Columns.AddRange((DataGridViewColumn)this.m_deckTextColumn, (DataGridViewColumn)this.m_offsetTextColumn, (DataGridViewColumn)this.m_slotWidthTextColumn, (DataGridViewColumn)this.m_numSlotsTextColumn, (DataGridViewColumn)this.m_isQlmTextColumn, (DataGridViewColumn)this.m_fillerColumn);
            this.m_sandGrid.Location = new Point(12, 12);
            this.m_sandGrid.Name = "m_sandGrid";
            this.m_sandGrid.ReadOnly = true;
            this.m_sandGrid.RowHeadersWidth = 24;
            this.m_sandGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.m_sandGrid.Size = new Size(416, 289);
            this.m_sandGrid.TabIndex = 0;
            this.m_sandGrid.SelectionChanged += new EventHandler(this.OnSelectedRowChanged);
            this.m_deckTextColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.m_deckTextColumn.Frozen = true;
            this.m_deckTextColumn.HeaderText = "Deck #";
            this.m_deckTextColumn.Name = "m_deckTextColumn";
            this.m_deckTextColumn.ReadOnly = true;
            this.m_deckTextColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.m_deckTextColumn.Width = 49;
            this.m_offsetTextColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.m_offsetTextColumn.Frozen = true;
            this.m_offsetTextColumn.HeaderText = "Y Axis Offset";
            this.m_offsetTextColumn.Name = "m_offsetTextColumn";
            this.m_offsetTextColumn.ReadOnly = true;
            this.m_offsetTextColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.m_offsetTextColumn.Width = 73;
            this.m_slotWidthTextColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.m_slotWidthTextColumn.Frozen = true;
            this.m_slotWidthTextColumn.HeaderText = "Slot Width";
            this.m_slotWidthTextColumn.Name = "m_slotWidthTextColumn";
            this.m_slotWidthTextColumn.ReadOnly = true;
            this.m_slotWidthTextColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.m_slotWidthTextColumn.Width = 62;
            this.m_numSlotsTextColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.m_numSlotsTextColumn.Frozen = true;
            this.m_numSlotsTextColumn.HeaderText = "# Slots";
            this.m_numSlotsTextColumn.Name = "m_numSlotsTextColumn";
            this.m_numSlotsTextColumn.ReadOnly = true;
            this.m_numSlotsTextColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.m_numSlotsTextColumn.Width = 46;
            this.m_isQlmTextColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.m_isQlmTextColumn.Frozen = true;
            this.m_isQlmTextColumn.HeaderText = "QLM Deck?";
            this.m_isQlmTextColumn.Name = "m_isQlmTextColumn";
            this.m_isQlmTextColumn.ReadOnly = true;
            this.m_isQlmTextColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.m_isQlmTextColumn.Width = 71;
            this.m_fillerColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.m_fillerColumn.HeaderText = "";
            this.m_fillerColumn.Name = "m_fillerColumn";
            this.m_fillerColumn.ReadOnly = true;
            this.m_propertiesButton.BackColor = Color.LightGray;
            this.m_propertiesButton.Enabled = false;
            this.m_propertiesButton.Location = new Point(179, 307);
            this.m_propertiesButton.Name = "m_propertiesButton";
            this.m_propertiesButton.Size = new Size(75, 23);
            this.m_propertiesButton.TabIndex = 3;
            this.m_propertiesButton.Text = "Properties...";
            this.m_propertiesButton.UseVisualStyleBackColor = false;
            this.m_propertiesButton.Click += new EventHandler(this.OnProperties);
            this.m_fixSlotAlignment.BackColor = Color.LightGray;
            this.m_fixSlotAlignment.Location = new Point(274, 307);
            this.m_fixSlotAlignment.Name = "m_fixSlotAlignment";
            this.m_fixSlotAlignment.Size = new Size(100, 23);
            this.m_fixSlotAlignment.TabIndex = 7;
            this.m_fixSlotAlignment.Text = "Fix Slot Alignment";
            this.m_fixSlotAlignment.UseVisualStyleBackColor = false;
            this.m_fixSlotAlignment.Click += new EventHandler(this.button1_Click);
            this.m_exitButton.BackColor = Color.LightGray;
            this.m_exitButton.Location = new Point(434, 302);
            this.m_exitButton.Name = "m_exitButton";
            this.m_exitButton.Size = new Size(75, 32);
            this.m_exitButton.TabIndex = 8;
            this.m_exitButton.Text = "Close";
            this.m_exitButton.UseVisualStyleBackColor = false;
            this.m_exitButton.Click += new EventHandler(this.m_exitButton_Click);
            this.AcceptButton = (IButtonControl)this.m_okButton;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(521, 342);
            this.ControlBox = false;
            this.Controls.Add((Control)this.m_exitButton);
            this.Controls.Add((Control)this.m_fixSlotAlignment);
            this.Controls.Add((Control)this.m_propertiesButton);
            this.Controls.Add((Control)this.m_sandGrid);
            this.Controls.Add((Control)this.m_okButton);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Name = nameof(DeckConfigurationForm);
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Deck Configuration Editor";
            ((ISupportInitialize)this.m_sandGrid).EndInit();
            this.ResumeLayout(false);
        }
    }
}
