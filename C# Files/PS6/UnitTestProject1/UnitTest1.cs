using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System.Collections.Generic;

namespace UnitTestProject1
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			Spreadsheet s = new Spreadsheet();
			IEnumerable<string> cells = s.GetNamesOfAllNonemptyCells();
			Assert.IsFalse(cells.GetEnumerator().MoveNext());
		}
	}
}
