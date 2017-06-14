namespace cspaint
{
    partial class Tools
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
        	//this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        	this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        	this.button1 = new System.Windows.Forms.Button();
        	this.button2 = new System.Windows.Forms.Button();
        	this.button3 = new System.Windows.Forms.Button();
        	this.colorDialog1 = new System.Windows.Forms.ColorDialog();
        	this.panel1 = new System.Windows.Forms.Panel();
        	this.button5 = new System.Windows.Forms.Button();
        	this.button4 = new System.Windows.Forms.Button();
        	this.checkBox1 = new System.Windows.Forms.CheckBox();
        	this.button6 = new System.Windows.Forms.Button();
        	this.panel2 = new System.Windows.Forms.Panel();
        	this.checkBox2 = new System.Windows.Forms.CheckBox();
        	this.button7 = new System.Windows.Forms.Button();
        	this.button8 = new System.Windows.Forms.Button();
        	this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
        	this.label1 = new System.Windows.Forms.Label();
        	this.SuspendLayout();

        	// openFileDialog1
        	// 
        	this.openFileDialog1.FileName = "openFileDialog1";
        	// 
        	// button1
        	// 
        	this.button1.Location = new System.Drawing.Point(3, 36);
        	this.button1.Name = "button1";
        	this.button1.Size = new System.Drawing.Size(57, 23);
        	this.button1.TabIndex = 1;
        	this.button1.Text = "Pen";
        	this.button1.UseVisualStyleBackColor = true;
        	this.button1.Click += new System.EventHandler(this.Button1Click);
        	// 
        	// button2
        	// 
        	this.button2.Location = new System.Drawing.Point(3, 65);
        	this.button2.Name = "button2";
        	this.button2.Size = new System.Drawing.Size(57, 23);
        	this.button2.TabIndex = 2;
        	this.button2.Text = "Rect";
        	this.button2.UseVisualStyleBackColor = true;
        	this.button2.Click += new System.EventHandler(this.Button2Click);
        	// 
        	// button3
        	// 
        	this.button3.Location = new System.Drawing.Point(3, 94);
        	this.button3.Name = "button3";
        	this.button3.Size = new System.Drawing.Size(57, 23);
        	this.button3.TabIndex = 3;
        	this.button3.Text = "Circle";
        	this.button3.UseVisualStyleBackColor = true;
        	this.button3.Click += new System.EventHandler(this.Button3Click);
        	// 
        	// panel1
        	// 
        	this.panel1.BackColor = System.Drawing.Color.Black;
        	this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        	this.panel1.Location = new System.Drawing.Point(83, 336);
        	this.panel1.Name = "panel1";
        	this.panel1.Size = new System.Drawing.Size(21, 22);
        	this.panel1.TabIndex = 5;
        	this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1Paint);
        	// 
        	// button5
        	// 
        	this.button5.Location = new System.Drawing.Point(3, 335);
        	this.button5.Name = "button5";
        	this.button5.Size = new System.Drawing.Size(74, 23);
        	this.button5.TabIndex = 6;
        	this.button5.Text = "Color";
        	this.button5.UseVisualStyleBackColor = true;
        	this.button5.Click += new System.EventHandler(this.Button5Click);
        	// 
        	// button4
        	// 
        	this.button4.Location = new System.Drawing.Point(3, 387);
        	this.button4.Name = "button4";
        	this.button4.Size = new System.Drawing.Size(74, 25);
        	this.button4.TabIndex = 7;
        	this.button4.Text = "List objects";
        	this.button4.UseVisualStyleBackColor = true;
        	this.button4.Click += new System.EventHandler(this.Button4Click);
        	// 
        	// checkBox1
        	// 
        	this.checkBox1.Checked = true;
        	this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
        	this.checkBox1.Location = new System.Drawing.Point(11, 263);
        	this.checkBox1.Name = "checkBox1";
        	this.checkBox1.Size = new System.Drawing.Size(66, 24);
        	this.checkBox1.TabIndex = 8;
        	this.checkBox1.Text = "Filled";
        	this.checkBox1.UseVisualStyleBackColor = true;
        	this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1CheckedChanged);
        	// 
        	// button6
        	// 
        	this.button6.Location = new System.Drawing.Point(3, 360);
        	this.button6.Name = "button6";
        	this.button6.Size = new System.Drawing.Size(74, 25);
        	this.button6.TabIndex = 9;
        	this.button6.Text = "Fill color";
        	this.button6.UseVisualStyleBackColor = true;
        	this.button6.Click += new System.EventHandler(this.Button6Click);
        	// 
        	// panel2
        	// 
        	this.panel2.BackColor = System.Drawing.Color.White;
        	this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        	this.panel2.Location = new System.Drawing.Point(83, 363);
        	this.panel2.Name = "panel2";
        	this.panel2.Size = new System.Drawing.Size(21, 22);
        	this.panel2.TabIndex = 6;
        	// 
        	// checkBox2
        	// 
        	this.checkBox2.Checked = true;
        	this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
        	this.checkBox2.Location = new System.Drawing.Point(11, 243);
        	this.checkBox2.Name = "checkBox2";
        	this.checkBox2.Size = new System.Drawing.Size(66, 24);
        	this.checkBox2.TabIndex = 10;
        	this.checkBox2.Text = "Closed";
        	this.checkBox2.UseVisualStyleBackColor = true;
        	this.checkBox2.CheckedChanged += new System.EventHandler(this.CheckBox2CheckedChanged);
        	// 
        	// button7
        	// 
        	this.button7.Location = new System.Drawing.Point(2, 287);
        	this.button7.Name = "button7";
        	this.button7.Size = new System.Drawing.Size(75, 23);
        	this.button7.TabIndex = 11;
        	this.button7.Text = "New layer";
        	this.button7.UseVisualStyleBackColor = true;
        	this.button7.Click += new System.EventHandler(this.Button7Click);
        	// 
        	// button8
        	// 
        	this.button8.Location = new System.Drawing.Point(2, 311);
        	this.button8.Name = "button8";
        	this.button8.Size = new System.Drawing.Size(75, 23);
        	this.button8.TabIndex = 12;
        	this.button8.Text = "Move layer";
        	this.button8.UseVisualStyleBackColor = true;
        	this.button8.Click += new System.EventHandler(this.Button8Click);
        	// 
        	// label1
        	// 
        	this.label1.Location = new System.Drawing.Point(0, 415);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(123, 13);
        	this.label1.TabIndex = 13;
        	this.label1.Text = "CSPaint by mjt 2006-07";
        	// 
        	// Tools
        	// 
        	this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.BackColor = System.Drawing.SystemColors.ButtonFace;
        	this.ClientSize = new System.Drawing.Size(123, 429);
        	this.Controls.Add(this.label1);
        	this.Controls.Add(this.button8);
        	this.Controls.Add(this.button7);
        	this.Controls.Add(this.checkBox2);
        	this.Controls.Add(this.panel2);
        	this.Controls.Add(this.button6);
        	this.Controls.Add(this.checkBox1);
        	this.Controls.Add(this.button4);
        	this.Controls.Add(this.button5);
        	this.Controls.Add(this.panel1);
        	this.Controls.Add(this.button3);
        	this.Controls.Add(this.button2);
        	this.Controls.Add(this.button1);
        	//this.Controls.Add(this.menuStrip1);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
        	this.Location = new System.Drawing.Point(0, 20);
        	//this.MainMenuStrip = this.menuStrip1;
        	this.MaximizeBox = false;
        	this.Name = "Tools";
        	this.ShowIcon = false;
        	this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
        	this.Text = "Tools";
        	this.TopMost = true;
        	this.Load += new System.EventHandler(this.Form1Load);
        	//this.menuStrip1.ResumeLayout(false);
        	//this.menuStrip1.PerformLayout();
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;

        #endregion

        //private System.Windows.Forms.MenuStrip menuStrip1;
/*        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        */
       
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        
        void Form1Load(object sender, System.EventArgs e)
        {
        	
        }
    }
}

