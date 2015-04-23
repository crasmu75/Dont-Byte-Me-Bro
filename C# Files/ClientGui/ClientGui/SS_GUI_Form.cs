// Team author: DONT_BYTE_ME_BRO -- Jessie Delacenserie, Drew McClelland, Kameron Paulsen, Camille Rasmussen
// CS 3505 -- final project -- Collaborative Spreadsheet
// 4/23/15

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
using System.Text.RegularExpressions;
using WindowsFormsApplication1;
using Model;
using ClientGui;

namespace SpreadsheetGUI
{
	public partial class SS_GUI_Form : Form
	{
        /// <summary>
        /// Spreadsheet connected to the GUI
        /// </summary>
		Spreadsheet Frame1;

        /// <summary>
        /// String to hold the current cell content
        /// </summary>
		string currCell;

        /// <summary>
        /// New model for processing
        /// </summary>
        private ClientModel model;

		/// <summary>
		/// Initializes a new form, or spreadsheet window.
		/// Also creates a new spreadsheet and sets the default startup values.
		/// </summary>
		public SS_GUI_Form(ClientModel currModel, string spName)
		{
            // Set initial state of the GUI -- start the form
			InitializeComponent();
			Frame1 = new Spreadsheet(s => true, s => s.ToUpper(), spName);
			spreadsheetPanel1.SelectionChanged += displaySelection;
			spreadsheetPanel1.SetSelection(0, 0);
			currCell = "A1";
			this.Text = spName;
			AcceptButton = Set_Content_Button;
			WindowState = FormWindowState.Maximized;

            // Process model events
            model = currModel;
            model.IncomingCellUpdateEvent += CellUpdateCommand;
            model.IncomingGenericErrorEvent += GenericErrorReceived;
            model.IncomingCellUpdateErrorEvent += CellErrorCommand;
            model.ConnectionLostErrorEvent += ConnectionErrorReceived;
            model.InvalidCommandEvent += InvalidCommandReceived;
            model.InvalidStateErrorEvent += InvalidStateRecieved;
		}

		/// <summary>
		/// Displayes the newly selected cell's information in the text boxes.
		/// </summary>
		/// <param name="ss"></param>
		private void displaySelection(SpreadsheetPanel ss)
		{
			int row, col;
			ss.GetSelection(out col, out row);

			char c = (char)(col + 65);
			currCell = "" + c + (row + 1);
			Cell_Name_Display.Text = currCell;
			Cell_Content_Display.Text = Frame1.GetCellContents(currCell).ToString();
            Cell_Value_Display.Text = Frame1.GetCellValue(currCell).ToString();
		}

		/// <summary>
		/// Updates all of the dependent cells in the spreadsheet when a value is updated.
		/// </summary>
		/// <param name="cells"></param>
		private void UpdateSpreadsheetCells(IEnumerable<string> cells)
		{
			foreach (string s in cells)
			{
				double cellR = 0;
				int cellRow = 0;
				char cellC = (char)(s.ElementAt(0));
				int cellCol = cellC - 65;
				// If the numerical digit is single, just parse the single character.
				if (s.Length < 3)
					cellRow = s.ElementAt(s.Length - 1) - 49;
				// Else combine the two last digits and parse into a double for row number.
				else
				{
					string t = "";
					t += s.ElementAt(s.Length - 2);
					t += s.ElementAt(s.Length - 1);
					Double.TryParse(t, out cellR);
					cellRow = (int)(cellR - 1);

				}
                spreadsheetPanel1.SetValue(cellCol, cellRow, Frame1.GetCellValue(s).ToString());
			}
		}

		/// <summary>
		/// Updates the text boxes to match the cell being selected.
		/// </summary>
		private void UpdateCurrCellTextBoxes()
		{
			this.Invoke((MethodInvoker)delegate
			{
                    Cell_Content_Display.Text = Frame1.GetCellContents(currCell).ToString();
                    Cell_Value_Display.Text = Frame1.GetCellValue(currCell).ToString();
                    Cell_Name_Display.Text = currCell;
			});
		}

		/// <summary>
		/// Allows closing of the window through the file menu.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Sends cell update to server, but does not update the actual cell!
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Set_Content_Button_Click(object sender, EventArgs e)
		{
			//Update_Cells();

            // Convert current row and col to cell name to send to server
            int row, col;
            spreadsheetPanel1.GetSelection(out col, out row);
			row++;
            char cellCol = (char) (col + 65);
            string cell = cellCol + row.ToString();

            string cellContent = Cell_Content_Display.Text;

            Boolean okToSend = true;

			if(cellContent != "")
				if (cellContent[0] == '=')
				{
					if (!Frame1.isValidFormula(cellContent.Substring(1)))
					{
						MessageBox.Show("Invalid Formula Entered.");
						okToSend = false;
					}
				}

            if (okToSend)
            {
                // Create command to send to server
                string message = "";
                message += "cell " + cell + " " + cellContent + "\n";

                // Send change to server
                model.SendMessage(message);
            }
		}

		/// <summary>
		/// Shows the help dialog box.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void showHelpF1ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			HelpBox();
		}
	
		/// <summary>
		/// Provides text for the help dialog box.
		/// </summary>
		private void HelpBox()
		{
			MessageBox.Show("The mouse can be used to navigate to different cells and select the cotent box.\n\n"
			+ "The content box can be edited for any given cell.\n\nPressing \"Enter\" or clicking the \"Set Contents\" button will set the cell contents to the string, double or formula provided in the \"Cell Contents\" box. Formulas are designated with an \"=\" sign at the begining."
			+ "\n\nMy additional feature was the addition of a calculator, which can be found by clicking the \"Calculator\" menu next to the file menu. The calculator can take input from the buttons, or from your keyboard!", "Help");
		}
 
		/// <summary>
		/// Runs a calculator form when the menu button is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void showCalculatorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Form2 f2 = new Form2();
			f2.Show();
		}

		/// <summary>
		/// Sends a command to server to undo last operation
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="keyData"></param>
		/// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Z))
            {
                model.SendMessage("undo\n");
                return true;
            }

            return false;
        }

		/// <summary>
		/// Called when we receive an update command from the server
		/// </summary>
		/// <param name="cmd"></param>
        private void CellUpdateCommand(string cmd)
        {
            // Ints to hold row and col number
            int row, col;

            // Split the command by spaces
            char[] delimiterChars = { ' '};
            string[] words = cmd.Split(delimiterChars);

            // Get cell name from command and convert to row and col numbers
            string cellName = words[1];
            char colChar = cellName[0];
            col = (int)colChar - 65;
            row = Convert.ToInt32(cellName.Substring(1)) - 1;

            // Get the contents of the cell from the command
			string newContents = "";
			for (int i = 2; i < words.Length; i++)
			{
				newContents += words[i];
				if(i < words.Length -1)
					newContents += " ";
			}

            // Updates cells
            IEnumerable<string> recalc = null;
            recalc = Frame1.SetContentsOfCell(cellName, newContents);
            UpdateSpreadsheetCells(recalc);
            spreadsheetPanel1.SetValue(col, row, Frame1.GetCellValue(cellName).ToString());
            UpdateCurrCellTextBoxes();
        }

        /// <summary>
        /// Show error message when an invalid cell update is requested
        /// </summary>
        /// <param name="obj"></param>
        private void CellErrorCommand(string obj)
        {
            MessageBox.Show("ERROR: \n\n" + obj);
        }

        /// <summary>
        /// Show error message when connection is lost
        /// </summary>
        /// <param name="obj"></param>
        private void ConnectionErrorReceived(string obj)
        {
            MessageBox.Show(obj);
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
        /// Show error message when invalid command was received by server
        /// </summary>
        /// <param name="obj"></param>
        private void InvalidCommandReceived(string obj)
        {
            MessageBox.Show("ERROR:\nInvalid command received: " + obj);
        }

        /// <summary>
        /// Show error message when any other generic error is sent by server
        /// </summary>
        /// <param name="obj"></param>
        private void GenericErrorReceived(string obj)
        {
            MessageBox.Show("ERROR: \n\n" + obj);
        }

	}

}
