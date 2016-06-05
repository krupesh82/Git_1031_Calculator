using System;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;


namespace Calculator1031.Droid
{
	[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
	public class SplashActivity : AppCompatActivity
	{
		public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
		{
			base.OnCreate(savedInstanceState, persistentState);

			//global::Xamarin.Forms.Forms.Init (this, savedInstanceState);
		}

		protected override void OnResume()
		{
			base.OnResume();

			Task startupWork = new Task(() => {
				Task.Delay(2000);  // Simulate a bit of startup work.
			});

			startupWork.ContinueWith(t => {
				StartActivity(new Intent(Application.Context, typeof(MainActivity)));
			}, TaskScheduler.FromCurrentSynchronizationContext());

			startupWork.Start();
		}
	}
}