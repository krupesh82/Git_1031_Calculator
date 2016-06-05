using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Calculator1031
{
	public partial class SaveDialog : ContentPage
	{
		private Property property = null;

		public SaveDialog (Property prop)
		{
			InitializeComponent ();
			property = prop;
		}

		async void btnSaveName_Clicked(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty (entryName.Text)) 
			{
				await DisplayAlert ("Alert", "Please enter the property name!", "OK");
			} 
			else 
			{
				string name = entryName.Text;
				DBHelper dbHelper = new DBHelper ();

				if (dbHelper.DoesNameExist (name)) 
				{
					await DisplayAlert ("Alert", "The property name already exists. Please choose different name.", "OK");
				}  
				else 
				{
					property.Name = name;
					dbHelper.InsertProperty (property);
					await Navigation.PopModalAsync();
				}
			}

		}

		async void btnDismiss_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopModalAsync();
		}
	}
}

