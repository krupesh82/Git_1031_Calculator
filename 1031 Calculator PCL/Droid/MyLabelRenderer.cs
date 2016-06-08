using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Widget;
using Android.Graphics;

[assembly: ExportRenderer (typeof (MyLabel), typeof (MyLabelRenderer))]

namespace Calculator1031.Droid
{
	public class MyLabelRenderer : LabelRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged (e);

			var label = (TextView)Control; // for example
			Typeface font = Typeface.CreateFromAsset (Forms.Context.Assets, "Oswald-Regular.ttf");
			label.Typeface = font;
		}
	}
}