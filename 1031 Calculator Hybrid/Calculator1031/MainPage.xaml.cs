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
			string resourcePrefix = "";
			#if __IOS__
			resourcePrefix = "Calculator1031.iOS.Resources.";
			#endif
			#if __ANDROID__
			resourcePrefix = "Calculator1031.Droid.Resources.";
			#endif
			#if WINDOWS_PHONE
			resourcePrefix = "Calculator1031.WinPhone.Resources.";
			#endif

			tabSavedProperties.Appearing +=	(object sender, EventArgs e) => {
				tabSavedProperties.PopulateProperties();
			};

			tabCalculator.Icon = (FileImageSource)FileImageSource.FromResource (resourcePrefix + "calculator.svg");
			tabSavedProperties.Icon = (FileImageSource)FileImageSource.FromResource (resourcePrefix + "save.svg");
			tabAboutUs.Icon = (FileImageSource)FileImageSource.FromResource (resourcePrefix + "about.svg");
			tabDisclaimer.Icon = (FileImageSource)FileImageSource.FromResource (resourcePrefix + "disclaimer.svg");
		}
	}
}

