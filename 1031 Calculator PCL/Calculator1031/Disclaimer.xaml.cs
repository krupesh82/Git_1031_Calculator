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

			if (Device.OS == TargetPlatform.iOS) 
			{
				gridDisclaimer.RowDefinitions [0].Height = new GridLength (20);
			}
			if (Device.OS == TargetPlatform.Android)
			{
				gridDisclaimer.RowDefinitions[0].Height = new GridLength(0);
				gridDisclaimer.RowDefinitions[1].Height = new GridLength(50);
			}
		}
	}
}

