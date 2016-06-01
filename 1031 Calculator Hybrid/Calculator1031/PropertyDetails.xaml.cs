using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Calculator1031
{
	public partial class PropertyDetails : ContentPage
	{
		public Grid GridView
		{
			get { return gridDetails; }
		}
		public PropertyDetails ()
		{
			InitializeComponent ();
		}

		public void SetPropertyDetails(Property p)
		{
			lblName.Text = p.Name;
			lblState.Text = p.State;
			lblMaritalStatus.Text = p.MaritalStatus;
			lblIncome.Text = p.Income;
			lblPP.Text = "$" + p.PurchasePrice.ToString ();
			lblCI.Text = "$" + p.CapitalImprovements.ToString ();
			lblSP.Text = "$" + p.SalePrice.ToString ();
			lblTaxRate.Text = p.TaxRate.ToString () + "%";
			lblSavings.Text = "$" + p.Savings.ToString ();
		}
	}
}

