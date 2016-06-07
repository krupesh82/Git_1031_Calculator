using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Plugin.Toasts;

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
				ShowToast (ToastNotificationType.Info, "Please enter the property name.", 2);
				//await DisplayAlert ("Alert", "Please enter the property name!", "OK");

			} 
			else 
			{
				string name = entryName.Text;
				DBHelper dbHelper = new DBHelper ();

				if (dbHelper.DoesNameExist (name)) 
				{
					ShowToast (ToastNotificationType.Info, "The property name already exists. Please choose different name.", 2);
					//await DisplayAlert ("Alert", "The property name already exists. Please choose different name.", "OK");
				}  
				else 
				{
					try{
						property.Name = name;
						dbHelper.InsertProperty (property);
						ShowToast (ToastNotificationType.Info, "Property '" + name + "' saved successfully.", 2);
						await Navigation.PopModalAsync();
					}
					catch(Exception ex) {
						ShowToast (ToastNotificationType.Info, "Oops! Some error occured. Please try again.", 2);
					}
				}
			}

		}

		async void btnDismiss_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopModalAsync();
		}

		private async void ShowToast(ToastNotificationType type, string message, int toastTime)
		{
			var notificator = DependencyService.Get<IToastNotificator>();
			bool tapped = await notificator.Notify(type, message, "", TimeSpan.FromSeconds(toastTime));
		}
	}
}

