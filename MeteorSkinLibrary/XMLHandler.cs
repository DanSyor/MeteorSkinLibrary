using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MeteorSkinLibrary
{
    class XMLHandler
    {
        //Library path
        String LibraryPath = "config/Library.xml";

        public XMLHandler(String custom_Library_path)
        {
            this.LibraryPath = custom_Library_path;
        }

        public XMLHandler()
        {

        }

        //Returns an ArrayList containing character names
        public ArrayList get_character_list()
        {
            ArrayList Characters_array = new ArrayList();

            XElement xelement = XElement.Load(LibraryPath);
            IEnumerable<XElement> Characters = xelement.Elements();
            foreach (var character in Characters)
            {
                Characters_array.Add(character.Attribute("name").Value);
            }

            return Characters_array;

        }

        //Returns an ArrayList containing skins names for the ListBox.
        public ArrayList get_skin_list(String Charname)
        {
            ArrayList Skins_array = new ArrayList();

            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNodeList nodes = xml.SelectNodes("/Roaster/Character[attribute::name='" + Charname + "']/skin");
            int count = 0;

            foreach (XmlElement node in nodes)
            {
                count++;
                Skins_array.Add("Slot " + count + " - " + node.GetAttribute("name"));
            }

            return Skins_array;
        }

        //Returns a Skin item 
        public Skin get_skin(String Charname, int slot)
        {
            Skin skin = new MeteorSkinLibrary.Skin();
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNodeList nodes = xml.SelectNodes("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']");

            foreach (XmlElement node in nodes)
            {
                //Adding Skin info
                skin.name = node.GetAttribute("name");
                skin.origin = node.GetAttribute("origin");
                skin.slot = int.Parse(node.GetAttribute("slot"));

                //Adding csps to skin
                XmlNodeList csps = xml.SelectNodes("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']/csp");
                foreach (XmlElement csp in csps)
                {
                    skin.csps.Add(csp.InnerText);
                }
                //Adding models to skin
                XmlNodeList models = xml.SelectNodes("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']/model");
                foreach (XmlElement modelz in models)
                {
                    skin.models.Add(modelz.InnerText);
                }
            }

            return skin;
        }

        //Sets values to a Skin
        public void set_skin(String Charname, int slot, Skin skin)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNodeList nodes = xml.SelectNodes("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']");
            foreach (XmlElement node in nodes)
            {
                Console.WriteLine(node.GetAttribute("slot"));
                if (node.GetAttribute("slot") == skin.slot.ToString())
                {
                    node.SetAttribute("name", skin.name);
                    xml.Save(LibraryPath);
                }
            }
        }

        //Adds a Dummy Skin
        public void add_skin(String Charname, int slot)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode xmlChar = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + Charname + "']");
            XmlElement xmlSkin = xml.CreateElement("skin");
            xmlSkin.SetAttribute("slot", slot.ToString());
            xmlSkin.SetAttribute("name", "Dummy");
            xmlSkin.SetAttribute("origin", "Custom");

            xmlChar.AppendChild(xmlSkin);
            xml.Save(LibraryPath);


        }
        //Adds a Dummy Skin
        public void add_skin(String Charname, int slot,String skin_name)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode xmlChar = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + Charname + "']");
            XmlElement xmlSkin = xml.CreateElement("skin");
            xmlSkin.SetAttribute("slot", slot.ToString());
            xmlSkin.SetAttribute("name", skin_name);
            xmlSkin.SetAttribute("origin", "Custom");

            xmlChar.AppendChild(xmlSkin);
            xml.Save(LibraryPath);


        }
        //Clears Skin values
        internal void clean_skin(string Charname, int slot)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode node = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']");
            node.Attributes["origin"].Value = "EMPTY";
            node.Attributes["name"].Value = "EMPTY";
            xml.Save(LibraryPath);



        }

        //Deletes a Skin
        internal void delete_skin(string Charname, int slot)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNodeList Skins = xml.SelectNodes("/Roaster/Character[attribute::name='" + Charname + "']/skin");
            XmlNode node = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + Charname + "']");
            node.RemoveChild(node.LastChild);
            xml.Save(LibraryPath);
        }

        //Gets character folder name
        internal string get_folder_name(string Charname)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode character = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + Charname + "']");

            return character.Attributes["foldername"].Value;

        }

        //Sets the model element
        internal void add_skin_model(string Charname, int slot, String name)
        {
            Boolean exists = false;
           XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlElement model = xml.CreateElement("model");
            model.InnerText = name;
            Console.WriteLine("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']");
            XmlNode character_skin = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']");
            XmlNodeList models = xml.SelectNodes("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']/model");
            foreach(XmlElement modele in models)
            {
                if(modele.InnerText == name)
                {
                    Console.WriteLine(modele.InnerText+"-"+name);
                    exists = true;
                }
            }
            if (!exists)
            {
                character_skin.AppendChild(model);
            }
            

            xml.Save(LibraryPath);

        }

        //Add a CSP file to a skin
        internal void add_skin_csp(String Charname, int slot, String name)
        {
            Boolean exists = false;
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlElement csp = xml.CreateElement("csp");
            csp.InnerText = name;

            XmlNode character_skin = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']");
            XmlNodeList csps = xml.SelectNodes("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']/csp");
            foreach (XmlElement cspe in csps)
            {
                if (cspe.InnerText == name)
                {
                    Console.WriteLine(cspe.InnerText + "-" + name);
                    exists = true;
                }
            }
            if (!exists)
            {
                character_skin.AppendChild(csp);
            }

            xml.Save(LibraryPath);

        }

        //Delete a CSP file from the skin
        internal void delete_skin_csp(String Charname, int slot, String name)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);

            XmlNodeList character_skin = xml.SelectNodes("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']/csp");
            foreach (XmlElement csp in character_skin)
            {
                if (csp.InnerText == name)
                {
                    XmlNode csps = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']");
                    csps.RemoveChild(csp);
                }

            }

            xml.Save(LibraryPath);
        }

        internal String get_dlc_status(String Charname)
        {
            String dlc_status = "";
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode character = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + Charname + "']");

            return character.Attributes["dlc"].Value;

        }

        internal void delete_skin_all_csp(string Charname, string slot)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);

            XmlNodeList character_skin = xml.SelectNodes("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + int.Parse(slot).ToString() + "']/csp");
            foreach (XmlElement csp in character_skin)
            {
                XmlNode csps = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + int.Parse(slot).ToString() + "']");
                csps.RemoveChild(csp);
            }

            xml.Save(LibraryPath);
        }

        internal void delete_skin_all_model(string Charname, string slot)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);

            XmlNodeList character_skin = xml.SelectNodes("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + int.Parse(slot).ToString() + "']/model");
            foreach (XmlElement csp in character_skin)
            {
                XmlNode csps = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + int.Parse(slot).ToString() + "']");
                csps.RemoveChild(csp);
            }

            xml.Save(LibraryPath);
        }

        internal void delete_skin_model(string Charname, int slot, string name)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);

            XmlNodeList character_skin = xml.SelectNodes("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']/model");
            foreach (XmlElement csp in character_skin)
            {
                if (csp.InnerText == name)
                {
                    XmlNode csps = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']");
                    csps.RemoveChild(csp);
                }

            }

            xml.Save(LibraryPath);
        }

        internal String get_property(string property_name)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode character = xml.SelectSingleNode("/config/property[attribute::name='" + property_name + "']");

            return character.InnerText;
        }

        internal void set_property(string property_name,string property_value)
        {
            throw new NotImplementedException();
        }
    }


}
