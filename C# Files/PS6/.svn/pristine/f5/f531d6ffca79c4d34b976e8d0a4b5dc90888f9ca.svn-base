// Written by Drew McClelland
// u0634481
// PS4, Modified for PS5
// CS 3500 - F14

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System.Collections.Generic;
using SpreadsheetUtilities;


namespace SpreadsheetTester
{
	/// <summary>
	/// This class contains tests for the Spreadsheet class.
	/// </summary>
	[TestClass]
	public class UnitTest1
	{
		/// <summary>
		/// Tests getter and setter for text.
		/// </summary>
		[TestMethod]
		public void SetStringContent1()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("A1", "blue");
			Assert.AreEqual(sheet.GetCellContents("A1"), "blue");
		}

		/// <summary>
		/// Tests resetting cell text.
		/// </summary>
		[TestMethod]
		public void SetStringContentTwice()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("A1", "blue");
			sheet.SetContentsOfCell("A1", "green");
			Assert.AreEqual(sheet.GetCellContents("A1"), "green");
			Assert.AreNotEqual(sheet.GetCellContents("A1"), "blue");
		}

		/// <summary>
		/// Tests get cells to recalculate after a change in formula.
		/// </summary>
		[TestMethod]
		public void TestReturnedSet1()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("A1", "blue");
			sheet.SetContentsOfCell("A1", "green");
			sheet.SetContentsOfCell("B1", "purple");
			sheet.SetContentsOfCell("C1", "red");
			sheet.SetContentsOfCell("D1", "=A1");

			HashSet<string> set = (HashSet<string>)sheet.SetContentsOfCell("A1", "=B1 + C1");
			Assert.IsTrue(set.Count == 2);
			Assert.IsTrue(set.Contains("A1"));
			Assert.IsTrue(set.Contains("D1"));
		}

		/// <summary>
		/// Tests get cells to recalculate on a cell with no dependees.
		/// </summary>
		[TestMethod]
		public void TestReturnedSet2()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("A1", "blue");
			sheet.SetContentsOfCell("A1", "green");
			sheet.SetContentsOfCell("B1", "purple");
			sheet.SetContentsOfCell("C1", "red");
			sheet.SetContentsOfCell("D1", "C1");

			HashSet<string> set = (HashSet<string>)sheet.SetContentsOfCell("A1", "B1 + C1");
			Assert.IsTrue(set.Count == 1);
			Assert.IsTrue(set.Contains("A1"));
		}

		/// <summary>
		/// Tests getting cell contents after change in formula.
		/// </summary>
		[TestMethod]
		public void GetCellContentsFormula()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("A1", "blue");
			sheet.SetContentsOfCell("A1", "green");
			sheet.SetContentsOfCell("B1", "purple");
			sheet.SetContentsOfCell("C1", "red");
			sheet.SetContentsOfCell("D1", "=C1 + 2 + X1");

			Assert.AreEqual(sheet.GetCellContents("D1"), new Formula("C1+ 2+ X1"));
		}

		/// <summary>
		/// Tests get cells to recalculate after change in formula.
		/// </summary>
		[TestMethod]
		public void TestReturnedSetChanged()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("A1", "blue");
			sheet.SetContentsOfCell("A1", "green");
			sheet.SetContentsOfCell("B1", "purple");
			sheet.SetContentsOfCell("C1", "red");
			sheet.SetContentsOfCell("D1", "C1 + A1");
			sheet.SetContentsOfCell("D1", "B1");

			HashSet<string> set = (HashSet<string>)sheet.SetContentsOfCell("A1", "B1 + C1");
			Assert.IsTrue(set.Count == 1);
			Assert.IsTrue(set.Contains("A1"));
		}

		/// <summary>
		/// Tests circular dependency.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(CircularException))]
		public void TestCircularException1()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("A1", "=D1");
			sheet.SetContentsOfCell("B1", "purple");
			sheet.SetContentsOfCell("C1", "red");
			sheet.SetContentsOfCell("D1", "=C1 + B1");
			sheet.SetContentsOfCell("D1", "=A1");
		}

		/// <summary>
		/// Tests circular dependency for reset formula.
		/// </summary>
		[TestMethod]
		public void TestCircularException2()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("A1", "=D1");
			sheet.SetContentsOfCell("B1", "purple");
			sheet.SetContentsOfCell("C1", "red");
			
			sheet.SetContentsOfCell("D1", "=C1 + B1");
			try
			{
				sheet.SetContentsOfCell("D1", "=A1");
			}
			catch (CircularException)
			{

			}

			Assert.AreEqual(sheet.GetCellContents("D1"), new Formula("C1 + B1"));
		}

		/// <summary>
		/// Tests get names of all nonempty cells after adding cells with no content.
		/// </summary>
		[TestMethod]
		public void TestAddingEmptyCell()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("A1", "D1");
			sheet.SetContentsOfCell("B1", "purple");
			sheet.SetContentsOfCell("C1", "4");

			sheet.SetContentsOfCell("A1", "");
			sheet.SetContentsOfCell("B1", "");
			sheet.SetContentsOfCell("C1", "");

			
			Assert.IsFalse(sheet.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
		}

		/// <summary>
		/// Tests for addition of valid cell names.
		/// </summary>
		[TestMethod]
		public void TestValidName()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("aaa4", "=D1");
			sheet.SetContentsOfCell("a1", "purple");
			sheet.SetContentsOfCell("abA2", "4");
			sheet.SetContentsOfCell("ab22", "4");
			sheet.SetContentsOfCell("ab32", "4");
			sheet.SetContentsOfCell("aA1", "4");
			sheet.SetContentsOfCell("A22", "4");
			sheet.SetContentsOfCell("A330", "");

			int cellCount = 0;
			IEnumerator<string> nonEmpty = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();
			while (nonEmpty.MoveNext())
				cellCount++;

			Assert.AreEqual(cellCount, 7);
		}

		/// <summary>
		/// Tests setting invalid cell name with an "ampersand" in the name.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void TestSetInvalidName1()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("_&", "D1");
		}

		/// <summary>
		/// Tests setting invalid cell name with just a number.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void TestSetInvalidName2()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("1", "purple");
		}

		/// <summary>
		/// Tests setting invalid cell name with just a ampersand.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void TestSetInvalidName3()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("&", "4");
		}

		/// <summary>
		/// Tests setting invalid cell name with a number followed by a letter.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void TestSetInvalidName4()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("1b", "4");
		}

		/// <summary>
		/// Tests setting invalid cell name with a whitespace character to a number.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void TestSetInvalidName5()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("\n", "4");
		}

		/// <summary>
		/// Tests setting invalid cell name with empty string.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void TestSetInvalidName6()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("", "4");
		}

		/// <summary>
		/// Tests setting invalid cell name with a whiespace character to text.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void TestSetInvalidName7()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("\n", "blue");
		}

		/// <summary>
		/// Tests setting invalid cell name with a whitespace character to formula.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void TestSetInvalidName8()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("\n", "C1 + D1");
		}

		/// <summary>
		/// Tests getting invalid cell name with an ampersand in the name.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void TestGetInvalidName1()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.GetCellContents("_&");
		}

		/// <summary>
		/// Tests getting invalid cell name with just a number.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void TestGetInvalidName2()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.GetCellContents("1");
		}

		/// <summary>
		/// Tests getting invalid cell name with just an ampersand.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void TestGetInvalidName3()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.GetCellContents("&");
		}

		/// <summary>
		/// Tests getting invalid cell name with a number followed by a letter.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void TestGetInvalidName4()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.GetCellContents("1b");
		}

		/// <summary>
		/// Tests getting invalid cell name with a whitespace character to a number.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void TestGetInvalidName5()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.GetCellContents("\n");
		}

		/// <summary>
		/// Tests reseting a formula to a double and getting cells to recalculate.
		/// </summary>
		[TestMethod]
		public void TestResetDouble()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("A1", "B1 + C1");
			IEnumerator<string> set = sheet.SetContentsOfCell("A1", "5").GetEnumerator();
			
			Assert.AreEqual(set.MoveNext(), true);
			Assert.AreEqual(set.Current, "A1");
			Assert.IsFalse(set.MoveNext());
		}

		/// <summary>
		/// Tests resetting a formula to a string and getting cells to recalculate.
		/// </summary>
		[TestMethod]
		public void TestResetString1()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("A1", "B1 + C1");
			IEnumerator<string> set = sheet.SetContentsOfCell("A1", " ").GetEnumerator();

			Assert.AreEqual(set.MoveNext(), true);
			Assert.AreEqual(set.Current, "A1");
			Assert.IsFalse(set.MoveNext());
		}

		/// <summary>
		/// Tests resetting formula to an empty string and getting cells to recalculate.
		/// </summary>
		[TestMethod]
		public void TestResetString2()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("A1", "B1 + C1");
			IEnumerator<string> set = sheet.SetContentsOfCell("A1", "").GetEnumerator();

			Assert.AreEqual(set.MoveNext(), true);
			Assert.AreEqual(set.Current, "A1");
			Assert.IsFalse(set.MoveNext());
		}

		/// <summary>
		/// Tests passing empty string as cell name.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void ValidCellNameEmpty()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell("", "B1 + C1 + D1");
		}

		/// <summary>
		/// Tests passing null as a cell name.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void ValidCellNameNull()
		{
			Spreadsheet sheet = new Spreadsheet();
			sheet.SetContentsOfCell(null, "B1 + C1 + D1");
		}

		/// <summary>
		/// Tests getting a cell who hasn't been set yet.
		/// </summary>
		[TestMethod]
		public void GetUnsetCell()
		{
			Spreadsheet sheet = new Spreadsheet();
			Assert.AreEqual(sheet.GetCellContents("A1"), "");
		}

		/// <summary>
		/// Tests creating an abstract spreadsheet.
		/// </summary>
		[TestMethod]
		public void AbstractSpreadsheet()
		{
			AbstractSpreadsheet sheet = new Spreadsheet();
			Assert.AreEqual(sheet.GetCellContents("A1"), "");
		}
	}
}
