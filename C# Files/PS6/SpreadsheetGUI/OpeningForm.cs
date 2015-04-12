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

namespace SpreadsheetGUI
{
	public partial class OpeningForm : Form
	{
		/// <summary>
		/// New model for processing
		/// </summary>
		private ClientModel model;

		/// <summary>
		/// Hardcoded port number for connection
		/// </summary>
		private int portno = 2113;

		/// <summary>
		/// Initialization of this form, the connection form
		/// </summary>
		public OpeningForm()
		{
			InitializeComponent();
			model = new ClientModel();
			model.IncomingLineEvent += MessageReceived;
		}

		/// <summary>
		/// Connect Button click - connects with the given IP address or host name
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			if (textBox_host.Text == "" || textBox_username.Text == "" || textBox_spreadsheet.Text == "")
				MessageBox.Show("Please fill out all fields.");
			else
				model.Connect(textBox_host.Text, portno, textBox_username.Text, textBox_spreadsheet.Text);
		}

		/// <summary>
		/// Message Received is the incoming line event. This happens whenever we get a 
		/// message from the server.
		/// </summary>
		/// <param name="line"></param>
		private void MessageReceived(String line)
		{
			// This is where we need to parse the message and figure out what to do with it.

			// If this is a connection confirmed message, hide the current form and open the spreadsheet.
			if (true) // once the parser class has been created, this if will actually make sense
			{
				this.Invoke(new Action(() =>
				{
					this.Hide();
					Form1 spForm = new Form1();
					spForm.Show();
				}));
			}

			// If it was an error message for connection attempt, pop up a box for that.
			// The details will need to be parsed from the command, such as invalid username etc.
			if (false)
				MessageBox.Show("Connection attempt failed. Here are the details: ");
		}

	}
}