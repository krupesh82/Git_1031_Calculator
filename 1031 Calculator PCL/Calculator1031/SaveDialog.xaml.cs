using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Calculator1031
{
	public partial class SaveDialog : ContentPage
	{
		public Property Property = null;
		public bool IsCancelled = false;

		public SaveDialog (Property prop)
		{
			InitializeComponent ();
			Property = prop;
		}

		async void btnSaveName_Clicked(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty (entryName.Text)) 
			{	
				ShowToast ("Please enter the property name.", 2000);
			} 
			else 
			{
				string name = entryName.Text;
				DBHelper dbHelper = new DBHelper ();

				if (dbHelper.DoesNameExist (name)) 
				{
					ShowToast ("The property name already exists. Please choose different name.", 2000);
				}  
				else 
				{
					try{
						Property.Name = name;
						dbHelper.InsertProperty (Property);
						await Navigation.PopModalAsync();
					}
					catch(Exception ex) {
						ShowToast ("Oops! Some error occured. Please try again.", 2000);
					}
				}
			}

		}

		async void btnDismiss_Clicked(object sender, EventArgs e)
		{
			this.IsCancelled = true;
			await Navigation.PopModalAsync();
		}

		private void ShowToast(string message, int toastTime)
		{
			toastSDLabel.Text = message;
			toastSDLabel.IsVisible = true;
			Device.StartTimer(TimeSpan.FromMilliseconds(toastTime), () => {
				toastSDLabel.Text = "";
				toastSDLabel.IsVisible = false;
				return false;
			});
		}
	}
}

