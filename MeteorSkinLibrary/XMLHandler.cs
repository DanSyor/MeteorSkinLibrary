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
                skin.name = node.GetAttribute("name");
                skin.origin = node.GetAttribute("origin");
                skin.slot = int.Parse(node.GetAttribute("slot"));

                XmlNode model = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']/Model");

                if (model.InnerText == "Imported")
                {
                    skin.model = true;
                }else
                {
                    skin.model = false;
                }

                XmlNodeList csps = xml.SelectNodes("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']/csp");
                foreach(XmlElement csp in csps)
                {
                    skin.csps.Add(csp.InnerText);
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
            XmlElement model = xml.CreateElement("Model");
            model.InnerText = "Not Found";
            xmlSkin.AppendChild(model);

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
        internal void set_skin_model(string Charname, int slot, String status)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode node = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']");
            foreach(XmlElement xe in node.ChildNodes)
            {
                if (xe.Name == "Model")
                {
                    xe.InnerText = status;
                }
            }
            xml.Save(LibraryPath);

        }

        //Add a CSP file to a skin
        internal void add_skin_csp(String Charname, int slot, String name)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlElement csp = xml.CreateElement("csp");
            csp.InnerText = name;

            XmlNode character_skin = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']");
            character_skin.AppendChild(csp);

            xml.Save(LibraryPath);

        }

        //Delete a CSP file from the skin
        internal void delete_skin_csp(String Charname, int slot, String name)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);

            XmlNodeList character_skin = xml.SelectNodes("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']/csp");
            foreach(XmlElement csp in character_skin)
            {
                if(csp.InnerText == name)
                {
                    XmlNode csps = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + Charname + "']/skin[attribute::slot='" + slot + "']");
                    csps.RemoveChild(csp);
                }
                
            }

            xml.Save(LibraryPath);
        }

        internal String get_dlc_status(String Charname)
        {
            String dlc_status ="";
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode character = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + Charname + "']");

            return character.Attributes["dlc"].Value;
                
        }
    }


}
