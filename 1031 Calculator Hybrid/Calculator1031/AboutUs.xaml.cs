using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Calculator1031
{
	public partial class AboutUs : ContentPage
	{
		public AboutUs ()
		{
			InitializeComponent ();

			#if __IOS__
			gridAboutUs.RowDefinitions[0].Height = new GridLength(20);
			#endif
			#if __ANDROID__
			gridAboutUs.RowDefinitions[0].Height = new GridLength(0);
			gridAboutUs.RowDefinitions[1].Height = new GridLength(50);
			#endif
			#if WINDOWS_PHONE
			gridAboutUs.RowDefinitions[0].Height = new GridLength(0);
			#endif

			var tapGestureQ1 = new TapGestureRecognizer ();
			tapGestureQ1.Tapped += (object sender, EventArgs e) => {
				answer01.IsVisible = !answer01.IsVisible;
			};

			question01.GestureRecognizers.Add (tapGestureQ1);

			var tapGestureQ2 = new TapGestureRecognizer ();
			tapGestureQ2.Tapped += (object sender, EventArgs e) => {
				answer02.IsVisible = !answer02.IsVisible;
			};

			question02.GestureRecognizers.Add (tapGestureQ2);

			var tapGestureQ3 = new TapGestureRecognizer ();
			tapGestureQ3.Tapped += (object sender, EventArgs e) => {
				answer03.IsVisible = !answer03.IsVisible;
			};

			question03.GestureRecognizers.Add (tapGestureQ3);

			var tapGestureQ4 = new TapGestureRecognizer ();
			tapGestureQ4.Tapped += (object sender, EventArgs e) => {
				answer04.IsVisible = !answer04.IsVisible;
			};

			question04.GestureRecognizers.Add (tapGestureQ4);

			var tapGestureQ5 = new TapGestureRecognizer ();
			tapGestureQ5.Tapped += (object sender, EventArgs e) => {
				answer05.IsVisible = !answer05.IsVisible;
			};

			question05.GestureRecognizers.Add (tapGestureQ5);

			var tapGestureQ6 = new TapGestureRecognizer ();
			tapGestureQ6.Tapped += (object sender, EventArgs e) => {
				answer06.IsVisible = !answer06.IsVisible;
			};

			question06.GestureRecognizers.Add (tapGestureQ6);
		}
	}
}

