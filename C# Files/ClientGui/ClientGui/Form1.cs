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
	public partial class Form1 : Form
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
        public Form1()
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
            model.Connect(textBox_host.Text, portno);
        }

		/// <summary>
		/// Message Received is the incoming line event. This happens whenever we get a 
		/// message from the server.
		/// </summary>
		/// <param name="line"></param>
        private void MessageReceived(String line)
        {
			// This is where we need to parse the message and figure out what to do with it.
        }
    }
}
