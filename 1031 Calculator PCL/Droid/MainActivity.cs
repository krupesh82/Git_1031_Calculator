using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms;
using RoundedBoxView.Forms.Plugin.Droid;

namespace Calculator1031.Droid
{
	[Activity (Label = "Calculator1031.Droid", Icon = "@drawable/icon", Theme="@style/MyTheme.Main", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		static int doubleBackTimeInterval = 2000;
		long onBackPressed;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			RoundedBoxViewRenderer.Init ();
			LoadApplication (new App ());
		}

		public override void OnBackPressed ()
		{
			if (onBackPressed + doubleBackTimeInterval > System.DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) {
				base.OnBackPressed ();
				return;
			} else {
				Toast.MakeText (this, "Please click BACK again to exit", ToastLength.Short).Show ();
			}

			onBackPressed = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
		}
	}
}

