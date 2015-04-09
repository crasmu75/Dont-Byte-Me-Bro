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
			this.SuspendLayout();
			// 
			// textBox_host
			// 
			this.textBox_host.Location = new System.Drawing.Point(87, 77);
			this.textBox_host.Name = "textBox_host";
			this.textBox_host.Size = new System.Drawing.Size(115, 20);
			this.textBox_host.TabIndex = 0;
			// 
			// label_welcome
			// 
			this.label_welcome.AutoSize = true;
			this.label_welcome.Location = new System.Drawing.Point(64, 20);
			this.label_welcome.Name = "label_welcome";
			this.label_welcome.Size = new System.Drawing.Size(203, 13);
			this.label_welcome.TabIndex = 1;
			this.label_welcome.Text = "Welcome to the Spreadsheet Application!";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(127, 207);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "Connect";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label_host
			// 
			this.label_host.AutoSize = true;
			this.label_host.Location = new System.Drawing.Point(49, 80);
			this.label_host.Name = "label_host";
			this.label_host.Size = new System.Drawing.Size(32, 13);
			this.label_host.TabIndex = 6;
			this.label_host.Text = "Host:";
			// 
			// label_welcome1
			// 
			this.label_welcome1.AutoSize = true;
			this.label_welcome1.Location = new System.Drawing.Point(84, 36);
			this.label_welcome1.Name = "label_welcome1";
			this.label_welcome1.Size = new System.Drawing.Size(166, 13);
			this.label_welcome1.TabIndex = 7;
			this.label_welcome1.Text = "Please fill out all fields to connect.";
			// 
			// label_username
			// 
			this.label_username.AutoSize = true;
			this.label_username.Location = new System.Drawing.Point(49, 111);
			this.label_username.Name = "label_username";
			this.label_username.Size = new System.Drawing.Size(58, 13);
			this.label_username.TabIndex = 8;
			this.label_username.Text = "Username:";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(332, 242);
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
	}
}

