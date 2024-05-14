namespace Student_Manager
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
            this.timelbl = new System.Windows.Forms.Label();
            this.fileselectbtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ENCweekbtn = new System.Windows.Forms.Button();
            this.ENC4daybtn = new System.Windows.Forms.Button();
            this.ENC1daybtn = new System.Windows.Forms.Button();
            this.ENCtodaybtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fllwupcall14btn = new System.Windows.Forms.Button();
            this.fllwupcall10btn = new System.Windows.Forms.Button();
            this.fllwupcall7btn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button14 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.selectedFileLbl = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.ActiveLbl = new System.Windows.Forms.Label();
            this.InactiveLbl = new System.Windows.Forms.Label();
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // timelbl
            // 
            this.timelbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.timelbl.AutoSize = true;
            this.timelbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timelbl.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.timelbl.Location = new System.Drawing.Point(1289, 9);
            this.timelbl.Name = "timelbl";
            this.timelbl.Size = new System.Drawing.Size(57, 20);
            this.timelbl.TabIndex = 0;
            this.timelbl.Text = "label1";
            // 
            // fileselectbtn
            // 
            this.fileselectbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileselectbtn.Location = new System.Drawing.Point(13, 9);
            this.fileselectbtn.Name = "fileselectbtn";
            this.fileselectbtn.Size = new System.Drawing.Size(129, 33);
            this.fileselectbtn.TabIndex = 1;
            this.fileselectbtn.Text = "Select Action List";
            this.fileselectbtn.UseVisualStyleBackColor = true;
            this.fileselectbtn.Click += new System.EventHandler(this.fileselectbtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ENCweekbtn);
            this.groupBox1.Controls.Add(this.ENC4daybtn);
            this.groupBox1.Controls.Add(this.ENC1daybtn);
            this.groupBox1.Controls.Add(this.ENCtodaybtn);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(150, 195);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ENC BOOKINGS";
            // 
            // ENCweekbtn
            // 
            this.ENCweekbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ENCweekbtn.Location = new System.Drawing.Point(6, 153);
            this.ENCweekbtn.Name = "ENCweekbtn";
            this.ENCweekbtn.Size = new System.Drawing.Size(117, 33);
            this.ENCweekbtn.TabIndex = 3;
            this.ENCweekbtn.Text = "7 Days";
            this.ENCweekbtn.UseVisualStyleBackColor = true;
            this.ENCweekbtn.Click += new System.EventHandler(this.ENCweekbtn_Click);
            // 
            // ENC4daybtn
            // 
            this.ENC4daybtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ENC4daybtn.Location = new System.Drawing.Point(6, 114);
            this.ENC4daybtn.Name = "ENC4daybtn";
            this.ENC4daybtn.Size = new System.Drawing.Size(117, 33);
            this.ENC4daybtn.TabIndex = 2;
            this.ENC4daybtn.Text = "3 Days";
            this.ENC4daybtn.UseVisualStyleBackColor = true;
            this.ENC4daybtn.Click += new System.EventHandler(this.ENC4daybtn_Click);
            // 
            // ENC1daybtn
            // 
            this.ENC1daybtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ENC1daybtn.Location = new System.Drawing.Point(6, 75);
            this.ENC1daybtn.Name = "ENC1daybtn";
            this.ENC1daybtn.Size = new System.Drawing.Size(117, 33);
            this.ENC1daybtn.TabIndex = 1;
            this.ENC1daybtn.Text = "TOMORROW";
            this.ENC1daybtn.UseVisualStyleBackColor = true;
            this.ENC1daybtn.Click += new System.EventHandler(this.ENC1daybtn_Click);
            // 
            // ENCtodaybtn
            // 
            this.ENCtodaybtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ENCtodaybtn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ENCtodaybtn.Location = new System.Drawing.Point(6, 36);
            this.ENCtodaybtn.Name = "ENCtodaybtn";
            this.ENCtodaybtn.Size = new System.Drawing.Size(117, 33);
            this.ENCtodaybtn.TabIndex = 0;
            this.ENCtodaybtn.Text = "TODAY";
            this.ENCtodaybtn.UseVisualStyleBackColor = true;
            this.ENCtodaybtn.Click += new System.EventHandler(this.ENCtodaybtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.fllwupcall14btn);
            this.groupBox2.Controls.Add(this.fllwupcall10btn);
            this.groupBox2.Controls.Add(this.fllwupcall7btn);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 368);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(150, 159);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Encouragement Calls";
            // 
            // fllwupcall14btn
            // 
            this.fllwupcall14btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fllwupcall14btn.Location = new System.Drawing.Point(6, 118);
            this.fllwupcall14btn.Name = "fllwupcall14btn";
            this.fllwupcall14btn.Size = new System.Drawing.Size(117, 33);
            this.fllwupcall14btn.TabIndex = 6;
            this.fllwupcall14btn.Text = "14+ Days";
            this.fllwupcall14btn.UseVisualStyleBackColor = true;
            this.fllwupcall14btn.Click += new System.EventHandler(this.fllwupcall14btn_Click);
            // 
            // fllwupcall10btn
            // 
            this.fllwupcall10btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fllwupcall10btn.Location = new System.Drawing.Point(6, 79);
            this.fllwupcall10btn.Name = "fllwupcall10btn";
            this.fllwupcall10btn.Size = new System.Drawing.Size(117, 33);
            this.fllwupcall10btn.TabIndex = 5;
            this.fllwupcall10btn.Text = "10 Days";
            this.fllwupcall10btn.UseVisualStyleBackColor = true;
            this.fllwupcall10btn.Click += new System.EventHandler(this.fllwupcall10btn_Click);
            // 
            // fllwupcall7btn
            // 
            this.fllwupcall7btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fllwupcall7btn.Location = new System.Drawing.Point(6, 40);
            this.fllwupcall7btn.Name = "fllwupcall7btn";
            this.fllwupcall7btn.Size = new System.Drawing.Size(117, 33);
            this.fllwupcall7btn.TabIndex = 4;
            this.fllwupcall7btn.Text = "7 Days";
            this.fllwupcall7btn.UseVisualStyleBackColor = true;
            this.fllwupcall7btn.Click += new System.EventHandler(this.fllwupcall7btn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button14);
            this.groupBox3.Controls.Add(this.button13);
            this.groupBox3.Controls.Add(this.button12);
            this.groupBox3.Controls.Add(this.button11);
            this.groupBox3.Controls.Add(this.button10);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(13, 549);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(149, 271);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "CONTRACT MANAGEMENT";
            // 
            // button14
            // 
            this.button14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button14.Location = new System.Drawing.Point(6, 221);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(116, 44);
            this.button14.TabIndex = 5;
            this.button14.Text = "Booked SC/CC today";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button13
            // 
            this.button13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button13.Location = new System.Drawing.Point(7, 171);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(116, 44);
            this.button13.TabIndex = 3;
            this.button13.Text = "Full Access Students";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button12
            // 
            this.button12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button12.Location = new System.Drawing.Point(6, 121);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(116, 44);
            this.button12.TabIndex = 2;
            this.button12.Text = "INACTIVE Students";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button11
            // 
            this.button11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button11.Location = new System.Drawing.Point(6, 82);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(116, 33);
            this.button11.TabIndex = 1;
            this.button11.Text = "ACTIVE Students";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button10
            // 
            this.button10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.Location = new System.Drawing.Point(6, 43);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(116, 33);
            this.button10.TabIndex = 0;
            this.button10.Text = "ALL Students";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Location = new System.Drawing.Point(196, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1244, 758);
            this.panel1.TabIndex = 5;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1244, 758);
            this.dataGridView1.TabIndex = 0;
            // 
            // selectedFileLbl
            // 
            this.selectedFileLbl.AutoSize = true;
            this.selectedFileLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedFileLbl.Location = new System.Drawing.Point(148, 13);
            this.selectedFileLbl.Name = "selectedFileLbl";
            this.selectedFileLbl.Size = new System.Drawing.Size(0, 16);
            this.selectedFileLbl.TabIndex = 6;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(18, 260);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(144, 89);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "TOUCHPOINTS";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(7, 52);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(114, 26);
            this.button2.TabIndex = 1;
            this.button2.Text = "ADV TODAY";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(7, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 26);
            this.button1.TabIndex = 0;
            this.button1.Text = "First ENC";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ActiveLbl
            // 
            this.ActiveLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ActiveLbl.AutoSize = true;
            this.ActiveLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActiveLbl.Location = new System.Drawing.Point(1296, 813);
            this.ActiveLbl.Name = "ActiveLbl";
            this.ActiveLbl.Size = new System.Drawing.Size(50, 16);
            this.ActiveLbl.TabIndex = 6;
            this.ActiveLbl.Text = "label1";
            // 
            // InactiveLbl
            // 
            this.InactiveLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.InactiveLbl.AutoSize = true;
            this.InactiveLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InactiveLbl.Location = new System.Drawing.Point(1127, 813);
            this.InactiveLbl.Name = "InactiveLbl";
            this.InactiveLbl.Size = new System.Drawing.Size(50, 16);
            this.InactiveLbl.TabIndex = 8;
            this.InactiveLbl.Text = "label1";
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(196, 16);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(352, 20);
            this.SearchTextBox.TabIndex = 9;
            this.SearchTextBox.TextChanged += new System.EventHandler(this.SearchTextBox_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1452, 832);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.InactiveLbl);
            this.Controls.Add(this.ActiveLbl);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.selectedFileLbl);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.fileselectbtn);
            this.Controls.Add(this.timelbl);
            this.Name = "Form1";
            this.Text = "Student Manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label timelbl;
        private System.Windows.Forms.Button fileselectbtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ENCtodaybtn;
        private System.Windows.Forms.Button ENCweekbtn;
        private System.Windows.Forms.Button ENC4daybtn;
        private System.Windows.Forms.Button ENC1daybtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button fllwupcall14btn;
        private System.Windows.Forms.Button fllwupcall10btn;
        private System.Windows.Forms.Button fllwupcall7btn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label selectedFileLbl;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label ActiveLbl;
        private System.Windows.Forms.Label InactiveLbl;
        private System.Windows.Forms.TextBox SearchTextBox;
    }
}

