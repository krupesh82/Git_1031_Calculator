 	using System;
using Calculator1031;
using Xamarin.Forms;
using Calculator1031.iOS;
using System.IO;

using SQLite;

[assembly: Dependency (typeof (SQLite_iOS))]

namespace Calculator1031.iOS
{
	public class SQLite_iOS : ISQLite
	{
		public SQLite_iOS ()
		{
		}

		#region ISQLite implementation
		public SQLite.SQLiteConnection GetConnection ()
		{
			var sqliteFilename = "Properties.db3";
			string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
			string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
			var path = Path.Combine(libraryPath, sqliteFilename);

			// This is where we copy in the prepopulated database
			if (!File.Exists (path)) {
				File.Create (path);
			}

			var conn = new SQLite.SQLiteConnection(path);

			// Return the database connection 
			return conn;
		}
		#endregion
	}
}
