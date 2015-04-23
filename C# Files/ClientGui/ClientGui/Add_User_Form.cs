// Team author: DONT_BYTE_ME_BRO -- Jessie Delacenserie, Drew McClelland, Kameron Paulsen, Camille Rasmussen
// CS 3505 -- final project -- Collaborative Spreadsheet
// 4/23/15

using Model;
using ClientGui;
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

        /// <summary>
        /// Initializes the new form to add a user
        /// </summary>
        /// <param name="currModel"></param>
		public Add_User_Form(ClientModel currModel)
		{
            // Start the form and set the model
			InitializeComponent();
            model = currModel;

			// Process model events
            model = currModel;
            model.IncomingGenericErrorEvent += GenericErrorReceived;
            model.InvalidCommandEvent += InvalidCommandReceived;
            model.InvalidStateErrorEvent += InvalidStateRecieved;
            model.ConnectionLostErrorEvent += ConnectionErrorReceived;
            model.ConnectionConfirmationEvent += (string line) => { };
		}

        /// <summary>
        /// Send message to server when user clicks button to add user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button_add_Click(object sender, EventArgs e)
		{
            // Send message
			model.SendMessage("register " + textBox_username.Text + "\n");

            // Close the window
			this.Invoke(new Action(() =>
			{
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
        /// Show error message when connection is lost
        /// </summary>
        /// <param name="obj"></param>
        private void ConnectionErrorReceived(string obj)
        {
            MessageBox.Show(obj);
        }
	}
}
