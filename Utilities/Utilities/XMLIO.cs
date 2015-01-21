using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Utilities
{
    /// <summary>
    /// Generic class for loading/saving class from/to xml files
    /// </summary>
    /// <typeparam name="T">generic type to load/save</typeparam>
	public static class XMLIO<T>
	{
        /// <summary>
        /// Try to load a class from an xml file
        /// </summary>
        /// <param name="filename">full filename to load, must be utf8 file</param>
        /// <param name="itemFound">the instance of T to return</param>
        /// <param name="erreur">error that have occured, if any</param>
        /// <returns>true mean ok, false mean not ok (obviously)</returns>
		public static bool LoadFromFile(string filename, out T itemFound, out string erreur)
		{

			itemFound = default(T);
			erreur = "";

			if (!System.IO.File.Exists(filename))
			{
				erreur = string.Format("{0} doesn't exist / not found !", filename);
				return false;
			}

			try
			{
				FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
				XmlSerializer xs = new XmlSerializer(typeof(T));
				itemFound = (T)xs.Deserialize(fs);
				fs.Close();
			}
			catch (Exception ex)
			{
				erreur = ex.Message;
				if (ex.InnerException != null)
					erreur += "\n" + ex.InnerException.Message;
				itemFound = default(T);
				return false;
			}

			return true;

		}

        /// <summary>
        /// Try to interprete a string as an xml that could create an T instance
        /// </summary>
        /// <param name="content">the string, must be UTF8, no-bom</param>
        /// <param name="itemFound">the instance of T to return</param>
        /// <param name="erreur">error if any occurs</param>
        /// <returns>true mean ok, false mean not ok (obviously, too)</returns>
		public static bool LoadFromString(string content, out T itemFound, out string erreur)
		{

			itemFound = default(T);
			erreur = "";

			
			try
			{
				XmlSerializer xs = new XmlSerializer(typeof(T));
				MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(content));
				XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
				itemFound = (T)xs.Deserialize(memoryStream);

			}
			catch (Exception ex)
			{
				erreur = ex.Message;
				itemFound = default(T);
				return false;
			}

			return true;
		}


        /// <summary>
        /// Save a class into a xml file
        /// </summary>
        /// <param name="filename">full filename to create, you must have permission to write into the path</param>
        /// <param name="item">class to convert into xml</param>
        /// <param name="erreur">if an error occurs</param>
        /// <param name="force">force trying to overwrite existing file</param>
        /// <returns>true, false (man, it's obvious)</returns>
		public static bool SaveToFile(string filename, T item, out string erreur, bool force)
		{
			erreur = "";

			if (System.IO.File.Exists(filename) && !force)
			{
				erreur = string.Format("File {0} exist ! use param force=true for overwrite existing files", filename);
				return false;
			}

			try
			{
				FileStream fs = new FileStream(filename, FileMode.Create);
				XmlSerializer xs = new XmlSerializer(typeof(T));
				var sw = new StreamWriter(fs);
				xs.Serialize(sw, item);
				fs.Flush();
				fs.Close();
			}
			catch (Exception ex)
			{
				erreur = ex.Message;
				return false;
			}


			return true;

		}

        /// <summary>
        /// save a class into a xml string, for doing things... later... or not... do what you want... feel free
        /// </summary>
        /// <param name="item">instance of class T to convert</param>
        /// <param name="content">a string that would contain the xml code</param>
        /// <param name="erreur">oh, see that ! an wild error could appear here !</param>
        /// <returns>really, i have to say ?</returns>
		public static bool SaveToString(T item, out string content, out string erreur)
		{

			erreur = "";
			content = "";

			try
			{
				String XmlizedString = null;
				MemoryStream memoryStream = new MemoryStream();
				XmlSerializer xs = new XmlSerializer(typeof(T));
				XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

				xs.Serialize(xmlTextWriter, item);
				memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
				XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
				content = XmlizedString;
			}
			catch (Exception ex)
			{
				erreur = ex.Message;
				return false;
			}


			return true;
		}

        #region Code i borrow from forums, doesn't retreive theyre creators


        /// <summary>
        /// Save a class into a string, stripping bom
        /// </summary>
        /// <param name="item">class to convert</param>
        /// <param name="content">a string what would receive the xml</param>
        /// <param name="erreur">if an error occurs</param>
        /// <returns>true/false, obvious</returns>
		public static bool SaveToStringNoBOM(T item, out string content, out string erreur)
		{

			erreur = "";
			content = "";

			try
			{
				String XmlizedString = null;
				MemoryStream memoryStream = new MemoryStream();
				XmlSerializer xs = new XmlSerializer(typeof(T));
				var utf8WithoutBom = new System.Text.UTF8Encoding(false);
				XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, utf8WithoutBom);

				xs.Serialize(xmlTextWriter, item);
				memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
				XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
				content = XmlizedString;
			}
			catch (Exception ex)
			{
				erreur = ex.Message;
				return false;
			}


			return true;
		}

		/// <summary>
		/// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
		/// </summary>
		/// <param name="characters">Unicode Byte Array to be converted to String</param>
		/// <returns>String converted from Unicode Byte Array</returns>
		private static String UTF8ByteArrayToString(Byte[] characters)
		{
			UTF8Encoding encoding = new UTF8Encoding();
			String constructedString = encoding.GetString(characters);
			return (constructedString);
		}

		/// <summary>
		/// Converts the String to UTF8 Byte array and is used in De serialization
		/// </summary>
		/// <param name="pXmlString">string to convert</param>
		/// <returns>Byte arrays</returns>
		private static Byte[] StringToUTF8ByteArray(String pXmlString)
		{
			UTF8Encoding encoding = new UTF8Encoding();
			Byte[] byteArray = encoding.GetBytes(pXmlString);
			return byteArray;
        }

        #endregion

    }
}

