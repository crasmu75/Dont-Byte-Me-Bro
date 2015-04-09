using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SS;

namespace WindowsFormsApplication1
{
	public partial class Form2 : Form
	{
		//Provides a spreadsheet to compute the calculator formulas.
		Spreadsheet CalcBackground;
		// Provides a string to represent the current formula.
		string CurrentString;
	
		/// <summary>
		/// Initialized the calculator form and sets the formula to be zero.
		/// </summary>
		public Form2()
		{
			InitializeComponent();
			CalcBackground = new Spreadsheet();
			CurrentString = "";
			setBox();
		
		}

		/// <summary>
		/// Sets the text box to match the formula.
		/// </summary>
		private void setBox()
		{
			if (CurrentString == "")
				textBox1.Text = "0";
			else
				textBox1.Text = CurrentString;

			textBox1.Focus();
			textBox1.Select(textBox1.Text.Length, 0);
		}

		/// <summary>
		/// Deletes the most recently entered character in the fomrula.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button18_Click(object sender, EventArgs e)
		{
			if (CurrentString.Length > 0)
				CurrentString = CurrentString.Remove(CurrentString.Length - 1);
			setBox();
		}

		/// <summary>
		/// Adds a zero character to the formula.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			CurrentString += "0";
			setBox();
		}

		/// <summary>
		/// Adds a decimal character to the formula.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void decimal1_Click(object sender, EventArgs e)
		{
			CurrentString += ".";
			setBox();
		}


		// ***************************************************************************************
		// The following ten methods provide button clicks for adding numbers 1-9 to the formula.
		//
		// ***************************************************************************************
		private void one_Click(object sender, EventArgs e)
		{
			CurrentString += "1";
			setBox();
		}

		private void two_Click(object sender, EventArgs e)
		{
			CurrentString += "2";
			setBox();
		}

		private void three_Click(object sender, EventArgs e)
		{
			CurrentString += "3";
			setBox();
		}

		private void four_Click(object sender, EventArgs e)
		{
			CurrentString += "4";
			setBox();
		}

		private void five_Click(object sender, EventArgs e)
		{
			CurrentString += "5";
			setBox();
		}

		private void six_Click(object sender, EventArgs e)
		{
			CurrentString += "6";
			setBox();
		}

		private void seven_Click(object sender, EventArgs e)
		{
			CurrentString += "7";
			setBox();
		}

		private void eight_Click(object sender, EventArgs e)
		{
			CurrentString += "8";
			setBox();
		}

		private void nine_Click(object sender, EventArgs e)
		{
			CurrentString += "9";
			setBox();
		}

		/// <summary>
		/// Adds a plus character to the formula.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void plus_Click(object sender, EventArgs e)
		{
			CurrentString += "+";
			setBox();
		}

		/// <summary>
		/// Adds a minux character to the formula.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void minus_Click(object sender, EventArgs e)
		{
			CurrentString += "-";
			setBox();
		}

		/// <summary>
		/// Adds a multiply character to the formula.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void multiply_Click(object sender, EventArgs e)
		{
			CurrentString += "*";
			setBox();
		}

		/// <summary>
		/// Adds a divide character to the formula.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void divide_Click(object sender, EventArgs e)
		{
			CurrentString += "/";
			setBox();
		}

		/// <summary>
		/// Adds a left parenthesis to the formula.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void leftParen_Click(object sender, EventArgs e)
		{
			CurrentString += "(";
			setBox();
		}

		/// <summary>
		/// Adds a right parenthesis to the formula.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void rightParen_Click(object sender, EventArgs e)
		{
			CurrentString += ")";
			setBox();
		}

		/// <summary>
		/// Clears the text in the box.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void clear_Click(object sender, EventArgs e)
		{
			CurrentString = "";
			setBox();
		}

		/// <summary>
		/// Computes the value within the text box. 
		/// Displays an error if necessary.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void equal_Click(object sender, EventArgs e)
		{
			if (CurrentString == "")
				return;
			try
			{
				CalcBackground.SetContentsOfCell("A1", "=" + CurrentString);
				CurrentString = CalcBackground.GetCellValue("A1").ToString();
				setBox();
			}
			catch (Exception)
			{
				MessageBox.Show("Invalid formula entered. Please try again.");
				CurrentString = "";
				setBox();
			}
		}


		/// <summary>
		/// Method for all of the key presses to match what the buttons do.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Back)
				button18_Click(null, null);

			if (e.KeyChar == (char)Keys.D0)
				button1_Click(null, null);

			if (e.KeyChar == '.')// (char)Keys.)
				decimal1_Click(null, null);

			if (e.KeyChar == (char)Keys.D1)
				one_Click(null, null);

			if (e.KeyChar == (char)Keys.D2)
				two_Click(null, null);

			if (e.KeyChar == (char)Keys.D3)
				three_Click(null, null);

			if (e.KeyChar == (char)Keys.D4)
				four_Click(null, null);

			if (e.KeyChar == (char)Keys.D5)
				five_Click(null, null);

			if (e.KeyChar == (char)Keys.D6)
				six_Click(null, null);

			if (e.KeyChar == (char)Keys.D7)
				seven_Click(null, null);

			if (e.KeyChar == (char)Keys.D8)
				eight_Click(null, null);

			if (e.KeyChar == (char)Keys.D9)
				nine_Click(null, null);

			if (e.KeyChar == '+')
				plus_Click(null, null);

			if (e.KeyChar == '-')
				minus_Click(null, null);

			if (e.KeyChar == '*')
				multiply_Click(null, null);

			if (e.KeyChar == '/')
				divide_Click(null, null);

			if (e.KeyChar == '(')
				leftParen_Click(null, null);

			if (e.KeyChar == ')')
				rightParen_Click(null, null);

			if (e.KeyChar == (char)Keys.Enter)
				equal_Click(null, null);


		}
	}
}
