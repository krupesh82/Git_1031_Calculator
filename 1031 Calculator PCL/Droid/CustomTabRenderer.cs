using System;


using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(Calculator1031.Droid.CustomTabRenderer))]

namespace Calculator1031.Droid
{
	public class CustomTabRenderer: TabbedRenderer 
	{
		public override void OnWindowFocusChanged(bool hasWindowFocus)
		{   
			Activity _activity = this.Context as Activity;

			ActionBar actionBar = _activity.ActionBar;

			if (actionBar.TabCount == 4)
			{
				Android.App.ActionBar.Tab tabOne = actionBar.GetTabAt(0);
				tabOne.SetIcon (Resource.Drawable.calculator);

				Android.App.ActionBar.Tab tabTwo = actionBar.GetTabAt(1);
				tabTwo.SetIcon (Resource.Drawable.save);

				Android.App.ActionBar.Tab tabThree = actionBar.GetTabAt(2);
				tabThree.SetIcon (Resource.Drawable.about);

				Android.App.ActionBar.Tab tabFour = actionBar.GetTabAt(3);
				tabFour.SetIcon (Resource.Drawable.disclaimer);
			}
			base.OnWindowFocusChanged(hasWindowFocus);
		}
	}
}