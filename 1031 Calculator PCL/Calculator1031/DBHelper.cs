using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using SQLite;
using Xamarin.Forms;

namespace Calculator1031
{
	public interface ISQLite {
		SQLiteConnection GetConnection();
	}

	public class DBHelper
	{
		private SQLiteConnection _connection;
		private SQLiteConnection Connection
		{
			get
			{
				if (_connection == null) 
				{
					_connection = DependencyService.Get<ISQLite> ().GetConnection ();
				}
				return _connection;
			}
		}

		public DBHelper ()
		{
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

		public List<Property> GetAllNewProperties(int propId)
		{
			return this.Connection.Query<Property> ("SELECT * FROM [Property] WHERE [ID] > " + propId.ToString()).OrderBy (p => p.ID).ToList ();
		}

		public bool DoesNameExist(string propertyName)
		{
			int count = this.Connection.Table<Property> ().Count(p => p.Name == propertyName);

			if(count > 0) return true;
			else return false;
		}
	}
}

