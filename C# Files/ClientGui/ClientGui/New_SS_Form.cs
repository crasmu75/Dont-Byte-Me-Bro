using Model;
using SpreadsheetGUI;
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
	public partial class New_SS_Form : Form
	{
		ClientModel model, newModel;

		public New_SS_Form(ClientModel currModel)
		{
			model = currModel;
			newModel = new ClientModel();
			newModel.ConnectionConfirmationEvent += ConnectionReceived;
			model.testingevent += testinggggg;
			newModel.testingevent += testinggggg;

			InitializeComponent();
		}

		private void testinggggg(string obj)
		{
			MessageBox.Show("Message received from server. Message:\n" + obj);
		}

		private void ConnectionReceived(string obj)
		{
			this.Invoke(new Action(() =>
			{
				

				// make sure to pass in the new model that has the new socket
				DemoApplicationContext.getAppContext().RunForm(new SS_GUI_Form(newModel, textBox_spreadsheet_name.Text));
				this.Close();
			}));
		}

		private void button_open_Click(object sender, EventArgs e)
		{
			// attempt connection of the new socket with the old host, port, client name, and new spreadsheet name
			newModel.Connect(model.host, model.portn, model.clientn, textBox_spreadsheet_name.Text);
		}
	}
}
