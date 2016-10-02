using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace MeteorSkinLibrary
{
    public partial class Form1 : Form
    {
        //Handlers
        XMLHandler handler = new XMLHandler();
        fileHandler filehandler = new fileHandler();

        //Current Variables

        //Selected Character
        //Name in the list
        String selected_char_name;
        //ID of selected character
        int selected_skin_slot;
        //workspace folder path
        String selected_char_folder;
        //Character is DLC
        String dlc = "";
        //Two digit representation of skin slot
        String selected_skin_slot_model;
        String selected_skin_slot_csp;

        //Lists for soft
        ArrayList Characters = new ArrayList();
        ArrayList Skins = new ArrayList();
        //Selected Files
        String[] model_file_list;
        String[] csp_file_list;

        //Selected CSP
        string selected_skin_csp_name;

        public Form1()
        {
            InitializeComponent();
            if (!File.Exists("config/Default_Library.xml"))
            {
                console_write("Default Library not found, please add Default_Library.xml in the /config folder.");
            }
            else
            {
                if (!File.Exists("config/Library.xml"))
                {
                    console_write("Creating Library...");
                    File.Copy("config/Default_Library.xml", "config/Library.xml");
                    console_write("Done.");
                    Characters = handler.get_character_list();
                    init_character_ListBox();

                }
                else
                {
                    Characters = handler.get_character_list();
                    init_character_ListBox();
                }
            }

        }

        //Menu Functions
        //Menu Config Function
        private void menu_config(object sender, EventArgs e)
        {

        }

        //Menu Exit Function
        private void menu_software_exit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Filling Character list
        public void init_character_ListBox()
        {
            foreach (String chars in Characters)
            {
                CharacterList.Items.Add(chars);
            }

        }

        //Menu Reset Library
        private void menu_reset_library(object sender, EventArgs e)
        {
            File.Delete("config/Library.xml");
            File.Copy("config/Default_Library.xml", "config/Library.xml");
            skin_ListBox_reload();
            console_write("Done. ");
        }

        //Character Actions
        //When a character is selected
        private void character_selected(object sender, EventArgs e)
        {
            this.selected_char_name = CharacterList.SelectedItem.ToString();
            this.selected_char_folder = handler.get_folder_name(this.selected_char_name);
            if (handler.get_dlc_status(selected_char_name) == "yes")
            {
                this.dlc = "append/";
            }
            else
            {
                this.dlc = "";
            }

            skin_ListBox_reload();
        }

        //Reloads Skin List
        private void skin_ListBox_reload()
        {
            SkinListBox.Items.Clear();
            Skins = handler.get_skin_list(selected_char_name);
            foreach (String skin in Skins)
            {
                SkinListBox.Items.Add(skin);
            }
        }

        //Skin Details Actions
        //When a skin is selected
        private void skin_selected(object sender, EventArgs e)
        {
            skin_details_reload();
        }


        //Save button is pressed
        private void save_skin_info(object sender, EventArgs e)
        {
            Skin current = new MeteorSkinLibrary.Skin(int.Parse(SlotNumberText.Text), SkinOriginText.Text, SkinNameText.Text, false);
            handler.set_skin(this.selected_char_name, this.selected_skin_slot + 1, current);
            skin_ListBox_reload();
        }


        //Reloads Skin Details 
        private void skin_details_reload()
        {
            skin_details_clear();
            csps_ListView.Clear();
            remove_selected_csp.Enabled = false;

            Skin skin;
            this.selected_skin_slot = SkinListBox.SelectedIndex;
            this.selected_skin_slot_csp = ((this.selected_skin_slot + 1) < 10 ? "0" + (this.selected_skin_slot + 1).ToString() : (this.selected_skin_slot + 1).ToString());
            this.selected_skin_slot_model = (this.selected_skin_slot < 10 ? "0" + this.selected_skin_slot.ToString() : this.selected_skin_slot.ToString());
            skin = handler.get_skin(this.selected_char_name, SkinListBox.SelectedIndex + 1);
            SkinOriginText.Text = skin.origin;
            SkinNameText.Text = skin.name;
            SlotNumberText.Text = skin.slot.ToString();
            if (skin.csps.Count > 0)
            {
                foreach (String csp in skin.csps)
                {
                    csps_ListView.Items.Add(csp);
                }
            }
            if (skin.model)
            {
                textBox7.Text = "Imported";
                textBox7.BackColor = Color.LightGreen;
            }

            if (skin.origin != "Default")
            {
                button3.Enabled = true;
            }
            else
            {
                button3.Enabled = false;
            }


        }

        //Clear Skin Fields
        private void skin_details_clear()
        {
            textBox5.Text = "";
            textBox5.BackColor = SystemColors.Control;
            textBox6.Text = "";
            textBox6.BackColor = SystemColors.Control;
            textBox7.Text = "";
            textBox7.BackColor = SystemColors.Control;
        }

        //Skin Menu Actions     
        //When Add Skin is pressed
        private void skin_add(object sender, EventArgs e)
        {
            handler.add_skin(this.selected_char_name, SkinListBox.Items.Count + 1);
            console_write("Skin added for "+this.selected_char_name+" in slot "+ (SkinListBox.Items.Count + 1));
            skin_ListBox_reload();
        }

        //Skin File Actions
        //When Delete is pressed
        private void skin_delete(object sender, EventArgs e)
        {
            int index = SkinListBox.SelectedIndex + 1;
            int max = SkinListBox.Items.Count;
            Skin current = handler.get_skin(this.selected_char_name, index);

            if (index < max)
            {
                handler.clean_skin(this.selected_char_name, index);
                skin_ListBox_reload();
                console_write("Deleted slot "+index);
            }
            else
            {
                handler.delete_skin(this.selected_char_name, index);
                skin_ListBox_reload();
                console_write("Deleted slot " + index);
            }
            String modelpath = "workspace/data/fighter/" + this.selected_char_folder + "/body/c" + this.selected_skin_slot_model;
            if (Directory.Exists(modelpath)) { Directory.Delete(modelpath, true); }

            String csppath = "workspace/data/ui/replace/" + dlc + "chr/ ";

            String[] files = Directory.GetFiles(csppath, "*" + this.selected_char_folder + "_" + this.selected_skin_slot_csp + ".nut", SearchOption.AllDirectories);
            foreach (String file in files)
            {
                Console.WriteLine(file);
                File.Delete(file);
            }





        }

        //Drag & Drop Handlers
        private void model_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void model_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (Directory.Exists(FileList[0]))
            {
                Regex r = new Regex("c([\\d]{2}|xx)$", RegexOptions.IgnoreCase);
                if (r.IsMatch(FileList[0]))
                {
                    this.model_file_list = FileList;
                    textBox5.Text = "Skin Detected";
                    textBox5.BackColor = Color.LightGreen;
                    button4.Enabled = true;
                }
                else
                {
                    textBox5.Text = "Skin not recognised (cXX folder)";
                    textBox5.BackColor = Color.LightCoral;
                }
            }
            else
            {
                textBox5.Text = "Item is not a Directory";
            }


        }
        private void csp_DragEnter2(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void csp_DragDrop2(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (Directory.Exists(FileList[0]))
            {
                this.csp_file_list = FileList;
                textBox6.Text = "Folder OK";
                textBox6.BackColor = Color.LightGreen;
                button5.Enabled = true;
            }
            else
            {
                textBox6.Text = "Item wasn't a Directory";
            }


        }

        //When Model Move is pressed
        private void batch_move_model(object sender, EventArgs e)
        {

            String folder = this.selected_char_folder;
            String slot = "c" + this.selected_skin_slot_model;

            String path = "workspace/data/fighter/" + folder + "/body/";
            moveModel(path, slot);

        }
        //When CSP Move is pressed
        private void batch_copy_csp(object sender, EventArgs e)
        {
            String path = csp_file_list[0];
            String filter = "*" + this.selected_char_folder + "_" + "*.nut";
            Regex csp_type_regex = new Regex("(chr_|chrn_|stock_)");
            Regex csp_number_regex = new Regex("_(\\d+)_");

            String[] files = Directory.GetFiles(path,filter, SearchOption.AllDirectories);

            foreach (String file in files)
            {
                Console.WriteLine("File Detected :" + file);
                String type = csp_type_regex.Match(file).Groups[1].ToString();
                String number = csp_number_regex.Match(file).Groups[1].ToString();
                moveCSP(file, type + number, (this.selected_skin_slot_csp).ToString());
            }
            console_write("All detected CSP were moved to slot "+this.selected_skin_slot_csp);
            skin_details_reload();
        }

        //Used to move a cXX folder to the correct location
        private void moveModel(String path, String slot)
        {
            String origin = model_file_list[0];
            String destination = path;
            String slotfolder = path + slot;

            Console.WriteLine(origin);
            Console.WriteLine(destination);
            Console.WriteLine(slotfolder);

            if (Directory.Exists(slotfolder))
            {
                Directory.Delete(slotfolder, true);
                Directory.Move(origin, slotfolder);

                console_write("Skin model added to workspace");
                handler.set_skin_model(this.selected_char_name, int.Parse(this.selected_skin_slot_csp), "Imported");
                textBox7.Text = "Imported";
                textBox7.BackColor = Color.LightGreen;
            }
            else
            {
                if (Directory.Exists(destination))
                {
                    Directory.Move(origin, slotfolder);
                    console_write("Skin model added to workspace ");
                    handler.set_skin_model(this.selected_char_name, int.Parse(this.selected_skin_slot_csp), "Imported");
                    textBox7.Text = "Imported";
                    textBox7.BackColor = Color.LightGreen;
                }
                else
                {
                    Directory.CreateDirectory(destination);
                    Directory.Move(origin, slotfolder);
                    console_write("Skin model added to workspace ");
                    handler.set_skin_model(this.selected_char_name, int.Parse(this.selected_skin_slot_csp), "Imported");
                    textBox7.Text = "Imported";
                    textBox7.BackColor = Color.LightGreen;
                }
            }


        }
        //Used to move a CSP file to the correct location
        private void moveCSP(String FilePath, String Filetype, String slot)
        {
            String destination = "workspace/data/ui/replace/" + dlc + "chr/" + Filetype + "/";
            Console.WriteLine(destination);

            String character_folder = this.selected_char_folder;
            String final_file_name = Filetype + "_" + character_folder + "_" + slot + ".nut";
            Console.WriteLine(final_file_name);
            if (!Directory.Exists("workspace/data/ui/replace/" + dlc + "chr/" + Filetype + "/"))
            {
                Directory.CreateDirectory("workspace/data/ui/replace/" + dlc + "chr/" + Filetype + "/");
            }
            String original_filename = Path.GetFileName(FilePath);
            console_write("Detected \""+original_filename+"\"");
            File.Copy(FilePath, "workspace/data/ui/replace/" + dlc + "chr/" + Filetype + "/" + final_file_name, true);
            handler.add_skin_csp(selected_char_name, int.Parse(slot), Filetype);

        }

        //When a csp is deleted
        private void remove_selected_csp_Click(object sender, EventArgs e)
        {
            String file_path = "workspace/data/ui/replace/" + dlc + "chr/" + this.selected_skin_csp_name + "/" + this.selected_skin_csp_name + "_" + this.selected_char_folder + "_" + this.selected_skin_slot_csp + ".nut";
            File.Delete(file_path);
            handler.delete_skin_csp(selected_char_name, int.Parse(selected_skin_slot_csp), this.selected_skin_csp_name);
            skin_details_reload();
        }

        //When a csp is selected
        private void csp_selected(object sender, EventArgs e)
        {
            if (csps_ListView.SelectedItems.Count == 1)
            {
                this.selected_skin_csp_name = csps_ListView.SelectedItems[0].Text;
                selected_csp_name.Text = "Selected CSP : " + csps_ListView.SelectedItems[0].Text;
                remove_selected_csp.Enabled = true;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void console_write(String s)
        {
            textConsole.Text += s+"\n";
        }

        private void resetWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists("workspace"))
            {
                Directory.Delete("workspace", true);
                Directory.CreateDirectory("workspace");
            }
            else
            {
                Directory.CreateDirectory("workspace");
            }
            console_write("Workspace folders cleaned");
        }
    }


}
