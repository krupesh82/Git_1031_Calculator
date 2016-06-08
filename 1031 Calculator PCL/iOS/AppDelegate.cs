using System;
using System.Collections.Generic;
using System.Linq;

using Plugin.Toasts;
using Foundation;
using UIKit;
using RoundedBoxView.Forms.Plugin.iOSUnified;
using Xamarin.Forms;

namespace Calculator1031.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			RoundedBoxViewRenderer.Init ();

			DependencyService.Register<ToastNotificatorImplementation>();
			ToastNotificatorImplementation.Init();

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

