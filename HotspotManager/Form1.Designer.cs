using System.Drawing;
using System.Windows.Forms;

namespace HotspotManager
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblSsid = new System.Windows.Forms.Label();
            this.txtSsid = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblBand = new System.Windows.Forms.Label();
            this.cmbBand = new System.Windows.Forms.ComboBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpMainControls = new System.Windows.Forms.GroupBox();
            this.chkStartup = new System.Windows.Forms.CheckBox();
            this.pnlSetup = new System.Windows.Forms.Panel();
            this.rtbSetupLog = new System.Windows.Forms.RichTextBox();
            this.btnPerformSetup = new System.Windows.Forms.Button();
            this.lblSetupDescription = new System.Windows.Forms.Label();
            this.lblSetupTitle = new System.Windows.Forms.Label();
            this.statusStrip.SuspendLayout();
            this.grpMainControls.SuspendLayout();
            this.pnlSetup.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSsid
            // 
            this.lblSsid.AutoSize = true;
            this.lblSsid.Location = new System.Drawing.Point(11, 24);
            this.lblSsid.Name = "lblSsid";
            this.lblSsid.Size = new System.Drawing.Size(112, 13);
            this.lblSsid.TabIndex = 0;
            this.lblSsid.Text = "Hotspot Name (SSID):";
            // 
            // txtSsid
            // 
            this.txtSsid.Location = new System.Drawing.Point(128, 22);
            this.txtSsid.Name = "txtSsid";
            this.txtSsid.Size = new System.Drawing.Size(147, 20);
            this.txtSsid.TabIndex = 1;
            this.txtSsid.Text = "Devtop1-P50";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(11, 53);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(128, 50);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(147, 20);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.Text = "password123";
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblBand
            // 
            this.lblBand.AutoSize = true;
            this.lblBand.Location = new System.Drawing.Point(11, 81);
            this.lblBand.Name = "lblBand";
            this.lblBand.Size = new System.Drawing.Size(78, 13);
            this.lblBand.TabIndex = 4;
            this.lblBand.Text = "Network Band:";
            // 
            // cmbBand
            // 
            this.cmbBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBand.FormattingEnabled = true;
            this.cmbBand.Items.AddRange(new object[] {
            "5 GHz",
            "2.4 GHz",
            "Auto"});
            this.cmbBand.Location = new System.Drawing.Point(128, 79);
            this.cmbBand.Name = "cmbBand";
            this.cmbBand.Size = new System.Drawing.Size(147, 21);
            this.cmbBand.TabIndex = 3;
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.PaleGreen;
            this.btnStart.Enabled = false;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(14, 142);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(128, 36);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Start Hotspot";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.LightCoral;
            this.btnStop.Enabled = false;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(147, 142);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(128, 36);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "Stop Hotspot";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 218);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip.Size = new System.Drawing.Size(309, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 8;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(174, 17);
            this.lblStatus.Text = "Status: Checking Prerequisites...";
            // 
            // grpMainControls
            // 
            this.grpMainControls.Controls.Add(this.chkStartup);
            this.grpMainControls.Controls.Add(this.lblSsid);
            this.grpMainControls.Controls.Add(this.txtSsid);
            this.grpMainControls.Controls.Add(this.btnStop);
            this.grpMainControls.Controls.Add(this.lblPassword);
            this.grpMainControls.Controls.Add(this.btnStart);
            this.grpMainControls.Controls.Add(this.txtPassword);
            this.grpMainControls.Controls.Add(this.cmbBand);
            this.grpMainControls.Controls.Add(this.lblBand);
            this.grpMainControls.Enabled = false;
            this.grpMainControls.Location = new System.Drawing.Point(9, 10);
            this.grpMainControls.Name = "grpMainControls";
            this.grpMainControls.Size = new System.Drawing.Size(291, 195);
            this.grpMainControls.TabIndex = 9;
            this.grpMainControls.TabStop = false;
            this.grpMainControls.Text = "Hotspot Controls";
            // 
            // chkStartup
            // 
            this.chkStartup.AutoSize = true;
            this.chkStartup.Location = new System.Drawing.Point(14, 111);
            this.chkStartup.Name = "chkStartup";
            this.chkStartup.Size = new System.Drawing.Size(196, 17);
            this.chkStartup.TabIndex = 4;
            this.chkStartup.Text = "Start Hotspot automatically on Login";
            this.chkStartup.UseVisualStyleBackColor = true;
            this.chkStartup.CheckedChanged += new System.EventHandler(this.chkStartup_CheckedChanged);
            // 
            // pnlSetup
            // 
            this.pnlSetup.Controls.Add(this.rtbSetupLog);
            this.pnlSetup.Controls.Add(this.btnPerformSetup);
            this.pnlSetup.Controls.Add(this.lblSetupDescription);
            this.pnlSetup.Controls.Add(this.lblSetupTitle);
            this.pnlSetup.ForeColor = System.Drawing.Color.Silver;
            this.pnlSetup.Location = new System.Drawing.Point(9, 10);
            this.pnlSetup.Name = "pnlSetup";
            this.pnlSetup.Size = new System.Drawing.Size(291, 195);
            this.pnlSetup.TabIndex = 10;
            this.pnlSetup.Visible = false;
            // 
            // rtbSetupLog
            // 
            this.rtbSetupLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.rtbSetupLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbSetupLog.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbSetupLog.ForeColor = System.Drawing.Color.Silver;
            this.rtbSetupLog.Location = new System.Drawing.Point(14, 53);
            this.rtbSetupLog.Name = "rtbSetupLog";
            this.rtbSetupLog.ReadOnly = true;
            this.rtbSetupLog.Size = new System.Drawing.Size(263, 84);
            this.rtbSetupLog.TabIndex = 3;
            this.rtbSetupLog.Text = "";
            // 
            // btnPerformSetup
            // 
            this.btnPerformSetup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPerformSetup.Location = new System.Drawing.Point(62, 142);
            this.btnPerformSetup.Name = "btnPerformSetup";
            this.btnPerformSetup.Size = new System.Drawing.Size(169, 36);
            this.btnPerformSetup.TabIndex = 2;
            this.btnPerformSetup.Text = "Perform One-Time Setup";
            this.btnPerformSetup.UseVisualStyleBackColor = true;
            this.btnPerformSetup.Click += new System.EventHandler(this.btnPerformSetup_Click);
            // 
            // lblSetupDescription
            // 
            this.lblSetupDescription.AutoSize = true;
            this.lblSetupDescription.Location = new System.Drawing.Point(11, 28);
            this.lblSetupDescription.Name = "lblSetupDescription";
            this.lblSetupDescription.Size = new System.Drawing.Size(196, 13);
            this.lblSetupDescription.TabIndex = 1;
            this.lblSetupDescription.Text = "This system needs to be configured first.";
            // 
            // lblSetupTitle
            // 
            this.lblSetupTitle.AutoSize = true;
            this.lblSetupTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSetupTitle.Location = new System.Drawing.Point(10, 8);
            this.lblSetupTitle.Name = "lblSetupTitle";
            this.lblSetupTitle.Size = new System.Drawing.Size(161, 17);
            this.lblSetupTitle.TabIndex = 0;
            this.lblSetupTitle.Text = "System Prerequisites";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 240);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.pnlSetup);
            this.Controls.Add(this.grpMainControls);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Local Hotspot Manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.grpMainControls.ResumeLayout(false);
            this.grpMainControls.PerformLayout();
            this.pnlSetup.ResumeLayout(false);
            this.pnlSetup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSsid;
        private System.Windows.Forms.TextBox txtSsid;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblBand;
        private System.Windows.Forms.ComboBox cmbBand;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.GroupBox grpMainControls;
        private System.Windows.Forms.Panel pnlSetup;
        private System.Windows.Forms.RichTextBox rtbSetupLog;
        private System.Windows.Forms.Button btnPerformSetup;
        private System.Windows.Forms.Label lblSetupDescription;
        private System.Windows.Forms.Label lblSetupTitle;
        private System.Windows.Forms.CheckBox chkStartup;
    }
}