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
	public partial class New_SS_Form : Form
	{
		ClientModel model;

		public New_SS_Form(ClientModel currModel)
		{
			model = currModel;
			InitializeComponent();
		}
	}
}
