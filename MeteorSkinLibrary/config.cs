﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MeteorSkinLibrary
{
    public partial class config : Form
    {
        public PropertyHandler properties = new PropertyHandler("mmsl_config/Config.xml");
        public LibraryHandler Library = new LibraryHandler("mmsl_config/Library.xml");
        public LibraryHandler Default_Library = new LibraryHandler("mmsl_config/Default_Library.xml");

        public config()
        {
            InitializeComponent();

            region_init();

            retrieve_config();

        }

        private void region_init()
        {
            regionbox.Items.Add("US");
            regionbox.Items.Add("EU");
        }

        public void open_workspace(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            String path = dialog.SelectedPath;
            if(Path.GetFileName(path) == "workspace")
            {
                textBox1.Text = path;
                textBox1.BackColor = Color.LightGreen;
                properties.add("explorer_workspace", path);
                message.Text = "Workspace path saved";

            }
            else
            {
                textBox1.Text = "Folder is not named workspace";
                textBox1.BackColor = Color.LightCoral;
            }
        }

        public void set_properties_handler(PropertyHandler config)
        {
            this.properties = config;
        }

        public void retrieve_config()
        {
            textBox1.Text = properties.get("explorer_workspace");

            if(properties.check("datafolder")){

                String locale = properties.get("datafolder");
                String language = "";
                if (locale != "data")
                {
                    locale = locale.Split('(')[1].Split(')')[0];
                    regionbox.Text = locale.Split('_')[0].ToUpper();
                    
                    switch (locale.Split('_')[1])
                    {
                        case "en":
                            language = "English";
                            break;
                        case "fr":
                            language = "French";
                            break;
                        case "gr":
                            language = "German";
                            break;
                        case "it":
                            language = "Italian";
                            break;
                        case "ne":
                            language = "Nederlands";
                            break;
                        case "po":
                            language = "Portugal";
                            break;
                        case "ru":
                            language = "Russian";
                            break;
                        case "sp":
                            language = "Spanish";
                            break;
                        default:
                            language = "";
                            break;

                    }

                }else
                {
                    language = "English";
                }
                
                
                localisationbox.Text = language;
            }
            checkBox1.Checked = Library.get_moved_dlc_status("Mewtwo");
            checkBox2.Checked = Library.get_moved_dlc_status("Lucas");
            checkBox3.Checked = Library.get_moved_dlc_status("Roy");
            checkBox4.Checked = Library.get_moved_dlc_status("Ryu");
            checkBox5.Checked = Library.get_moved_dlc_status("Cloud");
            checkBox6.Checked = Library.get_moved_dlc_status("Corrin");
            checkBox7.Checked = Library.get_moved_dlc_status("Bayonetta");

            message.Text = "";
            message.ForeColor = Color.Green;

            checkBox8.Checked = properties.get("unlocalised") == "1" ? true : false;

        }

        private void region_selected(object sender, EventArgs e)
        {
            String[] localisations_us = new String[] { "English", "French", "Spanish" };
            String[] localisations_eu = new String[] { "English", "French", "German", "Italian", "Nederlands","Portugal","Russia","Spanish"};
            localisationbox.Items.Clear();
            if (regionbox.Text == "US")
            {
                foreach(String l in localisations_us)
                {
                    
                    localisationbox.Items.Add(l);
                }
                
            }
            if (regionbox.Text == "EU")
            {
                foreach (String l in localisations_eu)
                {
                    localisationbox.Items.Add(l);
                }
            }
        }

        private void localization_selected(String locale, String input_region)
        {
            String datafolder = "data";
            String region = input_region.ToLower();
            switch (locale)
            {
                case "English":
                    if(region != "us")
                    {
                        datafolder = "data(" + region + "_en)";
                    }
                    break;
                case "French":
                    datafolder = "data(" + region + "_fr)";
                    break;
                case "German":
                    datafolder = "data(" + region + "_gr)";
                    break;
                case "Italian":
                    datafolder = "data(" + region + "_it)";
                    break;
                case "Nederlands":
                    datafolder = "data(" + region + "_ne)";
                    break;
                case "Portugal":
                    datafolder = "data(" + region + "_po)";
                    break;
                case "Russian":
                    datafolder = "data(" + region + "_ru)";
                    break;
                case "Spanish":
                    datafolder = "data(" + region + "_sp)";
                    break;
            }
            message.Text = "Data folder set to : " + datafolder;
            properties.add("datafolder",datafolder);
        }

        private void localisationbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            localization_selected(localisationbox.Text, regionbox.Text);
        }

        private void dlc_char_selected(object sender, EventArgs e)
        {
            String fullname = ((CheckBox)sender).Text;
            if (((CheckBox)sender).Checked)
            {
                Default_Library.set_moved_dlc_status(fullname, "1");
                Library.set_moved_dlc_status(fullname, "1");
                message.Text = fullname + " set to moved";
            }else
            {
                Default_Library.set_moved_dlc_status(fullname, "0");
                Library.set_moved_dlc_status(fullname, "0");
                message.Text = fullname + " set to original position";
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {

        }
    }



}
