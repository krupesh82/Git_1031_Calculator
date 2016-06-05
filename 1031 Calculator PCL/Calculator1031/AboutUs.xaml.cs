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

			if (Device.OS == TargetPlatform.iOS) 
			{
				gridAboutUs.RowDefinitions[0].Height = new GridLength(20);
			}
			if (Device.OS == TargetPlatform.Android) 
			{
				gridAboutUs.RowDefinitions [0].Height = new GridLength (0);
				gridAboutUs.RowDefinitions [1].Height = new GridLength (50);
			}

			img01.Source = ImageSource.FromResource("Calculator1031.Resources.down.png");
			img01.Aspect = Aspect.AspectFit;
			img02.Source = ImageSource.FromResource("Calculator1031.Resources.down.png");
			img02.Aspect = Aspect.AspectFit;
			img03.Source = ImageSource.FromResource("Calculator1031.Resources.down.png");
			img03.Aspect = Aspect.AspectFit;
			img04.Source = ImageSource.FromResource("Calculator1031.Resources.down.png");
			img04.Aspect = Aspect.AspectFit;
			img05.Source = ImageSource.FromResource("Calculator1031.Resources.down.png");
			img05.Aspect = Aspect.AspectFit;
			img06.Source = ImageSource.FromResource("Calculator1031.Resources.down.png");
			img06.Aspect = Aspect.AspectFit;

			var tapGestureQ1 = new TapGestureRecognizer ();
			tapGestureQ1.Tapped += (object sender, EventArgs e) => {
				answer01.IsVisible = !answer01.IsVisible;

				if(answer01.IsVisible)
					img01.Source = ImageSource.FromResource("Calculator1031.Resources.up.png");
				else
					img01.Source = ImageSource.FromResource("Calculator1031.Resources.down.png");
			};

			question01.GestureRecognizers.Add (tapGestureQ1);

			var tapGestureQ2 = new TapGestureRecognizer ();
			tapGestureQ2.Tapped += (object sender, EventArgs e) => {
				answer02.IsVisible = !answer02.IsVisible;

				if(answer02.IsVisible)
					img02.Source = ImageSource.FromResource("Calculator1031.Resources.up.png");
				else
					img02.Source = ImageSource.FromResource("Calculator1031.Resources.down.png");
			};

			question02.GestureRecognizers.Add (tapGestureQ2);

			var tapGestureQ3 = new TapGestureRecognizer ();
			tapGestureQ3.Tapped += (object sender, EventArgs e) => {
				answer03.IsVisible = !answer03.IsVisible;

				if(answer03.IsVisible)
					img03.Source = ImageSource.FromResource("Calculator1031.Resources.up.png");
				else
					img03.Source = ImageSource.FromResource("Calculator1031.Resources.down.png");
			};

			question03.GestureRecognizers.Add (tapGestureQ3);

			var tapGestureQ4 = new TapGestureRecognizer ();
			tapGestureQ4.Tapped += (object sender, EventArgs e) => {
				answer04.IsVisible = !answer04.IsVisible;

				if(answer04.IsVisible)
					img04.Source = ImageSource.FromResource("Calculator1031.Resources.up.png");
				else
					img04.Source = ImageSource.FromResource("Calculator1031.Resources.down.png");
			};

			question04.GestureRecognizers.Add (tapGestureQ4);

			var tapGestureQ5 = new TapGestureRecognizer ();
			tapGestureQ5.Tapped += (object sender, EventArgs e) => {
				answer05.IsVisible = !answer05.IsVisible;

				if(answer05.IsVisible)
					img05.Source = ImageSource.FromResource("Calculator1031.Resources.up.png");
				else
					img05.Source = ImageSource.FromResource("Calculator1031.Resources.down.png");
			};

			question05.GestureRecognizers.Add (tapGestureQ5);

			var tapGestureQ6 = new TapGestureRecognizer ();
			tapGestureQ6.Tapped += (object sender, EventArgs e) => {
				answer06.IsVisible = !answer06.IsVisible;

				if(answer06.IsVisible)
					img06.Source = ImageSource.FromResource("Calculator1031.Resources.up.png");
				else
					img06.Source = ImageSource.FromResource("Calculator1031.Resources.down.png");
			};

			question06.GestureRecognizers.Add (tapGestureQ6);
		}
	}
}

