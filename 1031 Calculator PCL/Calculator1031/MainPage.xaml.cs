using System;
using System.Collections.Generic;
using System.Reflection;

using Xamarin.Forms;

namespace Calculator1031
{
	public partial class MainPage : TabbedPage
	{
		public MainPage ()
		{
			InitializeComponent ();

			tabSavedProperties.Appearing +=	(object sender, EventArgs e) => {
				tabSavedProperties.PopulateProperties ();
			};

			if (Device.OS == TargetPlatform.Android) 
			{
				tabCalculator.Title = "";
				tabSavedProperties.Title = "";
				tabAboutUs.Title = "";
				tabDisclaimer.Title = "";
			}
		}
	}
}

