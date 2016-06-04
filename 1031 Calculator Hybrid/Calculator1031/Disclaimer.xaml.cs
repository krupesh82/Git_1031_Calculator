using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Calculator1031
{
	public partial class Disclaimer : ContentPage
	{
		public Disclaimer ()
		{
			InitializeComponent ();

			#if __IOS__
			gridDisclaimer.RowDefinitions[0].Height = new GridLength(20);
			#endif
			#if __ANDROID__
			gridDisclaimer.RowDefinitions[0].Height = new GridLength(0);
			gridDisclaimer.RowDefinitions[1].Height = new GridLength(50);
			#endif
			#if WINDOWS_PHONE
			gridDisclaimer.RowDefinitions[0].Height = new GridLength(0);
			#endif
		}
	}
}

