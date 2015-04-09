// Used to try XML methods and test spreadsheet save method and constructor.
// Drew McClelland
// u0634481
// CS 3500-F14
// PS5
// 10/15/2014

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;
using System.Xml;
using System.IO;

namespace ConsoleApplication1
{
	class Tester
	{
		static void Main(string[] args)
		{
			//Console.ReadLine();

			//XmlTextReader test = new XmlTextReader("C:\\Users\\Mac\\Desktop\\xml.xml");

			//test.ReadToFollowing("spreadsheet");
			//test.MoveToFirstAttribute();
			//Console.WriteLine("Version: " + test.Value);

			//while (test.Read())
			//{
			//	if (test.ReadToFollowing("cell"))
			//	{
			//		test.ReadToFollowing("name");
			//		string name = test.ReadElementContentAsString();
			//		Console.WriteLine("Cell: " + name);
			//		test.ReadToFollowing("contents");
			//		string content = test.ReadElementContentAsString();
			//		Console.WriteLine("Content: " + content);

			//		Console.WriteLine();
			//		continue;
			//	}
			//	break;
			//}
			//Console.ReadLine();
			//XmlWriterSettings settings = new XmlWriterSettings();
			//settings.NewLineChars = "\n";
			//settings.IndentChars = "";
			//settings.Indent = true;
			//XmlWriter x = XmlWriter.Create("..\\..\\..\\Project2\\MyXml.xml", settings);
			//x.WriteStartDocument();
			//x.WriteStartElement("spreadsheet");
			//x.WriteAttributeString("version", "xxblue");
			//x.WriteStartElement("cell");
			//x.WriteStartElement("name");
			//x.WriteString("\n");
			//x.WriteString("Cell Name");
			//x.WriteString("\n");
			//x.WriteEndElement();
			//x.WriteStartElement("contents");
			//x.WriteString("\n");
			//x.WriteString("Cell contents");
			//x.WriteString("\n");
			//x.WriteEndElement();
			//x.WriteEndElement();
			//x.WriteEndElement();
			//x.WriteEndDocument();
			//x.Close();

			//Spreadsheet sheet = new Spreadsheet();
			//sheet.SetContentsOfCell("D1", "=5");
			//sheet.SetContentsOfCell("C1", "=4.5");
			//sheet.SetContentsOfCell("E1", "appaales");
			//sheet.SetContentsOfCell("G1", "-2");
			//sheet.SetContentsOfCell("H1", "-4");
			//sheet.SetContentsOfCell("F1", "= G1 + H1");
			//sheet.Save(@"C:\Users\Mac\Desktop\MyXml2.xml");
			//Spreadsheet sheet2 = new Spreadsheet(@"C:\Users\Mac\Desktop\MyXml2.xml", s => true, s => s, "default");
			//Console.WriteLine(sheet2.GetCellContents("D1"));
			//Console.WriteLine(sheet2.GetCellContents("C1"));
			//Console.WriteLine(sheet2.GetCellContents("E1"));
			//Console.WriteLine(sheet2.GetCellContents("G1"));
			//Console.WriteLine(sheet2.GetCellContents("H1"));
			//Console.WriteLine(sheet2.GetCellContents("F1"));
		

			//Console.ReadLine();	
		}
	}
}
