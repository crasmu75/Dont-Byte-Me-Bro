namespace SpreadsheetGUI
{
	partial class SS_GUI_Form
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
			this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.saveCtrlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveCtrlSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.calculatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showCalculatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showHelpF1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.Cell_Name_Label = new System.Windows.Forms.Label();
			this.Cell_Name_Display = new System.Windows.Forms.TextBox();
			this.Cell_Content_Label = new System.Windows.Forms.Label();
			this.Cell_Content_Display = new System.Windows.Forms.TextBox();
			this.Cell_Value_Label = new System.Windows.Forms.Label();
			this.Cell_Value_Display = new System.Windows.Forms.TextBox();
			this.Set_Content_Button = new System.Windows.Forms.Button();
			this.usersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// spreadsheetPanel1
			// 
			this.spreadsheetPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 49);
			this.spreadsheetPanel1.Name = "spreadsheetPanel1";
			this.spreadsheetPanel1.Size = new System.Drawing.Size(1077, 631);
			this.spreadsheetPanel1.TabIndex = 0;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.calculatorToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.usersToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(6, 2, 0, 6);
			this.menuStrip1.Size = new System.Drawing.Size(1077, 27);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openFileMenu,
            this.saveCtrlToolStripMenuItem,
            this.saveCtrlSToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.closeAllToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 19);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.newToolStripMenuItem.Text = "New";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// openFileMenu
			// 
			this.openFileMenu.Name = "openFileMenu";
			this.openFileMenu.Size = new System.Drawing.Size(155, 22);
			this.openFileMenu.Text = "Open";
			this.openFileMenu.Click += new System.EventHandler(this.openFileMenu_Click);
			// 
			// saveCtrlToolStripMenuItem
			// 
			this.saveCtrlToolStripMenuItem.Name = "saveCtrlToolStripMenuItem";
			this.saveCtrlToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.saveCtrlToolStripMenuItem.Text = "Save ";
			// 
			// saveCtrlSToolStripMenuItem
			// 
			this.saveCtrlSToolStripMenuItem.Name = "saveCtrlSToolStripMenuItem";
			this.saveCtrlSToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.saveCtrlSToolStripMenuItem.Text = "Save As";
			this.saveCtrlSToolStripMenuItem.Click += new System.EventHandler(this.saveCtrlSToolStripMenuItem_Click);
			// 
			// closeToolStripMenuItem
			// 
			this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
			this.closeToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.closeToolStripMenuItem.Text = "Close (Alt + F4)";
			this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
			// 
			// closeAllToolStripMenuItem
			// 
			this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
			this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.closeAllToolStripMenuItem.Text = "Close All";
			this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.closeAllToolStripMenuItem_Click);
			// 
			// calculatorToolStripMenuItem
			// 
			this.calculatorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showCalculatorToolStripMenuItem});
			this.calculatorToolStripMenuItem.Name = "calculatorToolStripMenuItem";
			this.calculatorToolStripMenuItem.Size = new System.Drawing.Size(73, 19);
			this.calculatorToolStripMenuItem.Text = "Calculator";
			// 
			// showCalculatorToolStripMenuItem
			// 
			this.showCalculatorToolStripMenuItem.Name = "showCalculatorToolStripMenuItem";
			this.showCalculatorToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.showCalculatorToolStripMenuItem.Text = "Show Calculator";
			this.showCalculatorToolStripMenuItem.Click += new System.EventHandler(this.showCalculatorToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showHelpF1ToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 19);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// showHelpF1ToolStripMenuItem
			// 
			this.showHelpF1ToolStripMenuItem.Name = "showHelpF1ToolStripMenuItem";
			this.showHelpF1ToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
			this.showHelpF1ToolStripMenuItem.Text = "Show Help";
			this.showHelpF1ToolStripMenuItem.Click += new System.EventHandler(this.showHelpF1ToolStripMenuItem_Click);
			// 
			// Cell_Name_Label
			// 
			this.Cell_Name_Label.AutoSize = true;
			this.Cell_Name_Label.Dock = System.Windows.Forms.DockStyle.Top;
			this.Cell_Name_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Cell_Name_Label.Location = new System.Drawing.Point(0, 27);
			this.Cell_Name_Label.Name = "Cell_Name_Label";
			this.Cell_Name_Label.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
			this.Cell_Name_Label.Size = new System.Drawing.Size(74, 22);
			this.Cell_Name_Label.TabIndex = 2;
			this.Cell_Name_Label.Text = "Cell Name:";
			// 
			// Cell_Name_Display
			// 
			this.Cell_Name_Display.Location = new System.Drawing.Point(80, 26);
			this.Cell_Name_Display.Name = "Cell_Name_Display";
			this.Cell_Name_Display.ReadOnly = true;
			this.Cell_Name_Display.Size = new System.Drawing.Size(49, 20);
			this.Cell_Name_Display.TabIndex = 3;
			this.Cell_Name_Display.Text = "A1";
			// 
			// Cell_Content_Label
			// 
			this.Cell_Content_Label.AutoSize = true;
			this.Cell_Content_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Cell_Content_Label.Location = new System.Drawing.Point(435, 27);
			this.Cell_Content_Label.Name = "Cell_Content_Label";
			this.Cell_Content_Label.Size = new System.Drawing.Size(82, 16);
			this.Cell_Content_Label.TabIndex = 4;
			this.Cell_Content_Label.Text = "Cell Content:";
			// 
			// Cell_Content_Display
			// 
			this.Cell_Content_Display.Location = new System.Drawing.Point(523, 26);
			this.Cell_Content_Display.Name = "Cell_Content_Display";
			this.Cell_Content_Display.Size = new System.Drawing.Size(416, 20);
			this.Cell_Content_Display.TabIndex = 5;
			// 
			// Cell_Value_Label
			// 
			this.Cell_Value_Label.AutoSize = true;
			this.Cell_Value_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Cell_Value_Label.Location = new System.Drawing.Point(135, 27);
			this.Cell_Value_Label.Name = "Cell_Value_Label";
			this.Cell_Value_Label.Size = new System.Drawing.Size(72, 16);
			this.Cell_Value_Label.TabIndex = 6;
			this.Cell_Value_Label.Text = "Cell Value:";
			// 
			// Cell_Value_Display
			// 
			this.Cell_Value_Display.Location = new System.Drawing.Point(213, 26);
			this.Cell_Value_Display.Name = "Cell_Value_Display";
			this.Cell_Value_Display.ReadOnly = true;
			this.Cell_Value_Display.Size = new System.Drawing.Size(214, 20);
			this.Cell_Value_Display.TabIndex = 7;
			// 
			// Set_Content_Button
			// 
			this.Set_Content_Button.Location = new System.Drawing.Point(945, 24);
			this.Set_Content_Button.Name = "Set_Content_Button";
			this.Set_Content_Button.Size = new System.Drawing.Size(75, 23);
			this.Set_Content_Button.TabIndex = 8;
			this.Set_Content_Button.Text = "Set Content";
			this.Set_Content_Button.UseVisualStyleBackColor = true;
			this.Set_Content_Button.Click += new System.EventHandler(this.Set_Content_Button_Click);
			// 
			// usersToolStripMenuItem
			// 
			this.usersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addUserToolStripMenuItem});
			this.usersToolStripMenuItem.Name = "usersToolStripMenuItem";
			this.usersToolStripMenuItem.Size = new System.Drawing.Size(47, 19);
			this.usersToolStripMenuItem.Text = "Users";
			// 
			// addUserToolStripMenuItem
			// 
			this.addUserToolStripMenuItem.Name = "addUserToolStripMenuItem";
			this.addUserToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.addUserToolStripMenuItem.Text = "Add User";
			this.addUserToolStripMenuItem.Click += new System.EventHandler(this.addUserToolStripMenuItem_Click);
			// 
			// SS_GUI_Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1077, 680);
			this.Controls.Add(this.Set_Content_Button);
			this.Controls.Add(this.spreadsheetPanel1);
			this.Controls.Add(this.Cell_Value_Display);
			this.Controls.Add(this.Cell_Value_Label);
			this.Controls.Add(this.Cell_Content_Display);
			this.Controls.Add(this.Cell_Content_Label);
			this.Controls.Add(this.Cell_Name_Display);
			this.Controls.Add(this.Cell_Name_Label);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "SS_GUI_Form";
			this.Text = "Spreadsheet";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SS.SpreadsheetPanel spreadsheetPanel1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openFileMenu;
		private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem closeAllToolStripMenuItem;
		private System.Windows.Forms.Label Cell_Name_Label;
		private System.Windows.Forms.TextBox Cell_Name_Display;
		private System.Windows.Forms.Label Cell_Content_Label;
		private System.Windows.Forms.TextBox Cell_Content_Display;
		private System.Windows.Forms.Label Cell_Value_Label;
		private System.Windows.Forms.TextBox Cell_Value_Display;
		private System.Windows.Forms.ToolStripMenuItem saveCtrlSToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveCtrlToolStripMenuItem;
		private System.Windows.Forms.Button Set_Content_Button;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showHelpF1ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem calculatorToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showCalculatorToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem usersToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addUserToolStripMenuItem;
	}
}

