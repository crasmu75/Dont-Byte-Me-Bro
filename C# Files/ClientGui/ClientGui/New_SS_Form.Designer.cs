namespace ClientGui
{
	partial class New_SS_Form
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
			this.textBox_spreadsheet_name = new System.Windows.Forms.TextBox();
			this.label_spreadsheet = new System.Windows.Forms.Label();
			this.button_open = new System.Windows.Forms.Button();
			this.label_instructions = new System.Windows.Forms.Label();
			this.label_instructions1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textBox_spreadsheet_name
			// 
			this.textBox_spreadsheet_name.Location = new System.Drawing.Point(157, 96);
			this.textBox_spreadsheet_name.Name = "textBox_spreadsheet_name";
			this.textBox_spreadsheet_name.Size = new System.Drawing.Size(168, 20);
			this.textBox_spreadsheet_name.TabIndex = 0;
			// 
			// label_spreadsheet
			// 
			this.label_spreadsheet.AutoSize = true;
			this.label_spreadsheet.Location = new System.Drawing.Point(50, 99);
			this.label_spreadsheet.Name = "label_spreadsheet";
			this.label_spreadsheet.Size = new System.Drawing.Size(101, 13);
			this.label_spreadsheet.TabIndex = 1;
			this.label_spreadsheet.Text = "Spreadsheet Name:";
			// 
			// button_open
			// 
			this.button_open.Location = new System.Drawing.Point(157, 160);
			this.button_open.Name = "button_open";
			this.button_open.Size = new System.Drawing.Size(75, 23);
			this.button_open.TabIndex = 2;
			this.button_open.Text = "Open";
			this.button_open.UseVisualStyleBackColor = true;
			this.button_open.Click += new System.EventHandler(this.button_open_Click);
			// 
			// label_instructions
			// 
			this.label_instructions.AutoSize = true;
			this.label_instructions.Location = new System.Drawing.Point(62, 33);
			this.label_instructions.Name = "label_instructions";
			this.label_instructions.Size = new System.Drawing.Size(256, 13);
			this.label_instructions.TabIndex = 3;
			this.label_instructions.Text = "Enter the name of the spreadsheet you wish to open.";
			// 
			// label_instructions1
			// 
			this.label_instructions1.AutoSize = true;
			this.label_instructions1.Location = new System.Drawing.Point(10, 58);
			this.label_instructions1.Name = "label_instructions1";
			this.label_instructions1.Size = new System.Drawing.Size(362, 13);
			this.label_instructions1.TabIndex = 4;
			this.label_instructions1.Text = "If you wish to open a new spreadsheet, enter the name you want it to have.";
			// 
			// New_SS_Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 204);
			this.Controls.Add(this.label_instructions1);
			this.Controls.Add(this.label_instructions);
			this.Controls.Add(this.button_open);
			this.Controls.Add(this.label_spreadsheet);
			this.Controls.Add(this.textBox_spreadsheet_name);
			this.Name = "New_SS_Form";
			this.Text = "New_SS_Form";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBox_spreadsheet_name;
		private System.Windows.Forms.Label label_spreadsheet;
		private System.Windows.Forms.Button button_open;
		private System.Windows.Forms.Label label_instructions;
		private System.Windows.Forms.Label label_instructions1;
	}
}