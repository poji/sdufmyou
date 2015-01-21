using System;
using Utilities;

namespace Samples
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Demo for XMLIO");

			var xio = new XMLIO<SimpleClass> ();
			var entity = new SimpleClass ();
			entity.Name = "demo";
			entity.Occupation = "make simple demo, man !";
			entity.Quantity = 1;
			entity.IsSimpleEnough = true;

			// saving the stuff
			var filename = "demo.xml";
			if (System.IO.File.Exists (filename))
				System.IO.File.Delete (filename); // yes, i assume this will work without exception

			string xmlioError = "";
			if(!xio.SaveToFile(filename, entity, out xmlioError, true))
			{
				Console.WriteLine ("Ooops, saving this simple class made an error ! this error : " + xmlioError);
			}
			else
			{
				Console.WriteLine ("gooood, saving done !");
			}

			// loading the stuff
			var newentity = default(SimpleClass);
			if(!xio.LoadFromFile(filename, out newentity, out xmlioError))
			{
				Console.WriteLine ("Ooops, loading this simple class made an error ! this error : " + xmlioError);
			}
			else
			{
				Console.WriteLine ("gooood, loading done !");
				Console.Write ("Let see what inside ");
				Console.WriteLine (newentity.ToString ());
			}

			Console.WriteLine ("<PRESS A KEY TO QUIT>");
			Console.ReadKey ();
			Console.WriteLine ("bye....");
		}
	}
}
