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

        public Form1()
        {
            InitializeComponent();
            model = new ClientModel();
            model.IncomingLineEvent += MessageReceived;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            model.Connect(textBox1.Text, 2113);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            model.SendMessage(textBox3.Text);
        }

        private void MessageReceived(String line)
        {
            textBox2.Invoke(new Action(() => { textBox2.Text += line + "\r\n"; }));
        }
    }
}
