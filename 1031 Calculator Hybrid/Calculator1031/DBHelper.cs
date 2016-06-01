using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using SQLite;

namespace Calculator1031
{
	public class DBHelper
	{
	 	string DatabasePath {
			get {
				var sqliteFilename = "Properties.db3";
				#if __IOS__
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
				var path = Path.Combine(libraryPath, sqliteFilename);
				#else
				#if __ANDROID__
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				var path = Path.Combine(documentsPath, sqliteFilename);
				#else
				// WinPhone
				var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);;
				#endif
				#endif
				return path;
			}
		}

		private SQLiteConnection _connection;
		private SQLiteConnection Connection
		{
			get
			{
				if (_connection == null) 
				{
					_connection = new SQLiteConnection (this.DatabasePath, false);
				}
				return _connection;
			}
		}

		public DBHelper ()
		{
			if (!File.Exists (this.DatabasePath)) 
			{
				File.Create (this.DatabasePath);
			}
		
			this.Connection.CreateTable<Property> ();
		}

		public Property GetProperty(int id)
		{
			return this.Connection.Table<Property> ().FirstOrDefault (t => t.ID == id);
		}

		public void DeleteProperty(int id)
		{
			this.Connection.Delete<Property> (id);
		}

		public void InsertProperty(Property prop)
		{
			this.Connection.Insert (prop);
		}

		public List<Property> GetAllProperties()
		{
			return this.Connection.Table<Property> ().OrderByDescending (p => p.ID).ToList ();
		}

		public bool DoesNameExist(string propertyName)
		{
			int count = this.Connection.Table<Property> ().Count(p => p.Name == propertyName);

			if(count > 0) return true;
			else return false;
		}
	}
}

