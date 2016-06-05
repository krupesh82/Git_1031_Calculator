using System;
using SQLite;

namespace Calculator1031
{
	public class Property
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string Name { get; set; }

		public double PurchasePrice { get; set; }

		public double CapitalImprovements { get; set; }

		public double SalePrice { get; set; }

		public string State { get; set; }

		public string MaritalStatus { get; set; }

		public string Income { get; set; }

		public double TaxRate { get; set; }

		public double Savings { get; set; }        
	}
}

