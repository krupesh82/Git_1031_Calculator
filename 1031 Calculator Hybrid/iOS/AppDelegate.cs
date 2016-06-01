﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using RoundedBoxView.Forms.Plugin.iOSUnified;
namespace Calculator1031.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			RoundedBoxViewRenderer.Init ();
			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}
