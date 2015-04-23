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
using SpreadsheetGUI;
using System.Threading;

namespace ClientGui
{
	public partial class Login_Form : Form
	{
		/// <summary>
		/// New model for processing
		/// </summary>
		private ClientModel model;

		/// <summary>
		/// Initialization of this form, the connection form
		/// </summary>
        public Login_Form()
        {
            InitializeComponent();
            model = new ClientModel();
            model.ConnectionConfirmationEvent += MessageReceived;
			model.testingevent += testgghgh;
			model.IncomingErrorEvent += ErrorReceived;
			model.IncomingUsernameErrorEvent += UsernameError;
        }

		private void UsernameError(string obj)
		{
			MessageBox.Show("Username not registered or currently in use. Try again.");
		}

		private void ErrorReceived(string obj)
		{
			//MessageBox.Show("An error occured: \n\n" + obj);
		}

		private void testgghgh(string obj)
		{
			MessageBox.Show("Message received from server. Message:\n" + obj);
		}

		/// <summary>
		/// Connect Button click - connects with the given IP address or host name
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            int n;
            bool isNumber = int.TryParse(textBox_portno.Text, out n);

			if (textBox_host.Text == "" || textBox_username.Text == "" || textBox_spreadsheet.Text == "" || textBox_portno.Text == "")
				MessageBox.Show("Please fill out all fields.");
			else
                if (isNumber)
				    model.Connect(textBox_host.Text, n, textBox_username.Text, textBox_spreadsheet.Text);
        }

		/// <summary>
		/// Message Received is the incoming line event. This happens whenever we get a 
		/// message from the server.
		/// </summary>
		/// <param name="line"></param>
        private void MessageReceived(String line)
        {
			this.Invoke(new Action(() =>
				{
					DemoApplicationContext.getAppContext().RunForm(new SS_GUI_Form(model, model.spreadsheet));
					this.Close();
				}));
        }
    }
}
