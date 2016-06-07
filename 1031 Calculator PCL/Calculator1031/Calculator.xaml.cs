using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Plugin.Toasts;

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
			//FileImageSource fis = new FileImageSource ();
			//fis.File = "deletetrashicon.png";
			//FileImageSource.FromResource(
			//this.Icon = fis;

			if (Device.OS == TargetPlatform.iOS)
			{
				resourcePrefix = "Calculator1031.iOS.";
				gridCalc.RowDefinitions[0].Height = new GridLength(20);
				gridPercComplete.RowDefinitions[1].Height = new GridLength(130);
				gridPercComplete.ColumnDefinitions[1].Width = new GridLength(130);
				//	UIKit.UIImage img = new UIKit.UIImage("delete_trash.png");
				//this.Icon = ImageSource.FromFile("Calculator.svg");
			}
			if (Device.OS == TargetPlatform.Android) {
				resourcePrefix = "Calculator1031.Droid.";
				gridCalc.RowDefinitions [0].Height = new GridLength (0);
				gridCalc.RowDefinitions [1].Height = new GridLength (50);
				gridCalc.RowDefinitions [3].Height = new GridLength (40);
				gridCalc.RowDefinitions [4].Height = new GridLength (40);
				gridCalc.RowDefinitions [5].Height = new GridLength (40);
				gridCalc.RowDefinitions [6].Height = new GridLength (40);
				gridCalc.RowDefinitions [7].Height = new GridLength (40);
				gridCalc.RowDefinitions [8].Height = new GridLength (40);
				gridCalc.RowDefinitions [10].Height = new GridLength (0);
				gridPercComplete.RowDefinitions [1].Height = new GridLength (140);
				gridPercComplete.ColumnDefinitions [1].Width = new GridLength (140);
				btnSave.HeightRequest = 40;
				btnCalculate.HeightRequest = 40;
				blankText.HeightRequest = 0;
			}

			PopulateStates ();
			PopulateMaritalStatus ();
			PopulateIncome ();

			if (percentageComplete != 100.0)
				btnCalculate.IsEnabled = false;
			else
				btnCalculate.IsEnabled = true;

			var s = new FormattedString ();
			s.Spans.Add (new Span{ Text = percentageTax.ToString () + "%", FontSize= 27, FontAttributes = FontAttributes.Bold });
			s.Spans.Add (new Span { Text = Environment.NewLine });
			s.Spans.Add (new Span{ Text = "Tax Rate", FontSize= 16 });
			lblPercTax.FormattedText = s;

			s = new FormattedString ();
			s.Spans.Add (new Span{ Text = percentageTax.ToString () + "%", FontSize= 34, FontAttributes = FontAttributes.Bold });
			s.Spans.Add (new Span { Text = Environment.NewLine });
			s.Spans.Add (new Span{ Text = "Complete", FontSize= 16 });
			lblPercComplete.FormattedText = s;
		}

		private void PopulateStates()
		{
			if (_states != null && _states.Count > 0)
				return;

			pickerState.Items.Clear ();
			DataString[] dataStates = LoadFromJSON.GetData ("Calculator1031.Resources.States.json");

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
				dataIncome = LoadFromJSON.GetData ("Calculator1031.Resources.SingleIncomeGroup.json");
			} 
			else if (pickerMS.SelectedIndex == 1 && (_maritalIncome == null || _maritalIncome.Count <= 0)) 
			{
				dataIncome = LoadFromJSON.GetData ("Calculator1031.Resources.MarriedIncomeGroup.json");
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
			var s = new FormattedString ();
			s.Spans.Add (new Span{ Text = percentageTax.ToString () + "%", FontSize= 27, FontAttributes = FontAttributes.Bold });
			s.Spans.Add (new Span { Text = Environment.NewLine });
			s.Spans.Add (new Span{ Text = "Tax Rate", FontSize= 16 });
			lblPercTax.FormattedText = s;
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
			var s = new FormattedString ();
			s.Spans.Add (new Span{ Text = percentageComplete.ToString () + "%", FontSize= 34, FontAttributes = FontAttributes.Bold });
			s.Spans.Add (new Span { Text = Environment.NewLine });
			s.Spans.Add (new Span{ Text = "Complete", FontSize= 16 });
			lblPercComplete.FormattedText = s;

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
			var s = new FormattedString ();
			s.Spans.Add (new Span{ Text = "Your 1031 savings:", FontSize= 16 });
			s.Spans.Add (new Span { Text = Environment.NewLine });
			s.Spans.Add (new Span{ Text = "$" + savings.ToString (), FontSize= 24, FontAttributes = FontAttributes.Bold });
			lblPercComplete.FormattedText = s;
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
				ShowToast (ToastNotificationType.Info, "Please fill in all the fields before saving the calculation.", 2);
				//await DisplayAlert ("Alert", "Please fill in all the fields before saving the calculation!", "OK");
			} 
			else 
			{
				var saveDialog = new SaveDialog (GetProperty ());
				await Navigation.PushModalAsync (saveDialog);
			}
		}

		private async void ShowToast(ToastNotificationType type, string message, int toastTime)
		{
			var notificator = DependencyService.Get<IToastNotificator>();
			bool tapped = await notificator.Notify(type, message, "", TimeSpan.FromSeconds(toastTime));
		}
	}
}

