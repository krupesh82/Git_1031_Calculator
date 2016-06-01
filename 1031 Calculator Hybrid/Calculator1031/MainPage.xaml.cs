using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Calculator1031
{
	public partial class MainPage : TabbedPage
	{
		public MainPage ()
		{
			InitializeComponent ();

			tabSavedProperties.Appearing +=	(object sender, EventArgs e) => {
				tabSavedProperties.PopulateProperties();
			};
		}
	}
}

