namespace GCITester
{
    partial class SerialPortSettings
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labl7 = new System.Windows.Forms.Label();
            this.comboCOMPort = new System.Windows.Forms.ComboBox();
            this.baudRateLab = new System.Windows.Forms.Label();
            this.dataBitsLab = new System.Windows.Forms.Label();
            this.parityLab = new System.Windows.Forms.Label();
            this.stopBitsLab = new System.Windows.Forms.Label();
            this.comboBaudRate = new System.Windows.Forms.ComboBox();
            this.comboDataBits = new System.Windows.Forms.ComboBox();
            this.comboParity = new System.Windows.Forms.ComboBox();
            this.comboStopBits = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // labl7
            // 
            this.labl7.AutoSize = true;
            this.labl7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(49)))), ((int)(((byte)(95)))));
            this.labl7.Location = new System.Drawing.Point(40, 39);
            this.labl7.Name = "labl7";
            this.labl7.Size = new System.Drawing.Size(50, 13);
            this.labl7.TabIndex = 0;
            this.labl7.Text = "Com Port";
            this.labl7.Click += new System.EventHandler(this.label7_Click);
            // 
            // comboCOMPort
            // 
            this.comboCOMPort.FormattingEnabled = true;
            this.comboCOMPort.Location = new System.Drawing.Point(115, 39);
            this.comboCOMPort.Name = "comboCOMPort";
            this.comboCOMPort.Size = new System.Drawing.Size(121, 21);
            this.comboCOMPort.TabIndex = 1;
            this.comboCOMPort.SelectedIndexChanged += new System.EventHandler(this.comboCOMPort_SelectedIndexChanged);
            // 
            // baudRateLab
            // 
            this.baudRateLab.AutoSize = true;
            this.baudRateLab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(49)))), ((int)(((byte)(95)))));
            this.baudRateLab.Location = new System.Drawing.Point(40, 73);
            this.baudRateLab.Name = "baudRateLab";
            this.baudRateLab.Size = new System.Drawing.Size(58, 13);
            this.baudRateLab.TabIndex = 2;
            this.baudRateLab.Text = "Baud Rate";
            // 
            // dataBitsLab
            // 
            this.dataBitsLab.AutoSize = true;
            this.dataBitsLab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(49)))), ((int)(((byte)(95)))));
            this.dataBitsLab.Location = new System.Drawing.Point(40, 108);
            this.dataBitsLab.Name = "dataBitsLab";
            this.dataBitsLab.Size = new System.Drawing.Size(50, 13);
            this.dataBitsLab.TabIndex = 3;
            this.dataBitsLab.Text = "Data Bits";
            // 
            // parityLab
            // 
            this.parityLab.AutoSize = true;
            this.parityLab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(49)))), ((int)(((byte)(95)))));
            this.parityLab.Location = new System.Drawing.Point(40, 139);
            this.parityLab.Name = "parityLab";
            this.parityLab.Size = new System.Drawing.Size(33, 13);
            this.parityLab.TabIndex = 4;
            this.parityLab.Text = "Parity";
            // 
            // stopBitsLab
            // 
            this.stopBitsLab.AutoSize = true;
            this.stopBitsLab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(49)))), ((int)(((byte)(95)))));
            this.stopBitsLab.Location = new System.Drawing.Point(40, 171);
            this.stopBitsLab.Name = "stopBitsLab";
            this.stopBitsLab.Size = new System.Drawing.Size(49, 13);
            this.stopBitsLab.TabIndex = 5;
            this.stopBitsLab.Text = "Stop Bits";
            // 
            // comboBaudRate
            // 
            this.comboBaudRate.FormattingEnabled = true;
            this.comboBaudRate.Location = new System.Drawing.Point(115, 73);
            this.comboBaudRate.Name = "comboBaudRate";
            this.comboBaudRate.Size = new System.Drawing.Size(121, 21);
            this.comboBaudRate.TabIndex = 6;
            this.comboBaudRate.SelectedIndexChanged += new System.EventHandler(this.comboBaudRate_SelectedIndexChanged);
            // 
            // comboDataBits
            // 
            this.comboDataBits.FormattingEnabled = true;
            this.comboDataBits.Location = new System.Drawing.Point(115, 108);
            this.comboDataBits.Name = "comboDataBits";
            this.comboDataBits.Size = new System.Drawing.Size(121, 21);
            this.comboDataBits.TabIndex = 7;
            this.comboDataBits.SelectedIndexChanged += new System.EventHandler(this.comboDataBits_SelectedIndexChanged);
            // 
            // comboParity
            // 
            this.comboParity.FormattingEnabled = true;
            this.comboParity.Location = new System.Drawing.Point(115, 139);
            this.comboParity.Name = "comboParity";
            this.comboParity.Size = new System.Drawing.Size(121, 21);
            this.comboParity.TabIndex = 8;
            this.comboParity.SelectedIndexChanged += new System.EventHandler(this.comboParity_SelectedIndexChanged);
            // 
            // comboStopBits
            // 
            this.comboStopBits.FormattingEnabled = true;
            this.comboStopBits.Location = new System.Drawing.Point(115, 171);
            this.comboStopBits.Name = "comboStopBits";
            this.comboStopBits.Size = new System.Drawing.Size(121, 21);
            this.comboStopBits.TabIndex = 9;
            this.comboStopBits.SelectedIndexChanged += new System.EventHandler(this.comboStopBits_SelectedIndexChanged);
            // 
            // SerialPortSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboStopBits);
            this.Controls.Add(this.comboParity);
            this.Controls.Add(this.comboDataBits);
            this.Controls.Add(this.comboBaudRate);
            this.Controls.Add(this.stopBitsLab);
            this.Controls.Add(this.parityLab);
            this.Controls.Add(this.dataBitsLab);
            this.Controls.Add(this.baudRateLab);
            this.Controls.Add(this.comboCOMPort);
            this.Controls.Add(this.labl7);
            this.Name = "SerialPortSettings";
            this.Size = new System.Drawing.Size(349, 328);
            this.Load += new System.EventHandler(this.SerialPortSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboCOMPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBaudRate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboDataBits;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboParity;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboStopBits;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboHandshaking;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labl7;
        private System.Windows.Forms.Label baudRateLab;
        private System.Windows.Forms.Label dataBitsLab;
        private System.Windows.Forms.Label parityLab;
        private System.Windows.Forms.Label stopBitsLab;
    }
}
