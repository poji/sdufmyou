using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Utilities
{
	public class XMLIO<T>
	{
		public bool LoadFromFile(string filename, out T itemFound, out string erreur)
		{

			itemFound = default(T);
			erreur = "";

			if (!System.IO.File.Exists(filename))
			{
				erreur = string.Format("{0} n'existe pas / non trouvé !", filename);
				return false;
			}

			try
			{
				// lecture du fichier de configuration
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

		public bool LoadFromString(string content, out T itemFound, out string erreur)
		{

			itemFound = default(T);
			erreur = "";

			// lecture de la chaine à l'aide d'un 
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

		public bool SaveToFile(string filename, T item, out string erreur, bool force)
		{
			erreur = "";

			if (System.IO.File.Exists(filename) && !force)
			{
				erreur = string.Format("Le fichier {0} existe, utilisez force=true pour écraser un fichier existant", filename);
				return false;
			}

			try
			{
				// écriture dans le fichier 
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

		public bool SaveToString(T item, out string content, out string erreur)
		{

			erreur = "";
			content = "";

			// lecture de la chaine à l'aide d'un 
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

		public static bool ItemToString(T item, out string content, out string erreur)
		{

			erreur = "";
			content = "";

			// lecture de la chaine à l'aide d'un 
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

		public bool SaveToStringNoBOM(T item, out string content, out string erreur)
		{

			erreur = "";
			content = "";

			// lecture de la chaine à l'aide d'un 
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
		/// <param name="pXmlString"></param>
		/// <returns></returns>
		private static Byte[] StringToUTF8ByteArray(String pXmlString)
		{
			UTF8Encoding encoding = new UTF8Encoding();
			Byte[] byteArray = encoding.GetBytes(pXmlString);
			return byteArray;
		}

	}
}

