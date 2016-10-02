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
    internal class ConfigHandler
    {
        private string LibraryPath;

        #region Constructors
        //basic
        public ConfigHandler()
        {

        }
        //With folderpath
        public ConfigHandler(string custom_LibraryPath)
        {
            LibraryPath = custom_LibraryPath;
        }
        #endregion

        #region Properties
        internal String get(string property_name)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode character = xml.SelectSingleNode("/config/property[attribute::name='" + property_name + "']");
            return character.InnerText;
        }
        internal void set(string property_name, string property_value)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode property = xml.SelectSingleNode("/config/property[attribute::name='" + property_name + "']");
            property.InnerText = property_value;

            xml.Save(LibraryPath);
        }
        internal void add(string property_name, string property_value)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LibraryPath);
            XmlNode properties = xml.SelectSingleNode("/config");
            XmlElement property = xml.CreateElement(property_name);
            property.InnerText = property_value;
            properties.AppendChild(property);
            xml.Save(LibraryPath);
        }
        #endregion

        #region Path
        public void set_library_path(String path)
        {
            this.LibraryPath = path;
        }
        #endregion



    }
}