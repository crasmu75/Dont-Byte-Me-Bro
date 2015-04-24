// Team author: DONT_BYTE_ME_BRO -- Jessie Delacenserie, Drew McClelland, Kameron Paulsen, Camille Rasmussen
// CS 3505 -- final project -- Collaborative Spreadsheet
// 4/23/15

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
            // Start the form and set model
            InitializeComponent();
            model = new ClientModel();

            // Process model events
            model.ConnectionConfirmationEvent += MessageReceived;
            model.IncomingGenericErrorEvent += GenericErrorReceived;
			model.IncomingUsernameErrorEvent += UsernameError;
            model.InvalidCommandEvent += InvalidCommandReceived;
            model.InvalidStateErrorEvent += InvalidStateRecieved;
			model.ConnectionLostErrorEvent += ConnectionErrorReceived;
        }

        /// <summary>
        /// Connect Button click - connects with the given IP address or host name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // Checks for a valid port number (must be an int)
            int n;
            bool isNumber = int.TryParse(textBox_portno.Text, out n);

            // Checks that all fields are filled out and sends message to server
			if (textBox_host.Text == "" || textBox_username.Text == "" || textBox_spreadsheet.Text == "" || textBox_portno.Text == "")
				MessageBox.Show("Please fill out all fields.");
			else
			{
				if (isNumber)
					model.Connect(textBox_host.Text, n, textBox_username.Text, textBox_spreadsheet.Text);
				else
					MessageBox.Show("Port Number must be an integer.");
			}
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

        /// <summary>
        /// Show error message when request cannot be completed in current state
        /// </summary>
        /// <param name="obj"></param>
        private void InvalidStateRecieved(string obj)
        {
            MessageBox.Show("ERROR:\n\n" + obj);
        }

        /// <summary>
        /// Show error message when any other generic error is sent by server
        /// </summary>
        /// <param name="obj"></param>
        private void GenericErrorReceived(string obj)
        {
            MessageBox.Show("ERROR: \n\n" + obj);
        }

        /// <summary>
        /// Show error message when invalid command was received by server
        /// </summary>
        /// <param name="obj"></param>
        private void InvalidCommandReceived(string obj)
        {
            MessageBox.Show("ERROR:\nInvalid command received: " + obj);
        }

        /// <summary>
        /// Show error message when invalid username is received by server
        /// </summary>
        /// <param name="obj"></param>
		private void UsernameError(string obj)
		{
			MessageBox.Show("ERROR: " + obj + " is not a registered username. Try again.");
		}

        /// <summary>
        /// Show error message when connection is lost
        /// </summary>
        /// <param name="obj"></param>
		private void ConnectionErrorReceived(string obj)
		{
			MessageBox.Show(obj);
		}
    }
}
