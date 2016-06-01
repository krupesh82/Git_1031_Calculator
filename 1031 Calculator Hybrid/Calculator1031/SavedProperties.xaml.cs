using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

using Xamarin.Forms;

namespace Calculator1031
{
	public partial class SavedProperties : ContentPage
	{
		string deleteImage = "";

		public SavedProperties ()
		{
			InitializeComponent ();

			#if __IOS__
			gridSavedProperties.RowDefinitions[0].Height = new GridLength(20);
			deleteImage = "Calculator1031.iOS.Resources.delete.jpg";
			#endif
			#if __ANDROID__
			gridSavedProperties.RowDefinitions[0].Height = new GridLength(0);
			deleteImage = "Calculator1031.Droid.Resources.delete.jpg";
			#endif
			#if WINDOWS_PHONE
			gridSavedProperties.RowDefinitions[0].Height = new GridLength(0);
			deleteImage = "Calculator1031.WinPhone.Resources.delete.jpg";
			#endif

			PopulateProperties ();
		}

		public void PopulateProperties()
		{
			Device.BeginInvokeOnMainThread (() => {
				layoutPropertyStack.Children.Clear ();
				layoutPropertyStack.Spacing = 2;

				List<Property> properties = new DBHelper ().GetAllProperties ();

				foreach (Property property in properties) {
					StackLayout hLayout = new StackLayout ();
					hLayout.Orientation = StackOrientation.Horizontal;
					hLayout.HorizontalOptions = LayoutOptions.FillAndExpand;

					StackLayout nameBox = new StackLayout();
					nameBox.BackgroundColor = Color.Transparent;
					nameBox.HorizontalOptions = LayoutOptions.FillAndExpand;
					nameBox.VerticalOptions = LayoutOptions.Fill;
					nameBox.Orientation = StackOrientation.Horizontal;

					Label propName = new Label ();
					propName.Text = property.Name;
					propName.HorizontalOptions = LayoutOptions.StartAndExpand;
					propName.VerticalOptions = LayoutOptions.Center;
					propName.FontSize = 14;

					nameBox.Children.Add(propName);

					hLayout.Children.Add (nameBox);

					Button btnDelete = new Button();
					btnDelete.Text = "X";
					btnDelete.TextColor = Color.White;
					btnDelete.FontAttributes = FontAttributes.Bold;
					btnDelete.BackgroundColor = Color.Red;
					btnDelete.HorizontalOptions = LayoutOptions.End;
					btnDelete.VerticalOptions = LayoutOptions.Center;
					btnDelete.WidthRequest = 15;
					btnDelete.HeightRequest = 15;

					hLayout.Children.Add (btnDelete);

					layoutPropertyStack.Children.Add (hLayout);

					PropertyDetails details = new PropertyDetails ();
					details.SetPropertyDetails (property);

					Grid view = details.GridView;
					view.HeightRequest = 70;
					view.HorizontalOptions = LayoutOptions.Fill;
					view.IsVisible = false;

					layoutPropertyStack.Children.Add (view);

					BoxView titleLine = new BoxView();
					titleLine.HeightRequest = 1;
					titleLine.HorizontalOptions = LayoutOptions.Fill;
					titleLine.BackgroundColor=Color.Gray;

					layoutPropertyStack.Children.Add(titleLine);

					var tapGestureOnName = new TapGestureRecognizer ();
					tapGestureOnName.Tapped += (object sender, EventArgs e) => {
						view.IsVisible = !view.IsVisible;
					};

					nameBox.GestureRecognizers.Add (tapGestureOnName);

					btnDelete.Clicked += async (object sender, EventArgs e) => {
						
						bool task = await DisplayAlert("Delete Property", "Are you sure you want to delete '' property?", "Yes", "No");

						if(task){
							DBHelper dbHelper = new DBHelper ();
							dbHelper.DeleteProperty (property.ID);
							PopulateProperties();
						}
					};
				}
			});
		}
	}
}

