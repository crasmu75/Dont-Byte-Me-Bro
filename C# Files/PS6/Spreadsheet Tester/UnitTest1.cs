using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System.Collections.Generic;

namespace Spreadsheet_Tester
{
	[TestClass]
	public class UnitTest1
	{
		//[TestMethod]
		//private void TestMethod1()
		//{
		//	PrivateObject target = new PrivateObject(typeof (Spreadsheet));
		//	Type type = typeof(Spreadsheet);

		//	var checkValid = type.GetMethod("checkValidCellName");
		//	bool returned = (bool) checkValid.Invoke("", new object[1] {"2"});

		//	Assert.IsFalse(returned);
		//}

		[TestMethod]
		private void TestMethod2()
		{
			Spreadsheet s = new Spreadsheet();
			IEnumerable<string> cells = s.GetNamesOfAllNonemptyCells();
			Assert.IsFalse(cells.GetEnumerator().MoveNext());
			
		}
	}
}
