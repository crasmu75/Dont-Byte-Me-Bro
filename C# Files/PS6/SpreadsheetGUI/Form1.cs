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

namespace SpreadsheetGUI
{
	public partial class Form1 : Form
	{
		Spreadsheet Frame1;
		string currCell;
		string lastFileName;

        // Added to connect to server

        /// <summary>
        /// New model for processing
        /// </summary>
        private ClientModel model;

        /// <summary>
        /// Hardcoded port number for connection
        /// </summary>
        private int portno = 2113;


		/// <summary>
		/// Initializes a new form, or spreadsheet window.
		/// Also creates a new spreadsheet and sets the default startup values.
		/// </summary>
		public Form1()
		{
			InitializeComponent();
			Frame1 = new Spreadsheet(s => true, s => s.ToUpper(), "ps6");
			spreadsheetPanel1.SelectionChanged += displaySelection;
			spreadsheetPanel1.SetSelection(0, 0);
			currCell = "A1";
			lastFileName = null;
			this.Text = "New Spreadsheet";
			AcceptButton = Set_Content_Button;
			WindowState = FormWindowState.Maximized;

            // Added to connect to server
            model = new ClientModel();
            model.IncomingLineEvent += MessageReceived;
            model.IncomingCellUpdateEvent += CellUpdateCommand;
            model.IncomingErrorEvent += ErrorCommand;
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
			Cell_Content_Display.Text = Frame1.GetCellContents(currCell).ToString();
			Cell_Value_Display.Text = Frame1.GetCellValue(currCell).ToString();
			Cell_Name_Display.Text = currCell;
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
		/// Provides a save function in the file menu. This will use the last used filename or ask for a new one.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/*private void saveCtrlToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (lastFileName == null)
			{
				SaveAsDocument();
			}
			else
			{
				Frame1.Save(lastFileName);
			}
		}*/



		/// <summary>
		/// Updates the cells when the set content button is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Set_Content_Button_Click(object sender, EventArgs e)
		{
			//Update_Cells();

            // Convert current row and col to cell name to send to server
            int row, col;
            spreadsheetPanel1.GetSelection(out col, out row);
            char cellCol = (char) (col + 65);
            string cell = cellCol + row.ToString();

            string cellContent = Cell_Content_Display.Text;

            // Create command to send to server
            string message = "";
            message += "cell " + cell + " " + cellContent + "\n";

            // Send change to server
            model.SendMessage(message);

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
			+ "\n\nMy additional feature was the addition of a calculator, which can be found by clicking the \"Calculator\" menu next to the file menu. The calculator can take input from the buttons, or from your keyboard!"
			+"\n\nI also had a lot of trouble getting Coded UI tests to work. I made a few really long ones only to realize that they don't work. For some reason the test doesn't record"
			+" some of the key presses and doesn't correctly update the cells. I Eventually deleted all the tests because I couldn't get them to pass, even though I proved they should pass by manually doing each command.", "Help");
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

        private void HostName_TextChanged(object sender, EventArgs e)
        {

        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            model.Connect(HostName.Text, portno, "user1", "sheet1");
        }

        private void MessageReceived(String line)
        {
            
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Y))
            {
                model.SendMessage("undo\n");
                return true;
            }

            return false;
        }

        private void CellUpdateCommand(string cmd)
        {
            int row, col;

            char[] delimiterChars = { ' ', '\n'};
            string[] words = cmd.Split(delimiterChars);

            string cellName = words[1];
            char colChar = cellName[0];
            col = (int)colChar - 65;
            row = Convert.ToInt32(cellName.Substring(1));

            spreadsheetPanel1.SetValue(col, row, words[2]);
            UpdateCurrCellTextBoxes();
        }

        private void ErrorCommand(string obj)
        {
            MessageBox.Show(obj);
        }

    // UNUSED METHODS ----------------------------------------------------------------

        /// <summary>
        /// Sets the value of a a cell to be what is entered in the content display box, and updates the other cells in the table.
        /// </summary>
        private void Update_Cells()
        {
            // Updates cells and catches circular exception to display error.
            IEnumerable<string> recalc = null;
            try
            {
                recalc = Frame1.SetContentsOfCell(currCell, Cell_Content_Display.Text);
            }
            catch (CircularException)
            {
                MessageBox.Show("The formula you entered caused a circular exception. Cells must not have a circular dependency.");
                return;
            }
            int row, col;
            spreadsheetPanel1.GetSelection(out col, out row);
            UpdateSpreadsheetCells(recalc);
            spreadsheetPanel1.SetValue(col, row, Frame1.GetCellValue(currCell).ToString());
            UpdateCurrCellTextBoxes();

        }

        /// <summary>
        /// Shows the open file dialog and requests for saving before opening the file.
        /// Also updates all of the cells in the spreadsheet to the opened spreadsheet file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileMenu_Click(object sender, EventArgs e)
        {
            // Asks user to save before opening (and replacing) current spreadsheet.
            if (Frame1.Changed == true)
            {
                DialogResult x = MessageBox.Show("Opening a new spreadsheet will result in loss of unsaved data. Would you like to save the spreadsheet before closing?", "Open Confirmation", MessageBoxButtons.YesNoCancel);
                if (x == DialogResult.Yes)
                {
                    SaveAsDocument();
                }
                else if (x == DialogResult.Cancel)
                {
                    return;
                }
            }
            string fileName = OpenSpreadsheetDialog();

            if (fileName == null)
                return;

            // Opens new spreadsheet and updates all of the cells.
            lastFileName = fileName;
            this.Text = lastFileName;
            Frame1 = new Spreadsheet(fileName, s => true, s => s.ToUpper(), "ps6");
            IEnumerable<string> cells = Frame1.GetNamesOfAllNonemptyCells();
            ClearAllCells();

            UpdateSpreadsheetCells(cells);

            spreadsheetPanel1.SetSelection(0, 0);
            currCell = "A1";
            UpdateCurrCellTextBoxes();
        }

        /// <summary>
        /// Clears all of the cells to be empty before opening a new spreadsheet.
        /// </summary>
        private void ClearAllCells()
        {
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    spreadsheetPanel1.SetValue(i, j, "");
                }
            }
        }

        /// <summary>
        /// Provdes the open dialog box and information.
        /// Also checks for .sprd extension and appends if necessary.
        /// </summary>
        /// <returns></returns>
        private string OpenSpreadsheetDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Spreadsheet Files|*.sprd|All Files|*.*";
            dialog.ShowDialog();
            if (dialog.FilterIndex == 1)
            {
                Regex r = new Regex("(.sprd)");
                if (!r.IsMatch(dialog.FileName))
                {
                    MessageBox.Show("The file you entered is not a file with the spreadsheet (.sprd) extension.\n" +
                     "Please select a file with a .sprd extension, or change the filter to \"All Files\"");
                    return null;
                }
            }

            return dialog.FileName;
        }

        /// <summary>
        /// Provides a close all function in the file menu and makes sure the user wants to exit all windows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult x = MessageBox.Show("Closing all of the spreadsheets may result in loss of data. "
            + "If you want to save a spreadsheet, please save or close it individually.\n\nAre you sure you want close all windows without saving?",
            "Close All Windows", MessageBoxButtons.YesNoCancel);

            if (x == DialogResult.No)
            {
                return;
            }
            else if (x == DialogResult.Yes)
            {
                Environment.Exit(0);
            }
            else if (x == DialogResult.Cancel)
            {
                return;
            }
        }

        /// <summary>
        /// Provides a button to run a new instance of this form to provide a new window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same
            // thread as the other forms.
            DemoApplicationContext.getAppContext().RunForm(new Form1());
        }

        /// <summary>
        /// Provides a save function in the file menu. This will use the last used filename or ask for a new one.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveCtrlSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAsDocument();
        }

        /// <summary>
        /// Provides a save as function in the file menu. Automatically appends .sprd if needed.
        /// </summary>
        private void SaveAsDocument()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Spreadsheet Files|*.sprd|All Files|*.*";
            dialog.ShowDialog();
            string filename = dialog.FileName;
            // Matches and appends .sprd extension.
            if (dialog.FilterIndex == 1)
            {
                Regex r = new Regex("(.sprd)");
                if (!r.IsMatch(filename))
                {
                    filename += ".sprd";
                }
            }
            lastFileName = filename;
            this.Text = lastFileName;
            Frame1.Save(filename);
        }

        /// <summary>
        /// When the user attempts to close a form, a dialog box asks if the user would like to save the spreadsheet if it has been changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           if (Frame1.Changed == true)
            {
                DialogResult x = MessageBox.Show("Closing the spreadsheet will result in loss of unsaved data. Would you like to save the spreadsheet before closing?", "Close Confirmation", MessageBoxButtons.YesNoCancel);
                if (x == DialogResult.Yes)
                {
                    SaveAsDocument();
                }
                else if (x == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
	}

}
