using System;
using Utilities;

namespace Samples
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			DemoXmlIO();
            DemoXmlConfig();
            DemoUserPreference();
			Console.WriteLine ("<PRESS A KEY TO QUIT>");
			Console.ReadKey ();
			Console.WriteLine ("bye....");
		}

		private static void DemoXmlIO()
		{
			Console.WriteLine ("Demo for XMLIO :");

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
			if(!XMLIO<SimpleClass>.SaveToFile(filename, entity, out xmlioError, true))
			{
				Console.WriteLine ("Ooops, saving this simple class made an error ! this error : " + xmlioError);
			}
			else
			{
				Console.WriteLine ("gooood, saving done !");
			}

			// loading the stuff
			var newentity = default(SimpleClass);
            if (!XMLIO<SimpleClass>.LoadFromFile(filename, out newentity, out xmlioError))
			{
				Console.WriteLine ("Ooops, loading this simple class made an error ! this error : " + xmlioError);
			}
			else
			{
				Console.WriteLine ("gooood, loading done !");
				Console.Write ("Let see what inside ");
				Console.WriteLine (newentity.ToString ());
			}
		}

        private static void DemoXmlConfig()
        {
          
            var entity = default(XmlConfig.Config);
            var xmlioError = "";

            if (!XMLIO<XmlConfig.Config>.LoadFromFile("userprefdemo.xml", out entity, out xmlioError))
            {
                // ooops, errors can occurs in real life...
                Console.WriteLine("Errors occurs when loading file userprefdemo.xml : {0}", xmlioError);
            }
            else
            {
                // now we can use the entity 
                ShowXmlConfig(entity);

            }

        }

        private static void ShowXmlConfig(XmlConfig.Config entity)
        {
            Console.WriteLine("UserPrefDemo : ");
            Console.WriteLine("\tProcessus Name : ", entity.Processus.Name);
            foreach (var demoItem in entity.Processus.DemoItems)
            {
                Console.WriteLine("\t\tDemoItem : {0} = {1}", demoItem.Name, demoItem.Content);
            }
            Console.WriteLine("\tMails : ");
            foreach (var mail in entity.Mails)
            {
                Console.WriteLine("\t\t{0} : Enabled {1} / DebugOnly {2}", mail.Address, mail.Enabled, mail.DebugOnly);
            }
        }

        private static void DemoUserPreference()
        {
            var error = "";
            var userPreferences = new UserPreferences<XmlConfig.Config>(defaultFileName: "userprefdemo");
            if (!userPreferences.Load(out error))
            {
                Console.WriteLine("Loading error : {0}", error);
                return;
            }

            ShowXmlConfig(userPreferences.Preferences);

            // modifying it
            userPreferences.Preferences.Processus.Name = "demotest";
            userPreferences.Preferences.Mails[0].Enabled = false;
            if (!userPreferences.Save(out error))
            {
                Console.WriteLine("Saving error : {0}", error);
                return;
            }
            else
            {
                Console.WriteLine("You shoud have a new file in %USER DOCUMENT%/PojiUnlimitedDevApp/userprefdemo.xml");
            }
        }
    
    }
}
