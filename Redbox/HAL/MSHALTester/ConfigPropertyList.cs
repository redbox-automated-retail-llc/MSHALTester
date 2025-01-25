using Redbox.HAL.Client;
using Redbox.HAL.Common.GUI.Functions;
using Redbox.HAL.Component.Model;
using Redbox.HAL.Component.Model.Extensions;
using Redbox.HAL.MSHALTester.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;


namespace Redbox.HAL.MSHALTester
{
    public class ConfigPropertyList : UserControl, IDisposable
    {
        private DataGridViewCellStyle m_valueChangedStyle;
        private DataGridViewCellStyle m_defaultStyle;
        private bool IsDirty;
        private readonly HardwareService Service;
        private readonly XmlDocument m_camera;
        private readonly XmlDocument m_controller;
        private readonly List<ConfigItem> m_cameraConfig = new List<ConfigItem>();
        private readonly List<ConfigItem> m_controllerConfig = new List<ConfigItem>();
        private IContainer components;
        private Label m_descriptionLabel;
        private ListBox m_configItemListBox;
        private DataGridView m_propertyDataGridView;
        private DataGridViewTextBoxColumn Property;
        private DataGridViewTextBoxColumn Value;
        private Panel panel1;
        private Button m_alphSortButton;
        private Label m_filterLabel;
        private TextBox m_filterTextBox;
        private Button m_alphDescSortButton;
        private Button m_resetChangesButton;

        public event ConfigPropertyList.ConfigItemChanged OnConfigItemChange;

        public event ConfigPropertyList.SaveCompleted OnSave;

        public ConfigPropertyList(HardwareService service)
        {
            this.InitializeComponent();
            this.Service = service;
            this.m_defaultStyle = this.m_propertyDataGridView.DefaultCellStyle;
            this.m_valueChangedStyle = new DataGridViewCellStyle()
            {
                Font = new Font(this.Font.Name, this.Font.Size, FontStyle.Bold)
            };
            this.SetToolTips();
            this.m_configItemListBox.SelectedIndexChanged += new EventHandler(this.OnListBoxSelectionChanged);
            this.m_propertyDataGridView.CellEndEdit += new DataGridViewCellEventHandler(this.OnDataChanged);
            this.m_propertyDataGridView.SelectionChanged += new EventHandler(this.OnConfigPropertiesItemChanged);
            this.m_propertyDataGridView.CellClick += new DataGridViewCellEventHandler(this.OnButtonClick);
            this.m_configItemListBox.Items.Clear();
            this.m_propertyDataGridView.Rows.Clear();
            this.m_descriptionLabel.Text = string.Empty;
            this.m_camera = this.LoadConfigurationInner(Configuration.Camera, this.m_cameraConfig);
            if (this.m_camera != null)
                this.m_configItemListBox.Items.Add((object)Configuration.Camera);
            this.m_controller = this.LoadConfigurationInner(Configuration.Controller, this.m_controllerConfig);
            if (this.m_controller != null)
                this.m_configItemListBox.Items.Add((object)Configuration.Controller);
            this.IsDirty = false;
            this.Dock = DockStyle.Fill;
            this.Disposed += new EventHandler(this.ConfigPropertyList_Disposed);
        }

        private void ConfigPropertyList_Disposed(object sender, EventArgs e)
        {
            this.m_descriptionLabel.Text = string.Empty;
            this.m_configItemListBox.Items.Clear();
            this.m_propertyDataGridView.Rows.Clear();
        }

        public void Save()
        {
            if (this.IsDirty)
            {
                if (this.m_cameraConfig != null)
                {
                    if (!this.Service.SetConfiguration("camera", this.m_camera.InnerXml).Success)
                    {
                        this.SaveFailed();
                        return;
                    }
                    foreach (ConfigItem configItem in this.m_cameraConfig)
                        configItem.DefaultValue = configItem.Value;
                }
                if (this.m_controllerConfig != null)
                {
                    if (!this.Service.SetConfiguration("controller", this.m_controller.InnerXml).Success)
                    {
                        this.SaveFailed();
                        return;
                    }
                    foreach (ConfigItem configItem in this.m_controllerConfig)
                        configItem.DefaultValue = configItem.Value;
                }
            }
            this.m_propertyDataGridView.SuspendLayout();
            this.ListBoxSelectionChangedInner();
            this.m_propertyDataGridView.ResumeLayout();
            if (this.OnSave != null)
                this.OnSave();
            this.IsDirty = false;
        }

        private void SetToolTips()
        {
            ToolTip toolTip1 = new ToolTip();
            ToolTip toolTip2 = new ToolTip();
            ToolTip toolTip3 = new ToolTip();
            toolTip1.AutoPopDelay = toolTip2.AutoPopDelay = toolTip3.AutoPopDelay = 5000;
            toolTip1.InitialDelay = toolTip2.InitialDelay = toolTip3.InitialDelay = 1000;
            toolTip1.ReshowDelay = toolTip2.ReshowDelay = toolTip3.ReshowDelay = 500;
            toolTip1.ShowAlways = toolTip2.ShowAlways = toolTip3.ShowAlways = true;
            toolTip1.SetToolTip((Control)this.m_alphSortButton, "Sort Ascending");
            toolTip2.SetToolTip((Control)this.m_alphDescSortButton, "Sort Descending");
            toolTip3.SetToolTip((Control)this.m_resetChangesButton, "Reset All Configuration to Defaults");
        }

        private void SaveFailed()
        {
            int num = (int)MessageBox.Show("Save Failed!");
        }

        private XmlDocument LoadConfigurationInner(Configuration conf, List<ConfigItem> list)
        {
            HardwareCommandResult configuration = this.Service.GetConfiguration(conf.ToString());
            if (!configuration.Success)
                return (XmlDocument)null;
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.LoadXml(configuration.CommandMessages[0]);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log("An exception was thrown when trying to load configuration from Hal", ex, LogEntryType.Error);
                return (XmlDocument)null;
            }
            foreach (XmlNode childNode in xmlDocument.DocumentElement.ChildNodes)
            {
                if (childNode.ChildNodes.Count > 0 && childNode.ChildNodes[0].Name == "property")
                {
                    this.CreateSubNodeConfigItems(childNode, list);
                }
                else
                {
                    ConfigItem configItem = new ConfigItem(childNode);
                    if (configItem != null)
                        list.Add(configItem);
                }
            }
            return xmlDocument;
        }

        private void CreateSubNodeConfigItems(XmlNode node, List<ConfigItem> list)
        {
            string attributeValue = node.GetAttributeValue<string>("display-name");
            foreach (XmlNode childNode in node.ChildNodes)
            {
                ConfigItem configItem = new ConfigItem(childNode);
                configItem.DisplayName = attributeValue + " - " + configItem.DisplayName;
                list.Add(configItem);
            }
        }

        private void PopulateDataGrid(List<ConfigItem> list)
        {
            if (list.Count <= 0)
                return;
            this.m_propertyDataGridView.Rows.Clear();
            foreach (ConfigItem configItem in list)
            {
                if (configItem.CustomEditor == null)
                {
                    DataGridViewRow row = this.m_propertyDataGridView.Rows[this.m_propertyDataGridView.Rows.Add()];
                    row.Cells[0].Value = (object)configItem.DisplayName;
                    row.Cells[0].ReadOnly = true;
                    DataGridViewCell dataGridViewCell = (DataGridViewCell)null;
                    if (configItem.ValidValues != null)
                    {
                        dataGridViewCell = (DataGridViewCell)new DataGridViewComboBoxCell();
                        (dataGridViewCell as DataGridViewComboBoxCell).FlatStyle = FlatStyle.Flat;
                        foreach (string validValue in configItem.ValidValues)
                            (dataGridViewCell as DataGridViewComboBoxCell).Items.Add((object)validValue);
                        if (!(dataGridViewCell as DataGridViewComboBoxCell).Items.Contains(configItem.Value))
                            (dataGridViewCell as DataGridViewComboBoxCell).Items.Add(configItem.Value);
                    }
                    else if (configItem.ItemType == typeof(bool))
                    {
                        dataGridViewCell = (DataGridViewCell)new DataGridViewComboBoxCell();
                        (dataGridViewCell as DataGridViewComboBoxCell).FlatStyle = FlatStyle.Flat;
                        (dataGridViewCell as DataGridViewComboBoxCell).Items.Add((object)"Yes");
                        (dataGridViewCell as DataGridViewComboBoxCell).Items.Add((object)"No");
                        (dataGridViewCell as DataGridViewComboBoxCell).Value = configItem.Value.ToString().ToLower() == "true" ? (object)"Yes" : (object)"No";
                    }
                    if (dataGridViewCell != null)
                        row.Cells[1] = dataGridViewCell;
                    if (configItem.ItemType != typeof(bool))
                        row.Cells[1].Value = configItem.Value;
                    row.Cells[1].ReadOnly = configItem.ReadOnly;
                    if (configItem.ReadOnly)
                        row.DefaultCellStyle.ForeColor = Color.Gray;
                    row.Cells[1].Tag = (object)configItem;
                    this.SetStyle(row.Cells[1]);
                }
            }
        }

        private void OnListBoxSelectionChanged(object sender, EventArgs args)
        {
            this.ListBoxSelectionChangedInner();
        }

        private void ListBoxSelectionChangedInner()
        {
            this.PopulateDataGrid(this.m_configItemListBox.SelectedIndex == 0 ? this.m_cameraConfig : this.m_controllerConfig);
            this.m_filterTextBox.Text = string.Empty;
            this.ConfigPropertiesItemChangedInner();
        }

        private void OnDataChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = this.m_propertyDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            ConfigItem tag = (ConfigItem)cell.Tag;
            if (tag == null)
                return;
            object obj = cell.Value;
            if (tag.ItemType == typeof(bool))
                obj = cell.Value.ToString() == "Yes" ? (object)"True" : (object)"False";
            if (tag.ItemType == typeof(int) && !int.TryParse(obj.ToString(), out int _))
            {
                int num = (int)MessageBox.Show("Data entered is not an integer");
                cell.Value = tag.Value;
            }
            else
            {
                tag.Value = obj;
                this.SetStyle(cell);
                if (this.OnConfigItemChange != null)
                    this.OnConfigItemChange();
                this.IsDirty = true;
            }
        }

        private void SetStyle(DataGridViewCell cell)
        {
            if (cell == null || !(cell.Tag is ConfigItem tag) || tag.Value == null || tag.DefaultValue == null)
                return;
            if (tag.DefaultValue.Equals(tag.Value))
                cell.Style = this.m_defaultStyle;
            else
                cell.Style = this.m_valueChangedStyle;
        }

        private void OnConfigPropertiesItemChanged(object sender, EventArgs e)
        {
            this.ConfigPropertiesItemChangedInner();
        }

        private void ConfigPropertiesItemChangedInner()
        {
            DataGridViewSelectedRowCollection selectedRows = this.m_propertyDataGridView.SelectedRows;
            if (selectedRows == null || selectedRows.Count <= 0 || selectedRows[0].Cells[1].Tag == null)
                this.m_descriptionLabel.Text = string.Empty;
            else if (!(selectedRows[0].Cells[1].Tag is ConfigItem tag))
                this.m_descriptionLabel.Text = string.Empty;
            else
                this.m_descriptionLabel.Text = tag.Description;
        }

        private void OnReset(object sender, EventArgs e)
        {
            this.m_cameraConfig.FindAll((Predicate<ConfigItem>)(x => !x.ReadOnly && x.CustomEditor == null)).ForEach((Action<ConfigItem>)(x => x.Value = x.DefaultValue));
            this.m_controllerConfig.FindAll((Predicate<ConfigItem>)(x => !x.ReadOnly && x.CustomEditor == null)).ForEach((Action<ConfigItem>)(x => x.Value = x.DefaultValue));
            this.ListBoxSelectionChangedInner();
            this.IsDirty = false;
            int num = (int)MessageBox.Show("Successfully Reset Properties to Previously Loaded Values.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void OnButtonClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex > this.m_propertyDataGridView.Rows.Count - 1 || e.ColumnIndex > 1)
                return;
            DataGridViewButtonCell cell = this.m_propertyDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
        }

        private void m_alphSortButton_Click(object sender, EventArgs e)
        {
            this.m_propertyDataGridView.Sort((IComparer)new ConfigPropertyList.RowComparerAscending());
        }

        private void m_alphSortDescButton_Click(object sender, EventArgs e)
        {
            this.m_propertyDataGridView.Sort((IComparer)new ConfigPropertyList.RowComparerDescending());
        }

        private void m_filterTextBox_TextChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in (IEnumerable)this.m_propertyDataGridView.Rows)
            {
                if (string.IsNullOrEmpty(this.m_filterTextBox.Text))
                    row.Visible = true;
                else
                    row.Visible = row.Cells[0].Value.ToString().ToLower().Contains(this.m_filterTextBox.Text.ToLower());
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.m_descriptionLabel = new Label();
            this.m_configItemListBox = new ListBox();
            this.m_propertyDataGridView = new DataGridView();
            this.Property = new DataGridViewTextBoxColumn();
            this.Value = new DataGridViewTextBoxColumn();
            this.panel1 = new Panel();
            this.m_resetChangesButton = new Button();
            this.m_alphDescSortButton = new Button();
            this.m_filterLabel = new Label();
            this.m_filterTextBox = new TextBox();
            this.m_alphSortButton = new Button();
            ((ISupportInitialize)this.m_propertyDataGridView).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            this.m_descriptionLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.m_descriptionLabel.BorderStyle = BorderStyle.FixedSingle;
            this.m_descriptionLabel.Location = new Point(6, 431);
            this.m_descriptionLabel.Name = "m_descriptionLabel";
            this.m_descriptionLabel.Padding = new Padding(10);
            this.m_descriptionLabel.Size = new Size(342, 97);
            this.m_descriptionLabel.TabIndex = 0;
            this.m_descriptionLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.m_configItemListBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.m_configItemListBox.FormattingEnabled = true;
            this.m_configItemListBox.Location = new Point(6, 3);
            this.m_configItemListBox.Name = "m_configItemListBox";
            this.m_configItemListBox.Size = new Size(342, 56);
            this.m_configItemListBox.TabIndex = 1;
            this.m_propertyDataGridView.AllowUserToAddRows = false;
            this.m_propertyDataGridView.AllowUserToDeleteRows = false;
            this.m_propertyDataGridView.AllowUserToResizeRows = false;
            this.m_propertyDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.m_propertyDataGridView.BackgroundColor = SystemColors.Window;
            this.m_propertyDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_propertyDataGridView.Columns.AddRange((DataGridViewColumn)this.Property, (DataGridViewColumn)this.Value);
            this.m_propertyDataGridView.Location = new Point(6, 101);
            this.m_propertyDataGridView.MultiSelect = false;
            this.m_propertyDataGridView.Name = "m_propertyDataGridView";
            this.m_propertyDataGridView.RowHeadersVisible = false;
            this.m_propertyDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.m_propertyDataGridView.Size = new Size(342, 327);
            this.m_propertyDataGridView.TabIndex = 2;
            this.Property.HeaderText = "Property";
            this.Property.Name = "Property";
            this.Property.Width = 125;
            this.Value.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.panel1.Controls.Add((Control)this.m_resetChangesButton);
            this.panel1.Controls.Add((Control)this.m_alphDescSortButton);
            this.panel1.Controls.Add((Control)this.m_filterLabel);
            this.panel1.Controls.Add((Control)this.m_filterTextBox);
            this.panel1.Controls.Add((Control)this.m_alphSortButton);
            this.panel1.Location = new Point(6, 65);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(342, 30);
            this.panel1.TabIndex = 3;
            this.m_resetChangesButton.BackColor = Color.Transparent;
            this.m_resetChangesButton.BackgroundImageLayout = ImageLayout.None;
            this.m_resetChangesButton.Enabled = false;
            this.m_resetChangesButton.Image = (Image)Resources.undo;
            this.m_resetChangesButton.Location = new Point(72, 3);
            this.m_resetChangesButton.Name = "m_resetChangesButton";
            this.m_resetChangesButton.Size = new Size(29, 27);
            this.m_resetChangesButton.TabIndex = 4;
            this.m_resetChangesButton.UseVisualStyleBackColor = false;
            this.m_resetChangesButton.Visible = false;
            this.m_resetChangesButton.Click += new EventHandler(this.OnReset);
            this.m_alphDescSortButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.m_alphDescSortButton.BackColor = Color.Transparent;
            this.m_alphDescSortButton.BackgroundImageLayout = ImageLayout.None;
            this.m_alphDescSortButton.Image = (Image)Resources.sortDescending2;
            this.m_alphDescSortButton.Location = new Point(37, 3);
            this.m_alphDescSortButton.Name = "m_alphDescSortButton";
            this.m_alphDescSortButton.Size = new Size(29, 27);
            this.m_alphDescSortButton.TabIndex = 5;
            this.m_alphDescSortButton.Tag = (object)"Sort Descending";
            this.m_alphDescSortButton.UseVisualStyleBackColor = false;
            this.m_alphDescSortButton.Click += new EventHandler(this.m_alphSortDescButton_Click);
            this.m_filterLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.m_filterLabel.Location = new Point(107, 6);
            this.m_filterLabel.Name = "m_filterLabel";
            this.m_filterLabel.Size = new Size(50, 20);
            this.m_filterLabel.TabIndex = 4;
            this.m_filterLabel.Text = "Filter: ";
            this.m_filterLabel.TextAlign = ContentAlignment.MiddleRight;
            this.m_filterTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.m_filterTextBox.Location = new Point(163, 7);
            this.m_filterTextBox.Name = "m_filterTextBox";
            this.m_filterTextBox.Size = new Size(176, 20);
            this.m_filterTextBox.TabIndex = 1;
            this.m_filterTextBox.TextChanged += new EventHandler(this.m_filterTextBox_TextChanged);
            this.m_alphSortButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.m_alphSortButton.BackColor = Color.Transparent;
            this.m_alphSortButton.BackgroundImageLayout = ImageLayout.None;
            this.m_alphSortButton.Image = (Image)Resources.sortAscending2;
            this.m_alphSortButton.Location = new Point(3, 3);
            this.m_alphSortButton.Name = "m_alphSortButton";
            this.m_alphSortButton.Size = new Size(28, 27);
            this.m_alphSortButton.TabIndex = 0;
            this.m_alphSortButton.Tag = (object)"Sort Ascending";
            this.m_alphSortButton.UseVisualStyleBackColor = false;
            this.m_alphSortButton.Click += new EventHandler(this.m_alphSortButton_Click);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.WhiteSmoke;
            this.Controls.Add((Control)this.panel1);
            this.Controls.Add((Control)this.m_propertyDataGridView);
            this.Controls.Add((Control)this.m_configItemListBox);
            this.Controls.Add((Control)this.m_descriptionLabel);
            this.Name = nameof(ConfigPropertyList);
            this.Size = new Size(351, 538);
            ((ISupportInitialize)this.m_propertyDataGridView).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }

        public delegate void ConfigItemChanged();

        public delegate void SaveCompleted();

        private class RowComparerAscending : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                return (x as DataGridViewRow).Cells[0].Value.ToString().CompareTo((y as DataGridViewRow).Cells[0].Value.ToString());
            }
        }

        private class RowComparerDescending : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                return (y as DataGridViewRow).Cells[0].Value.ToString().CompareTo((x as DataGridViewRow).Cells[0].Value.ToString());
            }
        }
    }
}
