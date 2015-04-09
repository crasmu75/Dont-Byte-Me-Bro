// Tests written for Spreadsheet PS5
// Drew McClelland
// u0634481
// PS5
// 10/15/2014

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using SpreadsheetUtilities;

namespace SpreadsheetTesterPS5
{
	/// <summary>
	/// Provides a testing class for the PS5 spreadsheet.
	/// Mostly focuses on Validator, Normalizer and getting values.
	/// </summary>
	[TestClass]
	public class PS5Tester
	{
		/// <summary>
		/// Tests the spreadsheet isValid function for always false delegate.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void InvalidName1()
		{
			Spreadsheet sheet = new Spreadsheet(s => false, s => s, "default");
			sheet.SetContentsOfCell("D1", "blue");
		}

		/// <summary>
		/// Tests the spreadsheet isValid function for a specific cell name.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void InvalidName2()
		{
			Spreadsheet sheet = new Spreadsheet(s => {if (s == "D1") return true; return false;}, s => s, "default");
			sheet.SetContentsOfCell("B1", "blue");
		}

		/// <summary>
		/// Tests the spreadsheet valid name testing having an underscore int he begining.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void InvalidName3()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("D_1", "blue");
		}

		/// <summary>
		/// Tests the spreadsheet valid name testing ending with an underscore.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void InvalidName4()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("D1_", "blue");
		}

		/// <summary>
		/// Tests the spreadsheet valid name testing starting with an underscore.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void InvalidName5()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("_D1", "blue");
		}

		/// <summary>
		/// Tests the spreadsheet valid name testing ending and beginning with a letter.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void InvalidName6()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("D1a", "blue");
		}

		/// <summary>
		/// Tests the spreadsheet valid name testing ending and begining with a letter.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void InvalidName7()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("D111A", "blue");
		}

		/// <summary>
		/// Tests the spreadsheet valid name testing without a number.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void InvalidName8()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("DDDDA", "blue");
		}

		/// <summary>
		/// Tests the spreadsheet valid name testing begining with a number.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void InvalidName9()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("1D", "blue");
		}

		/// <summary>
		/// Tests the spreadsheet valid name testing for a number.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void InvalidName10()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("1", "blue");
		}

		/// <summary>
		/// Tests the spreadsheet valid name for capitalization.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void InvalidName11()
		{
			Spreadsheet sheet = new Spreadsheet(s => { if (s == "D1") return true; return false; }, s => s, "default");
			sheet.SetContentsOfCell("d1", "blue");
		}

		/// <summary>
		/// Tests the spreadsheet valid name testing with a specific validator.
		/// </summary>
		[TestMethod]
		public void ValidName1()
		{
			Spreadsheet sheet = new Spreadsheet(s => { if (s == "D1") return true; return false; }, s => s, "default");
			sheet.SetContentsOfCell("D1", "blue");
			Assert.AreEqual(sheet.GetCellContents("D1"), "blue");
		}

		/// <summary>
		/// Tests the spreadsheet valid name testing with a specific validator and normalizer.
		/// </summary>
		[TestMethod]
		public void ValidName2()
		{
			Spreadsheet sheet = new Spreadsheet(s => { if (s == "D1") return true; return false; }, s => s.ToUpper(), "default");
			sheet.SetContentsOfCell("d1", "blue");
			Assert.AreEqual(sheet.GetCellContents("D1"), "blue");
		}

		/// <summary>
		/// Tests the spreadsheet valid name testing with specific normalizer with getting cell contents.
		/// </summary>
		[TestMethod]
		public void ValidName3()
		{
			Spreadsheet sheet = new Spreadsheet(s => { if (s == "D1") return true; return false; }, s => s.ToUpper(), "default");
			sheet.SetContentsOfCell("d1", "blue");
			Assert.AreEqual(sheet.GetCellContents("d1"), "blue");
		}

		/// <summary>
		/// Tests the spreadsheet getting a double value.
		/// </summary>
		[TestMethod]
		public void GetValueDouble()
		{
			Spreadsheet sheet = new Spreadsheet(s => true, s => s, "default");
			sheet.SetContentsOfCell("D1", "5");
			Assert.AreEqual(sheet.GetCellContents("D1"), (double)5);
			Assert.AreEqual(sheet.GetCellValue("D1"), (double)5);
		}

		/// <summary>
		/// Tests the spreadsheet getting a formula value.
		/// </summary>
		[TestMethod]
		public void GetValueFormula01()
		{
			Spreadsheet sheet = new Spreadsheet(s => true, s => s, "default");
			sheet.SetContentsOfCell("D1", "=5");
			Assert.AreEqual(sheet.GetCellContents("D1"), new Formula("5"));
			Assert.AreEqual(sheet.GetCellValue("D1"), (double)5);
		}

		/// <summary>
		/// Tests the spreadsheet getting a formula value relying on other cell.
		/// </summary>
		[TestMethod]
		public void GetValueFormula02()
		{
			Spreadsheet sheet = new Spreadsheet(s => true, s => s, "default");
			sheet.SetContentsOfCell("D1", "=5");
			sheet.SetContentsOfCell("C1", "=4.5");
			Assert.AreEqual(sheet.GetCellValue("D1"), (double)5);
			Assert.AreEqual(sheet.GetCellValue("C1"), (double)4.5);

			sheet.SetContentsOfCell("B1", "=C1+D1");
			Assert.AreEqual(sheet.GetCellContents("B1"), new Formula("C1 + D1"));
			Assert.AreEqual(sheet.GetCellValue("B1"), (double)9.5);
		}

		/// <summary>
		/// Tests the spreadsheet getting a formula value relying on many cells.
		/// </summary>
		[TestMethod]
		public void GetValueFormula03()
		{
			Spreadsheet sheet = new Spreadsheet(s => true, s => s, "default");
			sheet.SetContentsOfCell("D1", "=5");
			sheet.SetContentsOfCell("C1", "=4.5");
			Assert.AreEqual(sheet.GetCellValue("D1"), (double)5);
			Assert.AreEqual(sheet.GetCellValue("C1"), (double)4.5);
			Assert.AreEqual(sheet.GetCellValue("E1"), "");
			Assert.AreEqual(sheet.GetCellValue("F1"), "");

			sheet.SetContentsOfCell("B1", "=(C1 + D1)/E1 + F1");
			Assert.AreEqual(sheet.GetCellContents("B1"), new Formula("(C1 + D1)/E1 + F1"));
			Assert.IsTrue(sheet.GetCellValue("B1") is FormulaError);
		}

		/// <summary>
		/// Tests the spreadsheet getting a formula value and contents.
		/// </summary>
		[TestMethod]
		public void GetValueFormula04()
		{
			Spreadsheet sheet = new Spreadsheet(s => true, s => s, "default");
			sheet.SetContentsOfCell("D1", "=5");
			sheet.SetContentsOfCell("C1", "=4.5");
			sheet.SetContentsOfCell("E1", "0.5");
			sheet.SetContentsOfCell("F1", "13");
			Assert.AreEqual(sheet.GetCellValue("D1"), (double)5);
			Assert.AreEqual(sheet.GetCellValue("C1"), (double)4.5);
			Assert.AreEqual(sheet.GetCellValue("E1"), (double)0.5);
			Assert.AreEqual(sheet.GetCellValue("F1"), (double)13);

			sheet.SetContentsOfCell("B1", "=(C1 + D1)/E1 + F1");
			Assert.AreEqual(sheet.GetCellContents("B1"), new Formula("(C1 + D1)/E1 + F1"));
			Assert.AreEqual(sheet.GetCellValue("B1"), (double)32);
		}

		/// <summary>
		/// Tests the spreadsheet getting a formula value with empty constructor.
		/// </summary>
		[TestMethod]
		public void GetValueDoubleEmptyConstructor()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("D1", "5");
			Assert.AreEqual(sheet.GetCellContents("D1"), (double)5);
			Assert.AreEqual(sheet.GetCellValue("D1"), (double)5);
		}

		/// <summary>
		/// Tests the spreadsheet getting a formula value with empty constructor.
		/// </summary>
		[TestMethod]
		public void GetValueFormula01EmptyConstructor()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("D1", "=5");
			Assert.AreEqual(sheet.GetCellContents("D1"), new Formula("5"));
			Assert.AreEqual(sheet.GetCellValue("D1"), (double)5);
		}

		/// <summary>
		/// Tests the spreadsheet getting a formula value with empty constructor.
		/// </summary>
		[TestMethod]
		public void GetValueFormula02EmptyConstructor()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("D1", "=5");
			sheet.SetContentsOfCell("C1", "=4.5");
			Assert.AreEqual(sheet.GetCellValue("D1"), (double)5);
			Assert.AreEqual(sheet.GetCellValue("C1"), (double)4.5);

			sheet.SetContentsOfCell("B1", "=C1+D1");
			Assert.AreEqual(sheet.GetCellContents("B1"), new Formula("C1 + D1"));
			Assert.AreEqual(sheet.GetCellValue("B1"), (double)9.5);
		}

		/// <summary>
		/// Tests the spreadsheet getting a formula value with empty constructor.
		/// </summary>
		[TestMethod]
		public void GetValueFormula03EmptyConstructor()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("D1", "=5");
			sheet.SetContentsOfCell("C1", "=4.5");
			Assert.AreEqual(sheet.GetCellValue("D1"), (double)5);
			Assert.AreEqual(sheet.GetCellValue("C1"), (double)4.5);
			Assert.AreEqual(sheet.GetCellValue("E1"), "");
			Assert.AreEqual(sheet.GetCellValue("F1"), "");

			sheet.SetContentsOfCell("B1", "=(C1 + D1)/E1 + F1");
			Assert.AreEqual(sheet.GetCellContents("B1"), new Formula("(C1 + D1)/E1 + F1"));
			Assert.IsTrue(sheet.GetCellValue("B1") is FormulaError);
		}

		/// <summary>
		/// Tests the spreadsheet getting a formula value with empty constructor.
		/// </summary>
		[TestMethod]
		public void GetValueFormula04EmptyConstructor()
		{
			Spreadsheet sheet = new Spreadsheet();
			Assert.IsTrue(!sheet.Changed);
			sheet.SetContentsOfCell("D1", "=5");
			Assert.IsTrue(sheet.Changed );
			sheet.SetContentsOfCell("C1", "=4.5");
			sheet.SetContentsOfCell("E1", "0.5");
			sheet.SetContentsOfCell("F1", "13");
			Assert.AreEqual(sheet.GetCellValue("D1"), (double)5);
			Assert.AreEqual(sheet.GetCellValue("C1"), (double)4.5);
			Assert.AreEqual(sheet.GetCellValue("E1"), (double)0.5);
			Assert.AreEqual(sheet.GetCellValue("F1"), (double)13);

			sheet.SetContentsOfCell("B1", "=(C1 + D1)/E1 + F1");
			Assert.AreEqual(sheet.GetCellContents("B1"), new Formula("(C1 + D1)/E1 + F1"));
			Assert.AreEqual(sheet.GetCellValue("B1"), (double)32);
		}


		/// <summary>
		/// Tests the spreadsheet getting a formula value with empty constructor and multiple cell layers.
		/// </summary>
		[TestMethod]
		public void GetValueFormulaMultipleCellLayers01()
		{
			Spreadsheet sheet = new Spreadsheet();
			Assert.IsTrue(!sheet.Changed);
			sheet.SetContentsOfCell("D1", "=5");
			Assert.IsTrue(sheet.Changed);
			sheet.SetContentsOfCell("C1", "=4.5");
			sheet.SetContentsOfCell("E1", "0.5");
			sheet.SetContentsOfCell("G1", "-2");
			sheet.SetContentsOfCell("F1", "= G1 + 13");
			Assert.AreEqual(sheet.GetCellValue("D1"), (double)5);
			Assert.AreEqual(sheet.GetCellValue("C1"), (double)4.5);
			Assert.AreEqual(sheet.GetCellValue("E1"), (double)0.5);
			Assert.AreEqual(sheet.GetCellValue("F1"), (double)11);
			Assert.AreEqual(sheet.GetCellValue("G1"), (double)-2);

			sheet.SetContentsOfCell("B1", "=(C1 + D1)/E1 + F1");
			Assert.AreEqual(sheet.GetCellContents("B1"), new Formula("(C1 + D1)/E1 + F1"));
			Assert.AreEqual(sheet.GetCellValue("B1"), (double)30);
		}

		/// <summary>
		/// Tests the spreadsheet getting a formula value with empty constructor with multiple cell layers.
		/// </summary>
		[TestMethod]
		public void GetValueFormulaMultipleCellLayers02()
		{
			Spreadsheet sheet = new Spreadsheet();
			Assert.IsTrue(!sheet.Changed);
			sheet.SetContentsOfCell("D1", "=5");
			Assert.IsTrue(sheet.Changed);
			sheet.SetContentsOfCell("C1", "=4.5");
			sheet.SetContentsOfCell("E1", "0.5");
			sheet.SetContentsOfCell("G1", "-2");
			sheet.SetContentsOfCell("H1", "-4");
			sheet.SetContentsOfCell("F1", "= G1 + H1");
			Assert.AreEqual(sheet.GetCellValue("D1"), (double)5);
			Assert.AreEqual(sheet.GetCellValue("C1"), (double)4.5);
			Assert.AreEqual(sheet.GetCellValue("E1"), (double)0.5);
			Assert.AreEqual(sheet.GetCellValue("F1"), (double)-6);
			Assert.AreEqual(sheet.GetCellValue("G1"), (double)-2);

			sheet.SetContentsOfCell("B1", "=(C1 + D1)/E1 + F1");
			Assert.AreEqual(sheet.GetCellContents("B1"), new Formula("(C1 + D1)/E1 + F1"));
			Assert.AreEqual(sheet.GetCellValue("B1"), (double)13);
		}

		/// <summary>
		/// Tests the spreadsheet creating, saving, and loading to and from an XML file.
		/// </summary>
		[TestMethod]
		public void CreateSaveLoadSpreadsheet()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("D1", "=5");
			sheet.SetContentsOfCell("C1", "=4.5");
			sheet.SetContentsOfCell("E1", "apples");
			sheet.SetContentsOfCell("G1", "-2");
			sheet.SetContentsOfCell("H1", "-4");
			sheet.SetContentsOfCell("F1", "= G1 + H1");
			sheet.Save(@"MyXml2.xml");
			Spreadsheet sheet2 = new Spreadsheet(@"MyXml2.xml", s => true, s => s, "default");
			Assert.AreEqual(sheet2.GetCellContents("D1"), new Formula("5"));
			Assert.AreEqual(sheet2.GetCellContents("C1"), new Formula("4.5"));
			Assert.AreEqual(sheet2.GetCellContents("E1"), "apples");
			Assert.AreEqual(sheet2.GetCellContents("G1"), (double)-2);
			Assert.AreEqual(sheet2.GetCellContents("H1"), (double)-4);
			Assert.AreEqual(sheet2.GetCellContents("F1"), new Formula("G1 + H1"));
			Assert.AreEqual(sheet2.GetCellValue("F1"), (double)-6);
		}
	}
}
