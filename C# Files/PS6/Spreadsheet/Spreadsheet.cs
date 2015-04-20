// Written by Joe Zachary for CS 3500, September 2012
// Version 1.7
// Revision history:  
//   Version 1.1 9/20/12 12:59 p.m.  Fixed comment that describes circular dependencies
//   Version 1.2 9/20/12 1:38 p.m.   Changed return type of GetCellContents to object
//   Version 1.3 9/24/12 8:41 a.m.   Modified the specification of GetCellsToRecalculate by 
//                                   adding a requirement for the names parameter
// Branched from PS4Skeleton
//   Version 1.4                     Branched from PS4Skeleton
//           Edited class comment for AbstractSpreadsheet
//           Made the three SetCellContents methods protected
//           Added a new method SetContentsOfCell.  This method abstract.
//           Added a new method GetCellValue.  This method is abstract.
//           Added a new property Changed.  This property is abstract.
//           Added a new method Save.  This method is abstract.
//           Added a new method GetSavedVersion.  This method is abstract.
//           Added a new class SpreadsheetReadWriteException.
//           Added IsValid, Normalize, and Version properties
//           Added a constructor for AbstractSpreadsheet

// Revision history:
//    Version 1.5 9/28/12 2:22 p.m.   Fixed example in comment for Save
//    Version 1.6 9/29/12 11:07 a.m.  Put a constructor into SpreadsheetReadWriteException
//    Version 1.7 9/29/12 11:14 a.m.  Added missing </summary> tag to comment

// Implemented by Drew McClelland
// u0634481
// PS5
// CS 3500-F14
// 10/15/2014


using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SpreadsheetUtilities;
using System.Xml;

namespace SS
{



	// PARAGRAPHS 2 and 3 modified for PS5.
	/// <summary>
	/// An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
	/// spreadsheet consists of an infinite number of named cells.
	/// 
	/// A string is a cell name if and only if it consists of one or more letters,
	/// followed by one or more digits AND it satisfies the predicate IsValid.
	/// For example, "A15", "a15", "XY032", and "BC7" are cell names so long as they
	/// satisfy IsValid.  On the other hand, "Z", "X_", and "hello" are not cell names,
	/// regardless of IsValid.
	/// 
	/// Any valid incoming cell name, whether passed as a parameter or embedded in a formula,
	/// must be normalized with the Normalize method before it is used by or saved in 
	/// this spreadsheet.  For example, if Normalize is s => s.ToUpper(), then
	/// the Formula "x3+a5" should be converted to "X3+A5" before use.
	/// 
	/// A spreadsheet contains a cell corresponding to every possible cell name.  
	/// In addition to a name, each cell has a contents and a value.  The distinction is
	/// important.
	/// 
	/// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
	/// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
	/// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
	/// 
	/// In a new spreadsheet, the contents of every cell is the empty string.
	///  
	/// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
	/// (By analogy, the value of an Excel cell is what is displayed in that cell's position
	/// in the grid.)
	/// 
	/// If a cell's contents is a string, its value is that string.
	/// 
	/// If a cell's contents is a double, its value is that double.
	/// 
	/// If a cell's contents is a Formula, its value is either a double or a FormulaError,
	/// as reported by the Evaluate method of the Formula class.  The value of a Formula,
	/// of course, can depend on the values of variables.  The value of a variable is the 
	/// value of the spreadsheet cell it names (if that cell's value is a double) or 
	/// is undefined (otherwise).
	/// 
	/// Spreadsheets are never allowed to contain a combination of Formulas that establish
	/// a circular dependency.  A circular dependency exists when a cell depends on itself.
	/// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
	/// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
	/// dependency.
	/// </summary>
	public class Spreadsheet : AbstractSpreadsheet
	{
		// A dictionary mapping cell names to cell objects (which contain name, content, and value).
		private Dictionary<string, Cell> cells;
		// Dependency Graph representing the relationship of cells and their dependency on one-another.
		private DependencyGraph dependencies;
		// A boolean value representing if the spreadsheet has changed after original construction or after performing a save.
		private bool hasChanged;





		/// <summary>
		/// Zero argument constructor for a spreadsheet.
		/// This constructor uses a Validator that is true if the variable satisfies the standard variable name guidelines (one or more letters followed by one or more numbers).
		/// This constructor uses a identity normalizer (The normalization of any variable will be itself).
		/// This constructor sets the version to be "default".
		/// </summary>
		public Spreadsheet()
			: base(s => true, s => s, "default")
		{
			cells = new Dictionary<string, Cell>();
			dependencies = new DependencyGraph();
			hasChanged = false;
		}



		/// <summary>
		/// Provides a three argument constructor for a spreadsheet.
		/// This constructor will use the provided Validator and Normalizer, and will set the version to be the provided version string.
		/// </summary>
		public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version)
			: base(isValid, normalize, version)
		{
			cells = new Dictionary<string, Cell>();
			dependencies = new DependencyGraph();
			hasChanged = false;
		}



		/// <summary>
		/// Provides a four argument constructor for a spreadsheet.
		/// This constructor will use the provided Validator and Normalizer, and will set the version to be the provided version string.
		/// This constructor will also construct cells based on an XMl spreadsheet representation of a spreadsheet.
		/// If the version passed into the constructor does not match the version in the XML file, an exception is thrown.
		/// </summary>
		public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version)
			: base(isValid, normalize, version)
		{
			cells = new Dictionary<string, Cell>();
			dependencies = new DependencyGraph();
			if (GetSavedVersion(filePath) != version)
				throw new SpreadsheetReadWriteException("The saved version of the XML file does not match the version provided to the constructor.");

			try
			{
				using (XmlTextReader xmlReader = new XmlTextReader(filePath))
				{
					//}
					//catch (Exception)
					//{
					//	xmlReader.Dispose();
					//	throw new SpreadsheetReadWriteException("File could not be opened using XmlTextReader");
					//}
					// Reads cells from XML spreadsheet.
					// TODO FIX THIS!
					while (xmlReader.Read())
					{
						string name = "";
						string contents = "";
						if (xmlReader.ReadToFollowing("name"))
						{
							name = xmlReader.ReadElementContentAsString();

							if (!xmlReader.ReadToFollowing("contents"))
								throw new SpreadsheetReadWriteException("No contents found associated with name: " + name);
							contents = xmlReader.ReadElementContentAsString();


							this.SetContentsOfCell(name, contents);
						}
					}
				}
			}
			catch (Exception e)
			{
				throw new SpreadsheetReadWriteException("Could not use file. - " + e.Message);
			}
			IsValid = isValid;
			Normalize = normalize;
			hasChanged = false;
		}



		// TODO : ADDED FOR PS5
		/// <summary>
		/// True if this spreadsheet has been modified since it was created or saved                  
		/// (whichever happened most recently); false otherwise.
		/// </summary>
		public override bool Changed
		{
			get
			{
				return hasChanged;
			}
			protected set
			{
				hasChanged = value;
			}
		}



		// TODO : ADDED FOR PS5
		/// <summary>
		/// Returns the version information of the spreadsheet saved in the named file.
		/// If there are any problems opening, reading, or closing the file, the method
		/// should throw a SpreadsheetReadWriteException with an explanatory message.
		/// </summary>
		public override string GetSavedVersion(string filename)
		{
			string vers = "";
			//XmlTextReader xmlReader = null;
			try
			{
				using (XmlTextReader xmlReader = new XmlTextReader(filename))
				{

					//}
					//catch (Exception)
					//{
					//	xmlReader.Dispose();
					//	throw new SpreadsheetReadWriteException("File could not be opened using XmlTextReader");
					//}
					//try
					//{
					// Attempts to read the spreadsheet version from the XML file.
					if (!xmlReader.ReadToFollowing("spreadsheet") || !xmlReader.MoveToFirstAttribute())
						throw new SpreadsheetReadWriteException("Spreadsheet version could not be found");
					//}
					//catch (Exception)
					//{
					//	throw new SpreadsheetReadWriteException("Spreadsheet version could not be found.");
					//}
					vers = xmlReader.Value;
					// Attemtps to close the XML reader.
					//try
					//{
					//	xmlReader.Close();
					//}
					//catch (Exception)
					//{
					//	xmlReader.Dispose();
					//	throw new SpreadsheetReadWriteException("File could not be closed.");
					//}
				}
			}
			catch (Exception e)
			{
				throw new SpreadsheetReadWriteException("File version could not be read. - " + e.Message);
			}
			return vers;
		}



		// TODO : ADDED FOR PS5
		/// <summary>
		/// Writes the contents of this spreadsheet to the named file using an XML format.
		/// The XML elements should be structured as follows:
		/// 
		/// <spreadsheet version="version information goes here">
		/// 
		/// <cell>
		/// <name>
		/// cell name goes here
		/// </name>
		/// <contents>
		/// cell contents goes here
		/// </contents>    
		/// </cell>
		/// 
		/// </spreadsheet>
		/// 
		/// There should be one cell element for each non-empty cell in the spreadsheet.  
		/// If the cell contains a string, it should be written as the contents.  
		/// If the cell contains a double d, d.ToString() should be written as the contents.  
		/// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
		/// 
		/// If there are any problems opening, writing, or closing the file, the method should throw a
		/// SpreadsheetReadWriteException with an explanatory message.
		/// </summary>
		public override void Save(string filename)
		{
			//XmlWriter x = null;
			// Attempts to create an XML writer with certain settings.
			//try
			//{

			XmlWriterSettings settings = new XmlWriterSettings();
			settings.NewLineChars = "\n";
			settings.IndentChars = "";
			settings.Indent = true;
			try
			{
				using (XmlWriter x = XmlWriter.Create(filename, settings))
				{
					//}
					//catch (Exception e)
					//{
					//if (x != null)
					//	x.Dispose();
					//throw new SpreadsheetReadWriteException("Could not open file: " + e.Message);
					//}
					// Attempts to save each cell in XML format.
					//try
					//{

					x.WriteStartDocument();
					x.WriteStartElement("spreadsheet");
					x.WriteAttributeString("version", Version);
					foreach (Cell c in cells.Values)
					{
						x.WriteStartElement("cell");
						x.WriteStartElement("name");
						x.WriteString(c.Name);
						x.WriteEndElement();
						x.WriteStartElement("contents");
						if (c.Content is Formula)
							x.WriteString("=" + c.Content.ToString());
						else
							x.WriteString(c.Content.ToString());
						x.WriteEndElement();
						x.WriteEndElement();
					}
					x.WriteEndElement();
					x.WriteEndDocument();

					//}
					//catch (Exception)
					//{
					//	x.Dispose();
					//	throw new SpreadsheetReadWriteException("Could not write file.");
					//}
					//// Attempts to close the Spreadsheet Writer.
					//try
					//{
					//	x.Close();
					//}
					//catch (Exception)
					//{
					//	x.Dispose();
					//	throw new SpreadsheetReadWriteException("Could not close file.");
					//}
				}
			}
			catch (Exception e)
			{
				throw new SpreadsheetReadWriteException("Could not use file. - " + e.Message);
			}



			Changed = false;
		}



		// ADDED FOR PS5
		/// <summary>
		/// If name is null or invalid, throws an InvalidNameException.
		/// 
		/// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
		/// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
		/// </summary>
		public override object GetCellValue(string name)
		{
			name = Normalize(name);
			CheckValidCellName(name);
			Cell c = new Cell();
			if (cells.TryGetValue(name, out c))
			{
				if (c.Value is double)
					return (double)c.Value;
				else if (c.Value is FormulaError)
					return (FormulaError)c.Value;
				else return (string)c.Value;
			}
			return "";
		}



		/// <summary>
		/// Enumerates the names of all the non-empty cells in the spreadsheet.
		/// </summary>
		public override IEnumerable<string> GetNamesOfAllNonemptyCells()
		{
			return cells.Keys;
		}



		/// <summary>
		/// If name is null or invalid, throws an InvalidNameException.
		/// 
		/// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
		/// value should be either a string, a double, or a Formula.
		/// </summary>
		public override object GetCellContents(string name)
		{
			name = Normalize(name);
			CheckValidCellName(name);

			Cell getContent = new Cell();
			if (cells.TryGetValue(name, out getContent))
			{
				return getContent.Content;
			}

			return "";
		}



		// TODO : ADDED FOR PS5
		/// <summary>
		/// If content is null, throws an ArgumentNullException.
		/// 
		/// Otherwise, if name is null or invalid, throws an InvalidNameException.
		/// 
		/// Otherwise, if content parses as a double, the contents of the named
		/// cell becomes that double.
		/// 
		/// Otherwise, if content begins with the character '=', an attempt is made
		/// to parse the remainder of content into a Formula f using the Formula
		/// constructor.  There are then three possibilities:
		/// 
		///   (1) If the remainder of content cannot be parsed into a Formula, a 
		///       SpreadsheetUtilities.FormulaFormatException is thrown.
		///       
		///   (2) Otherwise, if changing the contents of the named cell to be f
		///       would cause a circular dependency, a CircularException is thrown.
		///       
		///   (3) Otherwise, the contents of the named cell becomes f.
		/// 
		/// Otherwise, the contents of the named cell becomes content.
		/// 
		/// If an exception is not thrown, the method returns a set consisting of
		/// name plus the names of all other cells whose value depends, directly
		/// or indirectly, on the named cell.
		/// 
		/// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
		/// set {A1, B1, C1} is returned.
		/// </summary>
		public override ISet<string> SetContentsOfCell(string name, string content)
		{
			if (content == null)
				throw new ArgumentNullException();
			if (name == null)
				throw new InvalidNameException();
			name = Normalize(name);
			CheckValidCellName(name);
			HashSet<string> recalculate = new HashSet<string>();
			double @double = 0;
			// Attempts to parse the content as a double.
			if (Double.TryParse(content, out @double))
				recalculate = new HashSet<string>(SetCellContents(name, @double));
			// Attempts to parse the content as a formula.
			else if (content != "" && content.ElementAt(0) == '=')
			{
				content = content.Substring(1);
				Formula formula = new Formula(content, Normalize, IsValid);
				recalculate = new HashSet<string>(SetCellContents(name, formula));
			}
			// If the content was not a formula or a double, the contents are kept as a string.
			else
				recalculate = new HashSet<string>(SetCellContents(name, content));

			// Recalculates all of the cells that were changed due to the modificaiton of the provided cell.
			EvaluateCells(recalculate);
			return recalculate;
		}



		// MODIFIED PROTECTION FOR PS5
		/// <summary>
		/// If name is null or invalid, throws an InvalidNameException.
		/// 
		/// Otherwise, the contents of the named cell becomes number.  The method returns a
		/// set consisting of name plus the names of all other cells whose value depends, 
		/// directly or indirectly, on the named cell.
		/// 
		/// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
		/// set {A1, B1, C1} is returned.
		/// </summary>
		protected override ISet<string> SetCellContents(string name, double number)
		{
			CheckValidCellName(name);
			Cell outCell = new Cell();
			if (cells.TryGetValue(name, out outCell))
			{
				cells[name].Content = number;
				dependencies.ReplaceDependees(name, new List<string>());
			}

			else
			{
				cells.Add(name, new Cell(name, number, number));
			}

			Changed = true;

			return new HashSet<string>(GetCellsToRecalculate(name));
		}



		// MODIFIED PROTECTION FOR PS5
		/// <summary>
		/// If text is null, throws an ArgumentNullException.
		/// 
		/// Otherwise, if name is null or invalid, throws an InvalidNameException.
		/// 
		/// Otherwise, the contents of the named cell becomes text.  The method returns a
		/// set consisting of name plus the names of all other cells whose value depends, 
		/// directly or indirectly, on the named cell.
		/// 
		/// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
		/// set {A1, B1, C1} is returned.
		/// </summary>
		protected override ISet<string> SetCellContents(string name, string text)
		{
			if (text == null)
				throw new ArgumentNullException();
			CheckValidCellName(name);
			if (text == "")
			{
				HashSet<string> recalculate = new HashSet<string>(GetCellsToRecalculate(name));
				dependencies.ReplaceDependees(name, new List<string>());
				cells.Remove(name);
				return recalculate;

			}
			Cell outCell = new Cell();
			if (cells.TryGetValue(name, out outCell))
			{
				cells[name].Content = text;
				dependencies.ReplaceDependees(name, new List<string>());
			}
			else
			{
				cells.Add(name, new Cell(name, text, text));
			}

			Changed = true;

			return new HashSet<string>(GetCellsToRecalculate(name));
		}



		// MODIFIED PROTECTION FOR PS5
		/// <summary>
		/// If formula parameter is null, throws an ArgumentNullException.
		/// 
		/// Otherwise, if name is null or invalid, throws an InvalidNameException.
		/// 
		/// Otherwise, if changing the contents of the named cell to be the formula would cause a 
		/// circular dependency, throws a CircularException.
		/// 
		/// Otherwise, the contents of the named cell becomes formula.  The method returns a
		/// Set consisting of name plus the names of all other cells whose value depends,
		/// directly or indirectly, on the named cell.
		/// 
		/// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
		/// set {A1, B1, C1} is returned.
		/// </summary>
		protected override ISet<string> SetCellContents(string name, Formula formula)
		{
			if (formula == null)
				throw new ArgumentException();
			CheckValidCellName(name);

			bool originalChangedState = Changed;
			List<string> origDeps = dependencies.GetDependees(name).ToList();
			Dictionary<string, Cell> origCells = new Dictionary<string, Cell>();
			for (int i = 0; i < cells.Count; i++)
			{
				origCells.Add(cells.ElementAt(i).Key, cells.ElementAt(i).Value.Clone());
			}

			Cell getCell = new Cell();
			if (cells.TryGetValue(name, out getCell))
			{
				cells[name].Content = formula;
				cells[name].Value = null;
			}
			else
			{
				cells.Add(name, new Cell(name, formula, null));
			}
			dependencies.ReplaceDependees(name, formula.GetVariables());
			Changed = true;
			return new HashSet<string>(GetCellsToRecalculate(name));
		}



		/// <summary>
		/// If name is null, throws an ArgumentNullException.
		/// 
		/// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
		/// 
		/// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
		/// values depend directly on the value of the named cell.  In other words, returns
		/// an enumeration, without duplicates, of the names of all cells that contain
		/// formulas containing name.
		/// 
		/// For example, suppose that
		/// A1 contains 3
		/// B1 contains the formula A1 * A1
		/// C1 contains the formula B1 + A1
		/// D1 contains the formula B1 - C1
		/// The direct dependents of A1 are B1 and C1
		/// </summary>
		protected override IEnumerable<string> GetDirectDependents(string name)
		{

			if (name == null)
				throw new ArgumentNullException();
			name = Normalize(name);
			CheckValidCellName(name);

			return dependencies.GetDependents(name);													// GET DEPENDENTS
		}



		/// <summary>
		/// Determines if a cell name is valid.
		/// A valid cell name consists of one or more letters followed by one or more numbers.
		/// </summary>
		/// <param name="cellName">The name of the cell to be validated.</param>
		private void CheckValidCellName(string cellName)
		{
			if (cellName == null)
				throw new InvalidNameException();
			string varCheckLetters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

			string varCheckNumbers = "0123456789";
			char[] chars = cellName.ToCharArray();
			int j = 0;
			while (j < chars.Count() && varCheckLetters.Contains(chars[j]))
				j++;
			if (j == chars.Count() || j == 0)
				throw new InvalidNameException();
			while (j < chars.Count())
			{
				if (!varCheckNumbers.Contains(chars[j]))
					throw new InvalidNameException();
				j++;
			}
			if (!IsValid(cellName))
				throw new InvalidNameException();
		}


		/// <summary>
		/// Evaluates a set of cells in the order they appear.
		/// </summary>
		/// <param name="cellNames">Set of cells to be evaluated.</param>
		private void EvaluateCells(HashSet<string> cellNames)
		{
			foreach (string name in cellNames)
			{
				Cell outCell = new Cell();
				if (!cells.TryGetValue(name, out outCell))
				{
					continue;
				}
				if (outCell.Content is string || outCell.Content is double)
				{
					outCell.Value = outCell.Content;
					continue;
				}
				Formula formula = (Formula)outCell.Content;
                try
                {
                    outCell.Value = formula.Evaluate(GetValue);
                }
				catch (ArgumentException)
                {
                    outCell.Value = outCell.Content;
                }
			}

		}

		/// <summary>
		/// This provides a function to be used as a lookup function for the evaluator.
		/// </summary>
		/// <param name="s">Returns a value associated with a cell</param>
		/// <returns></returns>
		private double GetValue(string s)
		{
			Cell c = new Cell();
            if (!cells.TryGetValue(s, out c))
                throw new ArgumentException();
			if (!(c.Value is double))
				throw new ArgumentException();
			return (double)c.Value;
		}

        public Boolean isValidFormula(string s)
        {
            try
            {
                Formula f = new Formula(s);
                return true;
            }
            catch (FormulaFormatException)
            {
                return false;
            }
        }
	}
}

