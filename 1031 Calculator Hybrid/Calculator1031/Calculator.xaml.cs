using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Calculator1031
{
	public partial class Calculator : ContentPage
	{
		string resourcePrefix = "";
		List<DataString> _states = null;
		List<DataString> _singleIncome = null;
		List<DataString> _maritalIncome = null;
		double percentageComplete = 0;
		double savings = 0.0;
		double percentageTax = 0.0;

		public Calculator ()
		{
			InitializeComponent ();

			#if __IOS__
				resourcePrefix = "Calculator1031.iOS.";
				gridCalc.RowDefinitions[0].Height = new GridLength(20);
			#endif
			#if __ANDROID__
				resourcePrefix = "Calculator1031.Droid.";
				gridCalc.RowDefinitions[0].Height = new GridLength(0);
			#endif
			#if WINDOWS_PHONE
				resourcePrefix = "Calculator1031.WinPhone.";
				gridCalc.RowDefinitions[0].Height = new GridLength(0);
			#endif

			PopulateStates ();
			PopulateMaritalStatus ();
			PopulateIncome ();

			if (percentageComplete != 100.0)
				btnCalculate.IsEnabled = false;
			else
				btnCalculate.IsEnabled = true;
		}

		private void PopulateStates()
		{
			if (_states != null && _states.Count > 0)
				return;

			pickerState.Items.Clear ();
			DataString[] dataStates = LoadFromJSON.GetData (resourcePrefix + "Resources.States.json");

			if (dataStates != null && dataStates.Length > 0) 
			{
				_states = new List<DataString> ();
				foreach (DataString data in dataStates) 
				{
					pickerState.Items.Add (data.Text);
					_states.Add (data);
				}
			}
		}

		private void PopulateMaritalStatus()
		{
			if (pickerMS.Items.Count > 0)
				return;
			
			pickerMS.Items.Clear ();
			pickerMS.Items.Add ("Single");
			pickerMS.Items.Add ("Married");
		}

		private void PopulateIncome()
		{
			DataString[] dataIncome = null;
			if (pickerMS.SelectedIndex == 0 && (_singleIncome == null || _singleIncome.Count <= 0)) 
			{
				dataIncome = LoadFromJSON.GetData (resourcePrefix + "Resources.SingleIncomeGroup.json");
			} 
			else if (pickerMS.SelectedIndex == 1 && (_maritalIncome == null || _maritalIncome.Count <= 0)) 
			{
				dataIncome = LoadFromJSON.GetData (resourcePrefix + "Resources.MarriedIncomeGroup.json");
			}

			if (dataIncome != null && dataIncome.Length > 0) 
			{
				pickerIncome.Items.Clear ();

				if (pickerMS.SelectedIndex == 0) 
				{
					_singleIncome = new List<DataString> ();
					foreach (DataString data in dataIncome) 
					{
						pickerIncome.Items.Add (data.Text);
						_singleIncome.Add (data);
					}
				}
				else if (pickerMS.SelectedIndex == 1) 
				{
					_maritalIncome = new List<DataString> ();
					foreach (DataString data in dataIncome) 
					{
						pickerIncome.Items.Add (data.Text);
						_maritalIncome.Add (data);
					}
				}
			}
		}

		public void EntryPP_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.NewTextValue == e.OldTextValue)
				return;
			
			double pp = 0.0;

			if (!Double.TryParse (e.NewTextValue, out pp))
				Double.TryParse (e.OldTextValue, out pp);

			entryPP.Text = pp.ToString ();

			UpdatePercentageComplete ();
		}
			
		public void EntryCI_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.NewTextValue == e.OldTextValue)
				return;
			
			double ci = 0.0;

			if (!Double.TryParse (e.NewTextValue, out ci))
				Double.TryParse (e.OldTextValue, out ci);
			
			entryCI.Text = ci.ToString ();

			UpdatePercentageComplete ();
		}

		public void EntrySP_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.NewTextValue == e.OldTextValue)
				return;

			double sp = 0.0;

			if (!Double.TryParse (e.NewTextValue, out sp))
				Double.TryParse (e.OldTextValue, out sp);

			entrySP.Text = sp.ToString ();

			UpdatePercentageComplete ();
		}

		public void pickerState_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateTaxRate ();
			UpdatePercentageComplete ();
		}

		public void pickerMS_SelectedIndexChanged(object sender, EventArgs e)
		{
			PopulateIncome ();
			UpdateTaxRate ();
			UpdatePercentageComplete ();
		}

		public void pickerIncome_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateTaxRate ();
			UpdatePercentageComplete ();
		}

		private void UpdateTaxRate()
		{
			percentageTax = CalculateTaxRate ();
			lblPercTax.Text = percentageTax.ToString() + "% Tax Rate";
		}

		private double CalculateTaxRate()
		{
			string stateTax = "0.0";
			string federalTax = "0.0";

			if (pickerState.SelectedIndex >= 0)
			{
				stateTax = _states [pickerState.SelectedIndex].Value;
			}

			if (pickerMS.SelectedIndex == 0 && pickerIncome.SelectedIndex >= 0) 
			{
				federalTax = _singleIncome [pickerIncome.SelectedIndex].Value;
			} 
			else if (pickerMS.SelectedIndex == 1 && pickerIncome.SelectedIndex >= 0) 
			{
				federalTax = _maritalIncome [pickerIncome.SelectedIndex].Value;
			}

			return Convert.ToDouble(stateTax) + Convert.ToDouble(federalTax);
		}

		private void UpdatePercentageComplete()
		{
			int count = 0;

			if (!string.IsNullOrEmpty (entryPP.Text))
				count++;

			if (!string.IsNullOrEmpty (entryCI.Text))
				count++;

			if (!string.IsNullOrEmpty (entrySP.Text))
				count++;

			if (pickerState.SelectedIndex >= 0)
				count++;

			if (pickerMS.SelectedIndex >= 0)
				count++;

			if (pickerIncome.SelectedIndex >= 0)
				count++;

			percentageComplete = count * 100 / 6;
			string complete = percentageComplete.ToString () + "% Complete";
			lblPercComplete.Text = complete;

			if (percentageComplete != 100.0)
				btnCalculate.IsEnabled = false;
			else
				btnCalculate.IsEnabled = true;
		}

		private void CalculateSavings()
		{
			if (percentageComplete == 100.00)
			{
				double pp = Convert.ToDouble(entryPP.Text);
				double ci = Convert.ToDouble(entryCI.Text);
				double sp = Convert.ToDouble(entrySP.Text);

				if (sp < pp + ci)
				{
					savings = 0.00;
				}
				else
				{
					double gain = sp - (pp + ci);
					savings = gain * percentageTax / 100;
				}
			}
			else
			{
				savings = 0.0;
			}
		}

		public void btnCalculate_Clicked(object sender, EventArgs e)
		{
			CalculateSavings ();
			lblPercComplete.Text = "Your 1031 savings: $" + savings.ToString();
		}

		private Property GetProperty()
		{
			Property prop = new Property ();

			if (!string.IsNullOrEmpty (entryPP.Text))
				prop.PurchasePrice = Convert.ToDouble (entryPP.Text);

			if (!string.IsNullOrEmpty (entryCI.Text))
				prop.CapitalImprovements = Convert.ToDouble (entryCI.Text);

			if (!string.IsNullOrEmpty (entrySP.Text))
				prop.SalePrice = Convert.ToDouble (entrySP.Text);

			if (pickerState.SelectedIndex >= 0 && (_states != null && _states.Count > 0))
				prop.State = _states [pickerState.SelectedIndex].Text;

			if (pickerMS.SelectedIndex >= 0)
				prop.MaritalStatus = pickerMS.SelectedIndex == 0 ? "Single" : "Married";

			if (pickerIncome.SelectedIndex >= 0) 
			{
				if (pickerMS.SelectedIndex == 0 && (_singleIncome != null && _singleIncome.Count > 0))
					prop.Income = _singleIncome [pickerIncome.SelectedIndex].Text;
				else if (pickerMS.SelectedIndex == 1 && (_maritalIncome != null && _maritalIncome.Count > 0))
					prop.Income = _maritalIncome [pickerIncome.SelectedIndex].Text;
			}

			prop.TaxRate = percentageTax;

			CalculateSavings ();
			prop.Savings = savings;

			return prop;
		}

		async void btnSave_Clicked(object sender, EventArgs e)
		{
			if (percentageComplete != 100.0) 
			{
				await DisplayAlert ("Alert", "Please fill in all the fields before saving the calculation!", "OK");
			} 
			else 
			{
				var saveDialog = new SaveDialog (GetProperty ());
				await Navigation.PushModalAsync (saveDialog);
			}
		}
	}
}

