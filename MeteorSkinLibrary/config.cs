using System;
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

        public config()
        {
            InitializeComponent();
            retrieve_config();
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
        }
    }



}
