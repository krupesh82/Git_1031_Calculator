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
			//string resourcePrefix = "";
			//#if __IOS__
			//resourcePrefix = "Calculator1031.iOS.Resources.";
			//tabCalculator.Icon = ImageSource.FromResource (resourcePrefix + "calculator.svg");
			//tabSavedProperties.Icon = ImageSource.FromResource (resourcePrefix + "save.svg");
			//tabAboutUs.Icon = ImageSource.FromResource (resourcePrefix + "about.svg");
			//tabDisclaimer.Icon = ImageSource.FromResource (resourcePrefix + "disclaimer.svg");
			//#endif
//			#if __ANDROID__
//			resourcePrefix = "Calculator1031.Droid.Resources.";
//			#endif
//			#if WINDOWS_PHONE
//			resourcePrefix = "Calculator1031.WinPhone.Resources.";
//			#endif

			tabSavedProperties.Appearing +=	(object sender, EventArgs e) => {
				tabSavedProperties.PopulateProperties();
			};
		}
	}
}

