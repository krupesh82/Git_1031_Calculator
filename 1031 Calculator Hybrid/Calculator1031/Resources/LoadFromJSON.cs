using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Calculator1031
{
	public static class LoadFromJSON
	{

		public static DataString[] GetData(string fileName)
		{
			var assembly = typeof(LoadFromJSON).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream(fileName);

			DataString[] data;

			using (var reader = new System.IO.StreamReader(stream))
			{
				var json = reader.ReadToEnd();
				DataStrings rootobject = JsonConvert.DeserializeObject<DataStrings>(json);

				data = rootobject.Data;

				return data;
			}

		}
	}
}

