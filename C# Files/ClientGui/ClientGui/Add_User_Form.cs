using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientGui
{
	public partial class Add_User_Form : Form
	{
		/// <summary>
		/// New model for processing
		/// </summary>
		private ClientModel model;

		public Add_User_Form(ClientModel currModel)
		{
			InitializeComponent();

			// Added to connect to server
			model = currModel;
		}

		private void button_add_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Username being added.");
			model.SendMessage("register " + textBox_username.Text);
			this.Invoke(new Action(() =>
			{
				this.Hide();
			}));
		}
	}
}
