namespace ClientGui
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
			this.textBox_host = new System.Windows.Forms.TextBox();
			this.label_welcome = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.label_host = new System.Windows.Forms.Label();
			this.label_welcome1 = new System.Windows.Forms.Label();
			this.label_username = new System.Windows.Forms.Label();
			this.textBox_username = new System.Windows.Forms.TextBox();
			this.label_spreadsheet = new System.Windows.Forms.Label();
			this.textBox_spreadsheet = new System.Windows.Forms.TextBox();
			this.label_note = new System.Windows.Forms.Label();
			this.label_note1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textBox_host
			// 
			this.textBox_host.Location = new System.Drawing.Point(214, 77);
			this.textBox_host.Name = "textBox_host";
			this.textBox_host.Size = new System.Drawing.Size(115, 20);
			this.textBox_host.TabIndex = 0;
			// 
			// label_welcome
			// 
			this.label_welcome.AutoSize = true;
			this.label_welcome.Font = new System.Drawing.Font("Miramonte", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label_welcome.Location = new System.Drawing.Point(37, 12);
			this.label_welcome.Name = "label_welcome";
			this.label_welcome.Size = new System.Drawing.Size(350, 24);
			this.label_welcome.TabIndex = 1;
			this.label_welcome.Text = "Welcome to the Spreadsheet Application!";
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.GhostWhite;
			this.button1.Font = new System.Drawing.Font("Miramonte", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.Location = new System.Drawing.Point(176, 239);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "Connect";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label_host
			// 
			this.label_host.AutoSize = true;
			this.label_host.Font = new System.Drawing.Font("Miramonte", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label_host.Location = new System.Drawing.Point(97, 80);
			this.label_host.Name = "label_host";
			this.label_host.Size = new System.Drawing.Size(37, 16);
			this.label_host.TabIndex = 6;
			this.label_host.Text = "Host:";
			// 
			// label_welcome1
			// 
			this.label_welcome1.AutoSize = true;
			this.label_welcome1.Font = new System.Drawing.Font("Miramonte", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label_welcome1.Location = new System.Drawing.Point(118, 36);
			this.label_welcome1.Name = "label_welcome1";
			this.label_welcome1.Size = new System.Drawing.Size(193, 16);
			this.label_welcome1.TabIndex = 7;
			this.label_welcome1.Text = "Please fill out all fields to connect.";
			// 
			// label_username
			// 
			this.label_username.AutoSize = true;
			this.label_username.Font = new System.Drawing.Font("Miramonte", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label_username.Location = new System.Drawing.Point(97, 111);
			this.label_username.Name = "label_username";
			this.label_username.Size = new System.Drawing.Size(69, 16);
			this.label_username.TabIndex = 8;
			this.label_username.Text = "Username:";
			// 
			// textBox_username
			// 
			this.textBox_username.Location = new System.Drawing.Point(214, 111);
			this.textBox_username.Name = "textBox_username";
			this.textBox_username.Size = new System.Drawing.Size(115, 20);
			this.textBox_username.TabIndex = 9;
			// 
			// label_spreadsheet
			// 
			this.label_spreadsheet.AutoSize = true;
			this.label_spreadsheet.Font = new System.Drawing.Font("Miramonte", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label_spreadsheet.Location = new System.Drawing.Point(97, 195);
			this.label_spreadsheet.Name = "label_spreadsheet";
			this.label_spreadsheet.Size = new System.Drawing.Size(117, 16);
			this.label_spreadsheet.TabIndex = 10;
			this.label_spreadsheet.Text = "Spreadsheet Name:";
			// 
			// textBox_spreadsheet
			// 
			this.textBox_spreadsheet.Location = new System.Drawing.Point(214, 192);
			this.textBox_spreadsheet.Name = "textBox_spreadsheet";
			this.textBox_spreadsheet.Size = new System.Drawing.Size(115, 20);
			this.textBox_spreadsheet.TabIndex = 11;
			// 
			// label_note
			// 
			this.label_note.AutoSize = true;
			this.label_note.Font = new System.Drawing.Font("Miramonte", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label_note.Location = new System.Drawing.Point(27, 146);
			this.label_note.Name = "label_note";
			this.label_note.Size = new System.Drawing.Size(373, 16);
			this.label_note.TabIndex = 12;
			this.label_note.Text = "Enter the name of the spreadsheet on the server you wish to open. ";
			// 
			// label_note1
			// 
			this.label_note1.AutoSize = true;
			this.label_note1.Font = new System.Drawing.Font("Miramonte", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label_note1.Location = new System.Drawing.Point(2, 162);
			this.label_note1.Name = "label_note1";
			this.label_note1.Size = new System.Drawing.Size(417, 16);
			this.label_note1.TabIndex = 13;
			this.label_note1.Text = "If you wish to open a new spreadsheet, enter the name you want it to have.";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.MintCream;
			this.ClientSize = new System.Drawing.Size(422, 287);
			this.Controls.Add(this.label_note1);
			this.Controls.Add(this.label_note);
			this.Controls.Add(this.textBox_spreadsheet);
			this.Controls.Add(this.label_spreadsheet);
			this.Controls.Add(this.textBox_username);
			this.Controls.Add(this.label_username);
			this.Controls.Add(this.label_welcome1);
			this.Controls.Add(this.label_host);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label_welcome);
			this.Controls.Add(this.textBox_host);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBox_host;
		private System.Windows.Forms.Label label_welcome;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label_host;
		private System.Windows.Forms.Label label_welcome1;
		private System.Windows.Forms.Label label_username;
		private System.Windows.Forms.TextBox textBox_username;
		private System.Windows.Forms.Label label_spreadsheet;
		private System.Windows.Forms.TextBox textBox_spreadsheet;
		private System.Windows.Forms.Label label_note;
		private System.Windows.Forms.Label label_note1;
	}
}

