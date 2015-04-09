using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;

namespace SS
{
	/// <summary>
	/// This class represents a cell that contains a name, content, and a value.
	/// </summary>
	public class Cell
	{
		private string name;
		private object content;
		private object value;

		/// <summary>
		/// Public constructor for a cell.
		/// Name, Content, and Value initialized to parameters.
		/// </summary>
		/// <param name="name">Name of the cell.</param>
		/// <param name="contents">Contents of the Cell. Must be a string, number, or formula.</param>
		/// <param name="value">Value of the cell. Must be a string, number, or formula error.</param>
		public Cell(string name, object contents, object value)
		{
			this.name = name;
			this.content = contents;
			this.value = value;
		}

		/// <summary>
		/// Public constructor for a cell.
		/// Name, Content, and Value intialized to null.
		/// </summary>
		public Cell()
		{
			this.name = null;
			this.content = null;
			this.value = null;
		}

		/// <summary>
		/// get or set Name.
		/// </summary>
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		/// <summary>
		/// get or set Content.
		/// </summary>
		public object Content
		{
			get { return content; }
			set { content = value; }
		}

		/// <summary>
		/// get or set Value.
		/// </summary>
		public object Value
		{
			get { return value; }
			set { this.value = value; }
		}

		/// <summary>
		/// Provides string representation of a Cell.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if (this.content == null && this.value == null)
				return ("Name: " + name + "\tContents: " + "null" + "\tValue: " + "null");

			else if (this.content == null)
				return ("Name: " + name + "\tContents: " + "null" + "\tValue: " + value.ToString());

			else if (this.value == null)
				return ("Name: " + name + "\tContents: " + this.content.ToString() + "\tValue: " + "null");

			else
				return ("Name: " + name + "\tContents: " + this.content.ToString() + "\tValue: " + value.ToString());
		}

		/// <summary>
		/// If the argument is not a Cell, return false.
		/// If this Cell has the same name as the argument Cell, this returns true. Else, this returns false.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (!(obj is Cell))
				return false;
			Cell param = (Cell)obj;
			if (param.name == this.name)
				return true;
			return false;
		}

		/// <summary>
		/// Uses string hash function.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return name.GetHashCode();
		}

		/// <summary>
		/// Clones the Cell returning a new one with the same name, content, and value.
		/// </summary>
		/// <returns></returns>
		public Cell Clone()
		{
			if (value is Formula)
			{
				Formula newFormula = new Formula(value.ToString());
				return new Cell(name, content, newFormula);
			}
			else
				return new Cell(name, content, value);
		}
	}
}
