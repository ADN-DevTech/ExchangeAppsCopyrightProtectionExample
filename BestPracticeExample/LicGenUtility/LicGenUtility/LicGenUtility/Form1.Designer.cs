namespace LicGenUtility
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1_lockCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1_Decrypt = new System.Windows.Forms.Button();
            this.textBox1_shortMacCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4_appname = new System.Windows.Forms.Label();
            this.label4_licType = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1_LicCode = new System.Windows.Forms.TextBox();
            this.Lic_Generate_button = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.dateTimePicker1_expire = new System.Windows.Forms.DateTimePicker();
            this.comboBox1_Lic = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1_ok = new System.Windows.Forms.Button();
            this.clear_button = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1_lockCode
            // 
            this.textBox1_lockCode.Location = new System.Drawing.Point(24, 25);
            this.textBox1_lockCode.Multiline = true;
            this.textBox1_lockCode.Name = "textBox1_lockCode";
            this.textBox1_lockCode.Size = new System.Drawing.Size(797, 73);
            this.textBox1_lockCode.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Lock code";
            // 
            // button1_Decrypt
            // 
            this.button1_Decrypt.Location = new System.Drawing.Point(741, 104);
            this.button1_Decrypt.Name = "button1_Decrypt";
            this.button1_Decrypt.Size = new System.Drawing.Size(80, 32);
            this.button1_Decrypt.TabIndex = 2;
            this.button1_Decrypt.Text = "Decrypt";
            this.button1_Decrypt.UseVisualStyleBackColor = true;
            this.button1_Decrypt.Click += new System.EventHandler(this.button1_Decrypt_Click);
            // 
            // textBox1_shortMacCode
            // 
            this.textBox1_shortMacCode.Location = new System.Drawing.Point(24, 163);
            this.textBox1_shortMacCode.Multiline = true;
            this.textBox1_shortMacCode.Name = "textBox1_shortMacCode";
            this.textBox1_shortMacCode.Size = new System.Drawing.Size(797, 47);
            this.textBox1_shortMacCode.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(19, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Machine code";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(28, 233);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "App name";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4_appname
            // 
            this.label4_appname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4_appname.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4_appname.Location = new System.Drawing.Point(123, 228);
            this.label4_appname.Name = "label4_appname";
            this.label4_appname.Size = new System.Drawing.Size(484, 31);
            this.label4_appname.TabIndex = 6;
            this.label4_appname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4_appname.Click += new System.EventHandler(this.label4_appname_Click);
            // 
            // label4_licType
            // 
            this.label4_licType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4_licType.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4_licType.Location = new System.Drawing.Point(698, 228);
            this.label4_licType.Name = "label4_licType";
            this.label4_licType.Size = new System.Drawing.Size(123, 31);
            this.label4_licType.TabIndex = 8;
            this.label4_licType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4_licType.Click += new System.EventHandler(this.label4_licType_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(611, 233);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 20);
            this.label5.TabIndex = 7;
            this.label5.Text = "Lic Type";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1_LicCode);
            this.groupBox1.Controls.Add(this.Lic_Generate_button);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.dateTimePicker1_expire);
            this.groupBox1.Controls.Add(this.comboBox1_Lic);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(24, 280);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(797, 196);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "New license";
            // 
            // textBox1_LicCode
            // 
            this.textBox1_LicCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1_LicCode.Location = new System.Drawing.Point(24, 90);
            this.textBox1_LicCode.Multiline = true;
            this.textBox1_LicCode.Name = "textBox1_LicCode";
            this.textBox1_LicCode.Size = new System.Drawing.Size(754, 91);
            this.textBox1_LicCode.TabIndex = 10;
            this.textBox1_LicCode.TextChanged += new System.EventHandler(this.textBox1_LicCode_TextChanged);
            // 
            // Lic_Generate_button
            // 
            this.Lic_Generate_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lic_Generate_button.Location = new System.Drawing.Point(605, 30);
            this.Lic_Generate_button.Name = "Lic_Generate_button";
            this.Lic_Generate_button.Size = new System.Drawing.Size(173, 32);
            this.Lic_Generate_button.TabIndex = 12;
            this.Lic_Generate_button.Text = "Generate";
            this.Lic_Generate_button.UseVisualStyleBackColor = true;
            this.Lic_Generate_button.Click += new System.EventHandler(this.Lic_Generate_button_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(258, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 20);
            this.label6.TabIndex = 11;
            this.label6.Text = "Expire on";
            // 
            // dateTimePicker1_expire
            // 
            this.dateTimePicker1_expire.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1_expire.Location = new System.Drawing.Point(352, 33);
            this.dateTimePicker1_expire.Name = "dateTimePicker1_expire";
            this.dateTimePicker1_expire.Size = new System.Drawing.Size(231, 22);
            this.dateTimePicker1_expire.TabIndex = 10;
            this.dateTimePicker1_expire.ValueChanged += new System.EventHandler(this.dateTimePicker1_expire_ValueChanged);
            // 
            // comboBox1_Lic
            // 
            this.comboBox1_Lic.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1_Lic.FormattingEnabled = true;
            this.comboBox1_Lic.Items.AddRange(new object[] {
            "On Line",
            "Off Line"});
            this.comboBox1_Lic.Location = new System.Drawing.Point(99, 31);
            this.comboBox1_Lic.Name = "comboBox1_Lic";
            this.comboBox1_Lic.Size = new System.Drawing.Size(136, 24);
            this.comboBox1_Lic.TabIndex = 9;
            this.comboBox1_Lic.SelectedIndexChanged += new System.EventHandler(this.comboBox1_Lic_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(15, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Lic Type";
            // 
            // button1_ok
            // 
            this.button1_ok.Location = new System.Drawing.Point(741, 496);
            this.button1_ok.Name = "button1_ok";
            this.button1_ok.Size = new System.Drawing.Size(80, 32);
            this.button1_ok.TabIndex = 10;
            this.button1_ok.Text = "Done";
            this.button1_ok.UseVisualStyleBackColor = true;
            this.button1_ok.Click += new System.EventHandler(this.button1_ok_Click);
            // 
            // clear_button
            // 
            this.clear_button.Location = new System.Drawing.Point(24, 496);
            this.clear_button.Name = "clear_button";
            this.clear_button.Size = new System.Drawing.Size(80, 32);
            this.clear_button.TabIndex = 11;
            this.clear_button.Text = "Clear";
            this.clear_button.UseVisualStyleBackColor = true;
            this.clear_button.Click += new System.EventHandler(this.clear_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 548);
            this.Controls.Add(this.clear_button);
            this.Controls.Add(this.button1_ok);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4_licType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4_appname);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1_shortMacCode);
            this.Controls.Add(this.button1_Decrypt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1_lockCode);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Lic Generation Utility";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1_lockCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1_Decrypt;
        private System.Windows.Forms.TextBox textBox1_shortMacCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4_appname;
        private System.Windows.Forms.Label label4_licType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1_LicCode;
        private System.Windows.Forms.Button Lic_Generate_button;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateTimePicker1_expire;
        private System.Windows.Forms.ComboBox comboBox1_Lic;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1_ok;
        private System.Windows.Forms.Button clear_button;
    }
}

