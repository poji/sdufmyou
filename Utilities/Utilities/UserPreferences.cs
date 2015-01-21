using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Utilities
{
	public class UserPreferences<T> where T : new()
	{
		private string _appName;
		private string _defaultFileName;
		private T _preferences;

		public T Preferences { get { return _preferences;} }

		public UserPreferences (string appName = "PojiUnlimitedDevApp", string defaultFileName="userpref")
		{
			_preferences = default(T);
			_appName = appName;
			_defaultFileName = defaultFileName;
		}

		/// <summary>
		/// Load default preferences files
		/// </summary>
		/// <param name="error">if error occurs, message is put here</param>
		/// <returns>true or false (+error message)</returns>
		public bool Load(out string error)
		{
			error = "";

			var xio = new XMLIO<T>();
			var err = "";

			if(!xio.LoadFromFile(GetUserPrefFiles(), out _preferences, out err))
			{
				error=err;
			}

			return string.IsNullOrEmpty(error);
		}


		/// <summary>
		/// Save current preferences states into user file
		/// </summary>
		/// <param name="error">Error.</param>
		public bool Save(out string error)
		{
			error = "";

			var filename = this.CreateUserFile();

			var xio = new XMLIO<T>();
			var err = "";

			if (!xio.SaveToFile(filename,  _preferences, out err, true))
			{
				error = err;
			}

			return string.IsNullOrEmpty(error);
		}

		private string GetUserPrefFiles()
		{

			if (hasUserPreferenceFiles())
			{
				string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + '\\' + _appName;
				string file = path + @"\" + _defaultFileName + ".xml";
				return file;
			}

			return  @"\" + _defaultFileName + ".xml";

		}

		/// <summary>
		/// get if user have a preference view file
		/// </summary>
		/// <returns>true/false</returns>
		private bool hasUserPreferenceFiles()
		{
			string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + '\\' + _appName;
			if (!System.IO.Directory.Exists(path))
			{
				return false; // no path, no file
			}

			string file = path + @"\" + _defaultFileName + ".xml";
			if (!System.IO.File.Exists(file))
			{
				return false; // no file
			}
			return true; // file exists
		}

		/// <summary>
		/// Create user file, get the complete acces path
		/// </summary>
		/// <returns></returns>
		private string CreateUserFile()
		{
			string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + '\\' + _appName;
			if (!System.IO.Directory.Exists(path))
			{
				System.IO.Directory.CreateDirectory(path);
			}

			string file = path + @"\" + _defaultFileName + ".xml";

			if (!System.IO.File.Exists(file))
			{
				var fs = System.IO.File.Create(file); // create empty file
				if (fs != null)
				{
					fs.Close();
				}
			}
			return file;
		}


	}
}

