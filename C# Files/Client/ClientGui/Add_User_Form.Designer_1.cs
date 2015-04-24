namespace ClientGui
{
	partial class Add_User_Form
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
			this.label_new_user = new System.Windows.Forms.Label();
			this.textBox_username = new System.Windows.Forms.TextBox();
			this.button_add = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label_new_user
			// 
			this.label_new_user.AutoSize = true;
			this.label_new_user.Location = new System.Drawing.Point(40, 45);
			this.label_new_user.Name = "label_new_user";
			this.label_new_user.Size = new System.Drawing.Size(86, 13);
			this.label_new_user.TabIndex = 0;
			this.label_new_user.Text = "New Username: ";
			// 
			// textBox_username
			// 
			this.textBox_username.Location = new System.Drawing.Point(132, 42);
			this.textBox_username.Name = "textBox_username";
			this.textBox_username.Size = new System.Drawing.Size(115, 20);
			this.textBox_username.TabIndex = 1;
			// 
			// button_add
			// 
			this.button_add.Location = new System.Drawing.Point(112, 116);
			this.button_add.Name = "button_add";
			this.button_add.Size = new System.Drawing.Size(75, 23);
			this.button_add.TabIndex = 2;
			this.button_add.Text = "Add";
			this.button_add.UseVisualStyleBackColor = true;
			this.button_add.Click += new System.EventHandler(this.button_add_Click);
			// 
			// Add_User_Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(289, 169);
			this.Controls.Add(this.button_add);
			this.Controls.Add(this.textBox_username);
			this.Controls.Add(this.label_new_user);
			this.Name = "Add_User_Form";
			this.Text = "Add_User_Form";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label_new_user;
		private System.Windows.Forms.TextBox textBox_username;
		private System.Windows.Forms.Button button_add;
	}
}