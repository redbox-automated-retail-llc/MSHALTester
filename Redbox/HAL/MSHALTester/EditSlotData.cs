using Redbox.HAL.Client;
using Redbox.HAL.Client.Services;
using Redbox.HAL.Common.GUI.Functions;
using Redbox.HAL.Component.Model;
using Redbox.HAL.Component.Model.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;


namespace Redbox.HAL.MSHALTester
{
    public class EditSlotData : Form
    {
        private int CurrentEY;
        private int CurrentEX;
        private int OriginalEY;
        private int OriginalEX;
        private bool PositionsRecorded;
        private bool MoveFailed;
        private readonly DecksConfigurationManager Manager;
        private bool RinglightActive;
        private bool SensorbarActive;
        private int StartXPosition;
        private int m_changes;
        private int? m_preferredDeck;
        private int? m_qlmDeckNumber;
        private readonly XmlNode DeckNode;
        private readonly HardwareService Service;
        private readonly OutputBox OutputBox;
        private IContainer components;
        private Button m_moveLeftButton;
        private TextBox m_encoderUnitsTextBox;
        private Button m_moveRightButton;
        private Button m_okButton;
        private Button m_cancelButton;
        private Button button1;
        private Button button2;
        private ErrorProvider m_errorProvider;
        private TextBox m_deckTextBox;
        private Label label1;
        private Button m_moveButton;
        private Panel panel1;
        private Label label2;
        private Button m_gripperFingerClose;
        private Button m_gripperFingerOpen;
        private Button m_gripperRetractButton;
        private Button m_gripperExtendButton;
        private TextBox textBox1;
        private ListBox m_OutputListBox;
        private Label lbl_IncMoves;
        private Button m_saveChangesButton;
        private GroupBox groupBox1;
        private Button m_ringlightOnButton;
        private Button m_sensorBarOffButton;
        private Button m_sensorBarOnButton;
        private Button m_ringlightOffButton;
        private CheckBox m_applyToAllDecksCheckBox;
        private TextBox m_moveStatusTB;
        private Label label3;

        public EditSlotData(HardwareService service, DecksConfigurationManager mgr)
        {
            this.InitializeComponent();
            this.OutputBox = new OutputBox(this.m_OutputListBox);
            this.Service = service;
            this.Manager = mgr;
            XmlNode deckNode = this.Manager.FindDeckNode(8);
            if (deckNode == null || !deckNode.GetAttributeValue<bool>("IsQlm"))
                return;
            this.m_qlmDeckNumber = new int?(8);
        }

        public EditSlotData(
          HardwareService service,
          DecksConfigurationManager mgr,
          int deck,
          int measureSlot)
          : this(service, mgr)
        {
            this.DeckNode = mgr.FindDeckNode(deck);
            this.m_preferredDeck = new int?(deck);
            this.m_deckTextBox.Text = deck.ToString();
            this.m_deckTextBox.ReadOnly = true;
        }

        public EditSlotData(HardwareService service, int deck, DecksConfigurationManager mgr)
          : this(service, mgr)
        {
            this.ToggleApplyCheckBox(deck == 1);
            this.m_deckTextBox.Text = deck.ToString();
        }

        internal int Deck
        {
            get
            {
                if (this.m_preferredDeck.HasValue)
                    return this.m_preferredDeck.Value;
                string text = this.m_deckTextBox.Text;
                int result;
                if (string.IsNullOrEmpty(text) || !int.TryParse(text, out result))
                    return -1;
                this.m_applyToAllDecksCheckBox.Enabled = result == 1;
                this.m_applyToAllDecksCheckBox.Visible = result == 1;
                return result;
            }
        }

        internal bool ApplyToAllDecks
        {
            get => !this.m_preferredDeck.HasValue && this.m_applyToAllDecksCheckBox.Checked;
        }

        private void moveRightButton_Click(object sender, EventArgs e)
        {
            this.moveEncoderUnits(EditSlotData.Direction.Right);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.CleanupHardware();
            this.Close();
        }

        private bool moveEncoderUnits(EditSlotData.Direction direction)
        {
            if (-1 == this.Deck)
            {
                this.m_errorProvider.SetError((Control)this.m_deckTextBox, "Please select a deck.");
                return false;
            }
            if (!this.PositionsRecorded || this.MoveFailed)
                return false;
            string text = this.m_encoderUnitsTextBox.Text;
            int result = 0;
            if (int.TryParse(text, out result))
            {
                if ((direction == EditSlotData.Direction.Up || direction == EditSlotData.Direction.Down) && Math.Abs(result) > 1000)
                {
                    this.m_errorProvider.SetError((Control)this.m_encoderUnitsTextBox, "Units exceeds threshold for axis.");
                    return false;
                }
                if ((direction == EditSlotData.Direction.Left || direction == EditSlotData.Direction.Right) && Math.Abs(result) > 100)
                {
                    this.m_errorProvider.SetError((Control)this.m_encoderUnitsTextBox, "Units exceeds threshold for axis.");
                    return false;
                }
                if (direction == EditSlotData.Direction.Left || direction == EditSlotData.Direction.Down)
                    result = -result;
                bool flag = direction == EditSlotData.Direction.Up || direction == EditSlotData.Direction.Down;
                int units = flag ? this.CurrentEY + result : this.CurrentEX + result;
                ErrorCodes errorCodes = new MoveHelper(this.Service).MoveAbs(flag ? Axis.Y : Axis.X, units);
                this.OutputBox.Write("MOVE {0} {1} UNITS - New Location {2} ...", (object)direction.ToString().ToUpper(), (object)text, (object)units);
                this.OutputBox.Write(errorCodes.ToString());
                if (errorCodes != ErrorCodes.Success)
                    return false;
                if (flag)
                    this.CurrentEY = units;
                else
                    this.CurrentEX = units;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(this.RinglightActive ? "RINGLIGHT ON" : "RINGLIGHT OFF" + Environment.NewLine);
                stringBuilder.Append(this.SensorbarActive ? "SENSOR PICKER-ON" : "SENSOR PICKER-OFF" + Environment.NewLine);
                this.Service.ExecuteImmediateProgram(Encoding.ASCII.GetBytes(stringBuilder.ToString()), out HardwareJob _);
                this.IncrementChange();
                return true;
            }
            this.m_errorProvider.SetError((Control)this.m_encoderUnitsTextBox, "Must specify an integer!");
            return false;
        }

        private void m_moveLeftButton_Click(object sender, EventArgs e)
        {
            this.moveEncoderUnits(EditSlotData.Direction.Left);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.moveEncoderUnits(EditSlotData.Direction.Up);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.moveEncoderUnits(EditSlotData.Direction.Down);
        }

        private void m_moveButton_Click(object sender, EventArgs e)
        {
            this.m_errorProvider.Clear();
            if (this.m_preferredDeck.HasValue)
            {
                this.MoveToStart(this.m_preferredDeck.Value, this.BaseSlot(this.m_preferredDeck.Value));
                this.m_moveButton.Enabled = false;
            }
            else
            {
                int deck = this.Deck;
                if (deck == -1)
                    this.m_errorProvider.SetError((Control)this.m_deckTextBox, "The deck value must be valid and not excluded.");
                else if (this.m_qlmDeckNumber.HasValue && this.m_qlmDeckNumber.Value == this.Deck)
                    this.m_errorProvider.SetError((Control)this.m_deckTextBox, "The deck value must be valid and not excluded.");
                else
                    this.MoveToStart(this.Deck, this.BaseSlot(deck));
            }
        }

        private void MoveToStart(int deck, int slot)
        {
            MoveHelper moveHelper = new MoveHelper(this.Service);
            ErrorCodes errorCodes = moveHelper.MoveTo(deck, slot);
            if (errorCodes == ErrorCodes.Success)
            {
                this.m_moveStatusTB.Text = errorCodes.ToString().ToUpper();
                IControllerPosition position = moveHelper.GetPosition();
                this.PositionsRecorded = false;
                if (position.ReadOk)
                {
                    this.OriginalEY = this.CurrentEY = position.YCoordinate.Value;
                    this.OriginalEX = this.CurrentEX = position.XCoordinate.Value;
                    this.PositionsRecorded = true;
                }
                this.StartXPosition = this.CurrentEX;
                this.OutputBox.Write("Start positions: X = {0} Y = {1}", (object)this.CurrentEX, (object)this.CurrentEY);
                this.ClearChanges();
            }
            else
            {
                IMotionControlLimitResponse controlLimitResponse = moveHelper.ReadLimits();
                StringBuilder stringBuilder = new StringBuilder();
                if (controlLimitResponse.ReadOk)
                {
                    stringBuilder.Append(errorCodes.ToString().ToUpper());
                    foreach (IMotionControlLimit limit in controlLimitResponse.Limits)
                    {
                        if (limit.Blocked)
                        {
                            stringBuilder.Append(" ");
                            stringBuilder.Append((object)limit);
                        }
                    }
                }
                this.m_moveStatusTB.Text = stringBuilder.ToString();
                int num = (int)MessageBox.Show("Move to position failed!");
                this.MoveFailed = true;
            }
        }

        private void RunSimpleCommand(object sender, EventArgs e)
        {
            if (this.Deck == -1)
                this.m_errorProvider.SetError((Control)this.m_deckTextBox, "Please select a deck.");
            else if (this.Service == null)
            {
                this.NotifyCommunicationProblem();
            }
            else
            {
                if (!(sender is Button button))
                    return;
                IControlSystem service = ServiceLocator.Instance.GetService<IControlSystem>();
                IControlResponse controlResponse = (IControlResponse)null;
                if (button == this.m_ringlightOnButton)
                {
                    controlResponse = service.ToggleRingLight(true, new int?());
                    if (controlResponse.Success)
                        this.RinglightActive = true;
                }
                else if (button == this.m_ringlightOffButton)
                {
                    controlResponse = service.ToggleRingLight(false, new int?());
                    if (controlResponse.Success)
                        this.RinglightActive = false;
                }
                else if (button == this.m_sensorBarOnButton)
                {
                    controlResponse = service.SetSensors(true);
                    if (controlResponse.Success)
                        this.SensorbarActive = true;
                }
                else if (button == this.m_sensorBarOffButton)
                {
                    controlResponse = service.SetSensors(false);
                    if (controlResponse.Success)
                        this.SensorbarActive = false;
                }
                else if (button == this.m_gripperExtendButton)
                    controlResponse = service.ExtendArm();
                else if (button == this.m_gripperFingerOpen)
                    controlResponse = service.SetFinger(GripperFingerState.Rent);
                else if (button == this.m_gripperRetractButton)
                    controlResponse = service.RetractArm();
                else if (button == this.m_gripperFingerClose)
                    controlResponse = service.SetFinger(GripperFingerState.Closed);
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(button.Tag?.ToString() + " ... ");
                if (controlResponse.CommError)
                    stringBuilder.Append(controlResponse.Diagnostic);
                else if (controlResponse.TimedOut)
                    stringBuilder.Append(ErrorCodes.Timeout.ToString().ToUpper());
                else
                    stringBuilder.Append(ErrorCodes.Success.ToString().ToUpper());
                this.OutputBox.Write(stringBuilder.ToString());
            }
        }

        private void NotifyCommunicationProblem()
        {
            this.OutputBox.Write("Unable to communicate with HAL. Might be on a lunch break.");
            this.m_errorProvider.SetError((Control)this.m_deckTextBox, "Unable to communicate with HAL.");
        }

        private void ToggleApplyCheckBox(bool enabled)
        {
            this.m_applyToAllDecksCheckBox.Visible = enabled;
            this.m_applyToAllDecksCheckBox.Enabled = enabled;
        }

        private void saveChangesButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            if (this.m_changes > 0 && this.ApplyChanges())
                this.Manager.FlushChanges(false);
            this.CleanupHardware();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.ApplyChanges())
                this.Manager.FlushChanges(false);
            this.ClearChanges();
        }

        private bool ApplyChanges()
        {
            int deltaY = this.CurrentEY - this.OriginalEY;
            int deltaX = this.CurrentEX - this.OriginalEX;
            this.OriginalEX = this.CurrentEX;
            this.OriginalEY = this.CurrentEY;
            if (this.m_preferredDeck.HasValue)
                return this.UpdateDeckNode(this.DeckNode, deltaX, deltaY, false);
            if (this.Deck != 1 || !this.m_applyToAllDecksCheckBox.Checked)
                return this.UpdateDeckNode(this.Manager.FindDeckNode(this.Deck), deltaX, deltaY, false);
            int num = 0;
            foreach (XmlNode allDeckNode in this.Manager.FindAllDeckNodes())
            {
                if (this.UpdateDeckNode(allDeckNode, deltaX, deltaY, true))
                    ++num;
            }
            return num > 0;
        }

        private bool UpdateDeckNode(XmlNode node, int deltaX, int deltaY, bool applyBlind)
        {
            if (deltaY == 0 && deltaX == 0 || node.GetAttributeValue<bool>("IsQlm"))
                return false;
            int attributeValue1 = node.GetAttributeValue<int>("Number");
            if (deltaY != 0)
            {
                int num = node.GetAttributeValue<int>("Offset") + deltaY;
                node.SetAttributeValue<int>("Offset", num);
            }
            if (deltaX != 0)
            {
                XmlNode childNode = node.ChildNodes[0];
                Decimal? attributeValue2 = node.GetAttributeValue<Decimal?>("SlotWidth");
                Decimal num = attributeValue1 != 8 || applyBlind ? (Decimal)childNode.GetAttributeValue<int>("Offset") + (Decimal)deltaX : (Decimal)this.StartXPosition + (Decimal)deltaX - attributeValue2.Value;
                int attributeValue3 = node.GetAttributeValue<int>("Offset");
                int? attributeValue4 = node.GetAttributeValue<int?>("NumberOfSlots");
                int? attributeValue5 = node.GetAttributeValue<int?>("SellThruSlots");
                int? attributeValue6 = node.GetAttributeValue<int?>("SellThruOffset");
                int? attributeValue7 = node.GetAttributeValue<int?>("ApproachOffset");
                bool attributeValue8 = node.GetAttributeValue<bool>("IsQlm");
                int? slotsPerQuadrant = new int?(attributeValue5.HasValue ? 6 : 15);
                int count = node.ChildNodes.Count;
                node.Attributes.RemoveAll();
                CommonFunctions.ComputeQuadrants(new Decimal?(num), new int?(count), attributeValue5, attributeValue6, slotsPerQuadrant, attributeValue2, node);
                node.SetAttributeValue<int>("Number", attributeValue1);
                node.SetAttributeValue<int>("Offset", attributeValue3);
                node.SetAttributeValue<int?>("NumberOfSlots", attributeValue4);
                node.SetAttributeValue<Decimal?>("SlotWidth", attributeValue2);
                node.SetAttributeValue<bool>("IsQlm", attributeValue8);
                node.SetAttributeValue<int?>("SellThruSlots", attributeValue5);
                node.SetAttributeValue<int?>("SellThruOffset", attributeValue6);
                node.SetAttributeValue<int?>("ApproachOffset", attributeValue7);
                node.SetAttributeValue<int?>("SlotsPerQuadrant", slotsPerQuadrant);
            }
            return true;
        }

        private void ClearChanges()
        {
            this.m_changes = 0;
            this.m_saveChangesButton.Enabled = false;
        }

        private void IncrementChange()
        {
            ++this.m_changes;
            this.m_saveChangesButton.Enabled = this.m_changes > 0;
        }

        private void CleanupHardware()
        {
            if (this.Service == null)
                return;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("RINGLIGHT OFF" + Environment.NewLine);
            stringBuilder.Append("SENSOR PICKER-OFF" + Environment.NewLine);
            this.Service.ExecuteImmediateProgram(Encoding.ASCII.GetBytes(stringBuilder.ToString()), out HardwareJob _);
        }

        private int BaseSlot(int targetDeck) => targetDeck != 8 ? 1 : 2;

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = (IContainer)new System.ComponentModel.Container();
            this.m_moveLeftButton = new Button();
            this.m_encoderUnitsTextBox = new TextBox();
            this.m_moveRightButton = new Button();
            this.m_okButton = new Button();
            this.m_cancelButton = new Button();
            this.button1 = new Button();
            this.button2 = new Button();
            this.m_errorProvider = new ErrorProvider(this.components);
            this.label1 = new Label();
            this.m_deckTextBox = new TextBox();
            this.m_moveButton = new Button();
            this.panel1 = new Panel();
            this.m_gripperFingerClose = new Button();
            this.m_gripperFingerOpen = new Button();
            this.m_gripperRetractButton = new Button();
            this.m_gripperExtendButton = new Button();
            this.label2 = new Label();
            this.textBox1 = new TextBox();
            this.m_OutputListBox = new ListBox();
            this.lbl_IncMoves = new Label();
            this.m_saveChangesButton = new Button();
            this.groupBox1 = new GroupBox();
            this.m_sensorBarOffButton = new Button();
            this.m_sensorBarOnButton = new Button();
            this.m_ringlightOffButton = new Button();
            this.m_ringlightOnButton = new Button();
            this.m_applyToAllDecksCheckBox = new CheckBox();
            this.label3 = new Label();
            this.m_moveStatusTB = new TextBox();
            ((ISupportInitialize)this.m_errorProvider).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            this.m_moveLeftButton.BackColor = Color.LightGray;
            this.m_moveLeftButton.Location = new Point(529, 174);
            this.m_moveLeftButton.Name = "m_moveLeftButton";
            this.m_moveLeftButton.Size = new Size(92, 40);
            this.m_moveLeftButton.TabIndex = 0;
            this.m_moveLeftButton.Text = "< Move &Left";
            this.m_moveLeftButton.UseVisualStyleBackColor = false;
            this.m_moveLeftButton.Click += new EventHandler(this.m_moveLeftButton_Click);
            this.m_encoderUnitsTextBox.Location = new Point(627, 185);
            this.m_encoderUnitsTextBox.Name = "m_encoderUnitsTextBox";
            this.m_encoderUnitsTextBox.Size = new Size(74, 20);
            this.m_encoderUnitsTextBox.TabIndex = 1;
            this.m_encoderUnitsTextBox.Text = "10";
            this.m_moveRightButton.BackColor = Color.LightGray;
            this.m_moveRightButton.Location = new Point(707, 167);
            this.m_moveRightButton.Name = "m_moveRightButton";
            this.m_moveRightButton.Size = new Size(92, 40);
            this.m_moveRightButton.TabIndex = 2;
            this.m_moveRightButton.Text = "Move &Right >";
            this.m_moveRightButton.UseVisualStyleBackColor = false;
            this.m_moveRightButton.Click += new EventHandler(this.moveRightButton_Click);
            this.m_okButton.BackColor = Color.LightGray;
            this.m_okButton.Location = new Point(327, 425);
            this.m_okButton.Name = "m_okButton";
            this.m_okButton.Size = new Size(90, 40);
            this.m_okButton.TabIndex = 3;
            this.m_okButton.Text = "&Ok";
            this.m_okButton.UseVisualStyleBackColor = false;
            this.m_okButton.Click += new EventHandler(this.saveChangesButton_Click);
            this.m_cancelButton.BackColor = Color.LightGray;
            this.m_cancelButton.Location = new Point(423, 425);
            this.m_cancelButton.Name = "m_cancelButton";
            this.m_cancelButton.Size = new Size(90, 40);
            this.m_cancelButton.TabIndex = 4;
            this.m_cancelButton.Text = "&Cancel";
            this.m_cancelButton.UseVisualStyleBackColor = false;
            this.m_cancelButton.Click += new EventHandler(this.cancelButton_Click);
            this.button1.BackColor = Color.LightGray;
            this.button1.Location = new Point(626, 124);
            this.button1.Name = "button1";
            this.button1.Size = new Size(75, 55);
            this.button1.TabIndex = 5;
            this.button1.Text = "&Up";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button2.BackColor = Color.LightGray;
            this.button2.Location = new Point(627, 211);
            this.button2.Name = "button2";
            this.button2.Size = new Size(75, 53);
            this.button2.TabIndex = 6;
            this.button2.Text = "&Down";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.m_errorProvider.ContainerControl = (ContainerControl)this;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(14, 55);
            this.label1.Name = "label1";
            this.label1.Size = new Size(73, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Current Deck:";
            this.m_deckTextBox.Location = new Point(23, 71);
            this.m_deckTextBox.Name = "m_deckTextBox";
            this.m_deckTextBox.Size = new Size(64, 20);
            this.m_deckTextBox.TabIndex = 8;
            this.m_moveButton.BackColor = Color.LightGray;
            this.m_moveButton.Location = new Point(108, 50);
            this.m_moveButton.Name = "m_moveButton";
            this.m_moveButton.Size = new Size(95, 47);
            this.m_moveButton.TabIndex = 13;
            this.m_moveButton.Text = "Move";
            this.m_moveButton.UseVisualStyleBackColor = false;
            this.m_moveButton.Click += new EventHandler(this.m_moveButton_Click);
            this.panel1.Controls.Add((Control)this.m_gripperFingerClose);
            this.panel1.Controls.Add((Control)this.m_gripperFingerOpen);
            this.panel1.Controls.Add((Control)this.m_gripperRetractButton);
            this.panel1.Controls.Add((Control)this.m_gripperExtendButton);
            this.panel1.Location = new Point(15, 164);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(213, 115);
            this.panel1.TabIndex = 14;
            this.m_gripperFingerClose.BackColor = Color.LightGray;
            this.m_gripperFingerClose.Location = new Point(117, 72);
            this.m_gripperFingerClose.Name = "m_gripperFingerClose";
            this.m_gripperFingerClose.Size = new Size(92, 40);
            this.m_gripperFingerClose.TabIndex = 3;
            this.m_gripperFingerClose.Tag = (object)"GRIPPER CLOSE";
            this.m_gripperFingerClose.Text = "Close";
            this.m_gripperFingerClose.UseVisualStyleBackColor = false;
            this.m_gripperFingerClose.Click += new EventHandler(this.RunSimpleCommand);
            this.m_gripperFingerOpen.BackColor = Color.LightGray;
            this.m_gripperFingerOpen.Location = new Point(117, 3);
            this.m_gripperFingerOpen.Name = "m_gripperFingerOpen";
            this.m_gripperFingerOpen.Size = new Size(92, 40);
            this.m_gripperFingerOpen.TabIndex = 2;
            this.m_gripperFingerOpen.Tag = (object)"GRIPPER RENT";
            this.m_gripperFingerOpen.Text = "Rent";
            this.m_gripperFingerOpen.UseVisualStyleBackColor = false;
            this.m_gripperFingerOpen.Click += new EventHandler(this.RunSimpleCommand);
            this.m_gripperRetractButton.BackColor = Color.LightGray;
            this.m_gripperRetractButton.Location = new Point(3, 72);
            this.m_gripperRetractButton.Name = "m_gripperRetractButton";
            this.m_gripperRetractButton.Size = new Size(92, 40);
            this.m_gripperRetractButton.TabIndex = 1;
            this.m_gripperRetractButton.Tag = (object)"GRIPPER RETRACT";
            this.m_gripperRetractButton.Text = "Retract";
            this.m_gripperRetractButton.UseVisualStyleBackColor = false;
            this.m_gripperRetractButton.Click += new EventHandler(this.RunSimpleCommand);
            this.m_gripperExtendButton.BackColor = Color.LightGray;
            this.m_gripperExtendButton.Location = new Point(3, 3);
            this.m_gripperExtendButton.Name = "m_gripperExtendButton";
            this.m_gripperExtendButton.Size = new Size(92, 40);
            this.m_gripperExtendButton.TabIndex = 0;
            this.m_gripperExtendButton.Tag = (object)"GRIPPER EXTEND";
            this.m_gripperExtendButton.Text = "Extend";
            this.m_gripperExtendButton.UseVisualStyleBackColor = false;
            this.m_gripperExtendButton.Click += new EventHandler(this.RunSimpleCommand);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(15, 148);
            this.label2.Name = "label2";
            this.label2.Size = new Size(114, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Gripper Test Functions";
            this.textBox1.BackColor = Color.Yellow;
            this.textBox1.Location = new Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new Size(230, 20);
            this.textBox1.TabIndex = 16;
            this.textBox1.Text = "** This tool does not work on the QLM deck. **";
            this.m_OutputListBox.FormattingEnabled = true;
            this.m_OutputListBox.Location = new Point(15, 285);
            this.m_OutputListBox.Name = "m_OutputListBox";
            this.m_OutputListBox.SelectionMode = SelectionMode.MultiExtended;
            this.m_OutputListBox.Size = new Size(570, 134);
            this.m_OutputListBox.TabIndex = 17;
            this.lbl_IncMoves.AutoSize = true;
            this.lbl_IncMoves.Location = new Point(510, 124);
            this.lbl_IncMoves.Name = "lbl_IncMoves";
            this.lbl_IncMoves.Size = new Size(97, 13);
            this.lbl_IncMoves.TabIndex = 18;
            this.lbl_IncMoves.Text = "Incremental Moves";
            this.m_saveChangesButton.BackColor = Color.LightGray;
            this.m_saveChangesButton.Enabled = false;
            this.m_saveChangesButton.Location = new Point(231, 425);
            this.m_saveChangesButton.Name = "m_saveChangesButton";
            this.m_saveChangesButton.Size = new Size(90, 40);
            this.m_saveChangesButton.TabIndex = 19;
            this.m_saveChangesButton.Text = "Save Current Changes";
            this.m_saveChangesButton.UseVisualStyleBackColor = false;
            this.m_saveChangesButton.Click += new EventHandler(this.button3_Click);
            this.groupBox1.Controls.Add((Control)this.m_sensorBarOffButton);
            this.groupBox1.Controls.Add((Control)this.m_sensorBarOnButton);
            this.groupBox1.Controls.Add((Control)this.m_ringlightOffButton);
            this.groupBox1.Controls.Add((Control)this.m_ringlightOnButton);
            this.groupBox1.Location = new Point(243, 148);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(216, 131);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Lighting";
            this.m_sensorBarOffButton.BackColor = Color.LightGray;
            this.m_sensorBarOffButton.Location = new Point(118, 85);
            this.m_sensorBarOffButton.Name = "m_sensorBarOffButton";
            this.m_sensorBarOffButton.Size = new Size(92, 40);
            this.m_sensorBarOffButton.TabIndex = 3;
            this.m_sensorBarOffButton.Tag = (object)"SENSOR PICKER-OFF";
            this.m_sensorBarOffButton.Text = "Sensor Bar Off";
            this.m_sensorBarOffButton.UseVisualStyleBackColor = false;
            this.m_sensorBarOffButton.Click += new EventHandler(this.RunSimpleCommand);
            this.m_sensorBarOnButton.BackColor = Color.LightGray;
            this.m_sensorBarOnButton.Location = new Point(118, 17);
            this.m_sensorBarOnButton.Name = "m_sensorBarOnButton";
            this.m_sensorBarOnButton.Size = new Size(92, 40);
            this.m_sensorBarOnButton.TabIndex = 2;
            this.m_sensorBarOnButton.Tag = (object)"SENSOR PICKER-ON";
            this.m_sensorBarOnButton.Text = "Sensor Bar On";
            this.m_sensorBarOnButton.UseVisualStyleBackColor = false;
            this.m_sensorBarOnButton.Click += new EventHandler(this.RunSimpleCommand);
            this.m_ringlightOffButton.BackColor = Color.LightGray;
            this.m_ringlightOffButton.Location = new Point(6, 85);
            this.m_ringlightOffButton.Name = "m_ringlightOffButton";
            this.m_ringlightOffButton.Size = new Size(92, 40);
            this.m_ringlightOffButton.TabIndex = 1;
            this.m_ringlightOffButton.Tag = (object)"RINGLIGHT OFF";
            this.m_ringlightOffButton.Text = "Ringlight Off";
            this.m_ringlightOffButton.UseVisualStyleBackColor = false;
            this.m_ringlightOffButton.Click += new EventHandler(this.RunSimpleCommand);
            this.m_ringlightOnButton.BackColor = Color.LightGray;
            this.m_ringlightOnButton.Location = new Point(6, 17);
            this.m_ringlightOnButton.Name = "m_ringlightOnButton";
            this.m_ringlightOnButton.Size = new Size(92, 40);
            this.m_ringlightOnButton.TabIndex = 0;
            this.m_ringlightOnButton.Tag = (object)"RINGLIGHT ON";
            this.m_ringlightOnButton.Text = "Ringlight On";
            this.m_ringlightOnButton.UseVisualStyleBackColor = false;
            this.m_ringlightOnButton.Click += new EventHandler(this.RunSimpleCommand);
            this.m_applyToAllDecksCheckBox.AutoSize = true;
            this.m_applyToAllDecksCheckBox.Enabled = false;
            this.m_applyToAllDecksCheckBox.Location = new Point(23, 107);
            this.m_applyToAllDecksCheckBox.Name = "m_applyToAllDecksCheckBox";
            this.m_applyToAllDecksCheckBox.Size = new Size(115, 17);
            this.m_applyToAllDecksCheckBox.TabIndex = 21;
            this.m_applyToAllDecksCheckBox.Text = "Apply to all decks?";
            this.m_applyToAllDecksCheckBox.UseVisualStyleBackColor = true;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(228, 50);
            this.label3.Name = "label3";
            this.label3.Size = new Size(67, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Move Status";
            this.m_moveStatusTB.Location = new Point(231, 71);
            this.m_moveStatusTB.Name = "m_moveStatusTB";
            this.m_moveStatusTB.Size = new Size(186, 20);
            this.m_moveStatusTB.TabIndex = 23;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(832, 488);
            this.ControlBox = false;
            this.Controls.Add((Control)this.m_moveStatusTB);
            this.Controls.Add((Control)this.label3);
            this.Controls.Add((Control)this.m_applyToAllDecksCheckBox);
            this.Controls.Add((Control)this.groupBox1);
            this.Controls.Add((Control)this.m_saveChangesButton);
            this.Controls.Add((Control)this.lbl_IncMoves);
            this.Controls.Add((Control)this.m_OutputListBox);
            this.Controls.Add((Control)this.textBox1);
            this.Controls.Add((Control)this.label2);
            this.Controls.Add((Control)this.panel1);
            this.Controls.Add((Control)this.m_moveButton);
            this.Controls.Add((Control)this.m_deckTextBox);
            this.Controls.Add((Control)this.label1);
            this.Controls.Add((Control)this.button2);
            this.Controls.Add((Control)this.button1);
            this.Controls.Add((Control)this.m_cancelButton);
            this.Controls.Add((Control)this.m_okButton);
            this.Controls.Add((Control)this.m_moveRightButton);
            this.Controls.Add((Control)this.m_encoderUnitsTextBox);
            this.Controls.Add((Control)this.m_moveLeftButton);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Name = nameof(EditSlotData);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Edit Slot Data";
            ((ISupportInitialize)this.m_errorProvider).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public enum Direction
        {
            Left,
            Right,
            Up,
            Down,
        }
    }
}
