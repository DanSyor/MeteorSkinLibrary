﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MeteorSkinLibrary
{
    public class LibraryHandler
    {

        #region ClassVariables
        //Library path
        String LibraryPath = "mmsl_config/Library.xml";
        #endregion

        #region Constructors

        //Constructor specifying Library path
        public LibraryHandler(String custom_Library_path)
        {
            this.LibraryPath = custom_Library_path;
        }
        //Basic Constructor
        public LibraryHandler()
        {

        }
        #endregion

        #region Characters
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

        #endregion

        #region Skins
        //Returns an ArrayList containing skins names for the ListBox.
        public ArrayList get_skin_list(String fullname)
        {
            ArrayList Skins_array = new ArrayList();

            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNodeList nodes = xml.SelectNodes("/Roaster/Character[attribute::name='" + fullname + "']/skin");
            int count = 0;

            foreach (XmlElement node in nodes)
            {
                count++;
                Skins_array.Add(node.GetAttribute("slot"));
            }

            return Skins_array;
        }

        //Sets values to a Skin
        public void set_libraryname(String fullname, int slot, String libraryname)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNodeList nodes = xml.SelectNodes("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + slot + "']");
            foreach (XmlElement node in nodes)
            {
                if (node.GetAttribute("slot") == slot.ToString())
                {
                    node.SetAttribute("name", libraryname);
                    xml.Save(LibraryPath);
                }
            }
        }

        //Adds a Dummy Skin
        public void add_skin(String fullname, int slot)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode xmlChar = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']");
            XmlElement xmlSkin = xml.CreateElement("skin");
            xmlSkin.SetAttribute("slot", slot.ToString());
            xmlSkin.SetAttribute("name", "Dummy");
            xmlSkin.SetAttribute("origin", "Custom");

            xmlChar.AppendChild(xmlSkin);
            xml.Save(LibraryPath);


        }

        //Adds a Dummy Skin with a specific Library Name
        public void add_skin(String fullname, int slot, String skin_name)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode xmlChar = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']");
            XmlElement xmlSkin = xml.CreateElement("skin");
            xmlSkin.SetAttribute("slot", slot.ToString());
            xmlSkin.SetAttribute("name", skin_name);
            xmlSkin.SetAttribute("origin", "Custom");

            xmlChar.AppendChild(xmlSkin);
            xml.Save(LibraryPath);


        }
        //Adds a Dummy Skin with a specific Library Name and origin
        public void add_skin(String fullname, int slot, String skin_name, String origin)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode xmlChar = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']");
            XmlElement xmlSkin = xml.CreateElement("skin");
            xmlSkin.SetAttribute("slot", slot.ToString());
            xmlSkin.SetAttribute("name", skin_name);
            xmlSkin.SetAttribute("origin", origin);

            xmlChar.AppendChild(xmlSkin);
            xml.Save(LibraryPath);


        }

        //Clears Skin values
        internal void clean_skin(string fullname, int slot)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode node = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + slot + "']");
            XmlNodeList models = xml.SelectNodes("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + slot + "']/model");
            XmlNodeList csps = xml.SelectNodes("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + slot + "']/csp");
            foreach (XmlElement xe in models)
            {
                node.RemoveChild(xe);
            }
            foreach (XmlElement xe in csps)
            {
                node.RemoveChild(xe);
            }


            xml.Save(LibraryPath);



        }

        //Deletes a Skin
        internal void delete_skin(string fullname, int slot)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNodeList Skins = xml.SelectNodes("/Roaster/Character[attribute::name='" + fullname + "']/skin");
            XmlNode node = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']");
            node.RemoveChild(node.LastChild);
            xml.Save(LibraryPath);
        }

        //Reloads skin order
        internal void reload_skin_order(String fullname)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNodeList nodes = xml.SelectNodes("/Roaster/Character[attribute::name='" + fullname + "']/skin");
            int i = 1;
            foreach (XmlElement node in nodes)
            {
                node.SetAttribute("slot", i.ToString());
                i++;
                    
            }
            xml.Save(LibraryPath);
        }
        //sets a moved_dlc value
        internal void set_moved_dlc_status(String fullname,String status)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode character = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']");
            character.Attributes["moved_dlc"].Value = status;
            xml.Save(LibraryPath);
        }
        #endregion

        #region Models
        //Sets the model element
        internal void add_skin_model(string fullname, int slot, String name)
        {
            Boolean exists = false;
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlElement model = xml.CreateElement("model");
            model.InnerText = name;
            XmlNode character_skin = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + slot + "']");
            XmlNodeList models = xml.SelectNodes("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + slot + "']/model");
            if (!check_skin_model(fullname, slot,name))
            {
                character_skin.AppendChild(model);
            }


            xml.Save(LibraryPath);

        }
        //Deletes a model from a skin
        internal void delete_skin_model(string fullname, int slot, string name)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);

            XmlNodeList character_skin = xml.SelectNodes("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + slot + "']/model");
            foreach (XmlElement csp in character_skin)
            {
                if (csp.InnerText == name)
                {
                    XmlNode csps = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + slot + "']");
                    csps.RemoveChild(csp);
                }

            }

            xml.Save(LibraryPath);
        }
        //Deletes all models for a skin
        internal void delete_skin_all_model(string fullname, string slot)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);

            XmlNodeList character_skin = xml.SelectNodes("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + int.Parse(slot).ToString() + "']/model");
            foreach (XmlElement csp in character_skin)
            {
                XmlNode csps = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + int.Parse(slot).ToString() + "']");
                csps.RemoveChild(csp);
            }

            xml.Save(LibraryPath);
        }
        //gets csp list
        internal ArrayList get_models(String fullname, int slot)
        {
            ArrayList models = new ArrayList();
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);

            XmlNodeList character_skin = xml.SelectNodes("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + slot + "']/model");
            foreach (XmlElement model in character_skin)
            {
                models.Add(model.InnerText);
            }


            return models;
        }
        #endregion

        #region Csp
        //Add a CSP file to a skin
        internal void add_skin_csp(String fullname, int slot, String name)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlElement csp = xml.CreateElement("csp");
            csp.InnerText = name;

            XmlNode character_skin = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + slot + "']");
            XmlNodeList csps = xml.SelectNodes("/Roaster/Character[attribute::cspname='" + fullname + "']/skin[attribute::slot='" + slot + "']/csp");
            if (!check_skin_csp(fullname,slot,name))
            {
                character_skin.AppendChild(csp);
            }
            
            xml.Save(LibraryPath);

        }

        //Delete a CSP file from the skin
        internal void delete_skin_csp(String fullname, int slot, String name)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);

            XmlNodeList character_skin = xml.SelectNodes("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + slot + "']/csp");
            foreach (XmlElement csp in character_skin)
            {
                if (csp.InnerText == name)
                {
                    XmlNode csps = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + slot + "']");
                    csps.RemoveChild(csp);
                }

            }

            xml.Save(LibraryPath);
        }

        //Deletes all CSP from a skin
        internal void delete_skin_all_csp(string fullname, string slot)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);

            XmlNodeList character_skin = xml.SelectNodes("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + int.Parse(slot).ToString() + "']/csp");
            foreach (XmlElement csp in character_skin)
            {
                XmlNode csps = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + int.Parse(slot).ToString() + "']");
                csps.RemoveChild(csp);
            }

            xml.Save(LibraryPath);
        }

        //gets csp list
        internal ArrayList get_csps(String fullname, int slot)
        {
            ArrayList csps = new ArrayList();
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);

            XmlNodeList character_skin = xml.SelectNodes("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + slot + "']/csp");
            foreach (XmlElement csp in character_skin)
            {
                csps.Add(csp.InnerText);
            }


            return csps;
        }


        #endregion

        #region Checks
        //Checks a skin's presence
        internal Boolean check_skin(string fullname, int slot)
        {
            Boolean test = false;
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);

            XmlNodeList character_skin = xml.SelectNodes("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + slot + "']");
            if (character_skin.Count != 0)
            {
                test = true;
            }
            return test;
        }
        //check a character's presence
        internal Boolean check_character(string fullname)
        {
            Boolean test = false;
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);

            XmlNodeList character_skin = xml.SelectNodes("/Roaster/Character[attribute::name='" + fullname + "']");
            if (character_skin.Count != 0)
            {
                test = true;
            }
            return test;
        }
        //Check a character's modelfolder
        internal Boolean check_character_foldername(string character_folder)
        {
            Boolean test = false;
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);

            XmlNodeList character_skin = xml.SelectNodes("/Roaster/Character[attribute::foldername='" + character_folder + "']");
            if (character_skin.Count != 0)
            {
                test = true;
            }
            return test;
        }
        //Check a character's cspfolder
        internal Boolean check_fullname_cspname(string character_folder)
        {
            Boolean test = false;
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);

            XmlNodeList character_skin = xml.SelectNodes("/Roaster/Character[attribute::cspname='" + character_folder + "']");
            if (character_skin.Count != 0)
            {
                test = true;
            }
            return test;
        }
        //checks a CSP's presence
        internal Boolean check_skin_csp(string fullname, int slot, String cspname)
        {
            Boolean test = false;
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);

            XmlNodeList character_skin = xml.SelectNodes("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + slot + "']/csp");
            if (character_skin.Count != 0)
            {
                foreach(XmlElement xe in character_skin)
                {
                    if(xe.InnerText == cspname)
                    {
                        test = true;
                    }
                }
            }
            return test;
        }
        //checks a Model's presence
        internal Boolean check_skin_model(string fullname, int slot, String modelname)
        {
            Boolean test = false;
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);

            XmlNodeList character_skin = xml.SelectNodes("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + slot + "']/model");
            if (character_skin.Count != 0)
            {
                foreach (XmlElement xe in character_skin)
                {
                    if (xe.InnerText == modelname)
                    {
                        test = true;
                    }
                }
            }
            return test;
        }
        #endregion

        #region InformationGetters
        //Gets the full name aka Captain falcon using its modelfolder aka captain
        public String get_fullname_modelfolder(String modelfolder)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode character = xml.SelectSingleNode("/Roaster/Character[attribute::foldername='" + modelfolder + "']");
            return character.Attributes["name"].Value;
        }
        //Gets the full nama aka Dr. Mario using its cspfolder aka Drmario
        public String get_fullname_cspfolder(String cspfolder)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode character = xml.SelectSingleNode("/Roaster/Character[attribute::cspname='" + cspfolder + "']");
            return character.Attributes["name"].Value;
        }
        //Gets the modelfolder using full name
        internal string get_modelfolder_fullname(string fullname)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode character = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']");

            return character.Attributes["foldername"].Value;

        }
        //Gets the cspfolder using full name
        internal string get_cspfolder_fullname(String fullname)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode character = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']");

            return character.Attributes["cspname"].Value;
        }
        //Gets the dlc status
        internal Boolean get_dlc_status(String fullname)
        {
            Boolean dlc = false;
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode character = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']");
            String dlc_string = character.Attributes["dlc"].Value;

            if(dlc_string == "1")
            {
                dlc = true;
            }
            return dlc;

        }
        //Gets the origin
        internal string get_skin_origin(String fullname, int skinslot)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode character = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + skinslot + "']");
            return  character.Attributes["origin"].Value;
        }
        //Gets the library name
        internal string get_skin_libraryname(String fullname, int skinslot)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode character = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']/skin[attribute::slot='" + skinslot + "']");
            return character.Attributes["name"].Value;
        }
        //gets the moved_dlc status
        internal Boolean get_moved_dlc_status(String fullname)
        {
            Boolean test = false;
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode character = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']");
            if(character.Attributes["moved_dlc"].Value == "1")
            {
                test = true;
            }
            return test;
        }

        internal String get_ui_char_db_id(String fullname)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode character = xml.SelectSingleNode("/Roaster/Character[attribute::name='" + fullname + "']");
            return character.Attributes["ui_char_db_id"].Value;
        }
        #endregion



    }


}
