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
		private ClientModel model;
		private int portno = 2113;

        public Form1()
        {
            InitializeComponent();
            model = new ClientModel();
            model.IncomingLineEvent += MessageReceived;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            model.Connect(textBox1.Text, portno);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            model.SendMessage(textBox3.Text);
        }

        private void MessageReceived(String line)
        {
			Console.WriteLine("Got here: " + line);
            textBox2.Invoke(new Action(() => { textBox2.Text += line + "\r\n"; }));
        }
    }
}
