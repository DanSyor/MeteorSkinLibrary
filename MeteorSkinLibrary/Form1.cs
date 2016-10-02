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
        String[] model_folder_list;
        String[] csp_file_list;
        String[] slot_file_list;

        //destination
        String csp_destination;
        String model_destination;

        //Selected CSP
        string selected_skin_csp_name;
        //Selected Model
        private string selected_skin_model_name;

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
            console_write("Library reset complete");
        }

        //Character Actions
        //When a character is selected
        private void character_selected(object sender, EventArgs e)
        {
            this.selected_char_name = CharacterList.SelectedItem.ToString();
            this.selected_char_folder = handler.get_folder_name(this.selected_char_name);
            this.model_destination = "workspace/data/fighter/" + this.selected_char_folder + "/";

            if (handler.get_dlc_status(selected_char_name) == "yes")
            {
                this.csp_destination = "workspace/data/ui/replace/append/chr/";
            }
            else
            {
                this.csp_destination = "workspace/data/ui/replace/chr/";
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
            csps_ListView.Clear();
            models_ListView.Clear();
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
            if (skin.models.Count > 0)
            {
                foreach (String model in skin.models)
                {
                    models_ListView.Items.Add(model);
                }
            }

            if (skin.origin != "Default")
            {
                button3.Enabled = true;
            }
            else
            {
                button3.Enabled = false;
            }

            button2.Enabled = true;


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
            this.model_folder_list = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            batch_move_model();
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
                batch_copy_csp();
            }
            else
            {
                //textBox6.Text = "Item wasn't a Directory";
            }


        }
        private void slot_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void slot_DragDrop(object sender, DragEventArgs e)
        {
            this.slot_file_list = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            batch_add_slot();
        }

        //When Model is added
        private void batch_move_model()
        {

            String slot = this.selected_skin_slot_model;
            foreach(String folder in this.model_folder_list)
            {
                Console.WriteLine("Folder is : " + folder);
                String type = "";
                String currentfolder = Path.GetFileName(folder);
                Regex cxx = new Regex("^[c](xx|[0-9]{1,3}$)",RegexOptions.IgnoreCase);
                Regex lxx = new Regex("^[l](xx|[0-9]{1,3}$)", RegexOptions.IgnoreCase);
                if (cxx.IsMatch(currentfolder))
                {
                    type = "cxx";
                }
                if (lxx.IsMatch(currentfolder))
                {
                    type = "lxx";
                }

                if (currentfolder == "body")
                {
                    type = "body";
                }
                
                switch (type)
                {
                    case "body":
                        break;
                    case "cxx":
                        copyModel(folder, this.selected_skin_slot_model, "cxx");
                        break;
                    case "lxx":
                        copyModel(folder, this.selected_skin_slot_model, "lxx");
                        break;
                    default:
                        break;
                }
                skin_details_reload();
            }

        }
        private void batch_move_model(String[] filelist,int i_slot)
        {

            String slot = (i_slot-1) < 10 ? "0" + (i_slot - 1).ToString() : (i_slot - 1).ToString();
            foreach (String folder in filelist)
            {
                Console.WriteLine("Folder is : " + folder);
                String type = "";
                String currentfolder = Path.GetFileName(folder);
                Regex cxx = new Regex("^[c](xx|[0-9]{1,3}$)", RegexOptions.IgnoreCase);
                Regex lxx = new Regex("^[l](xx|[0-9]{1,3}$)", RegexOptions.IgnoreCase);
                if (cxx.IsMatch(currentfolder))
                {
                    type = "cxx";
                }
                if (lxx.IsMatch(currentfolder))
                {
                    type = "lxx";
                }

                if (currentfolder == "body")
                {
                    type = "body";
                }

                switch (type)
                {
                    case "body":
                        break;
                    case "cxx":
                        copyModel(folder, slot, "cxx");
                        break;
                    case "lxx":
                        copyModel(folder, slot, "lxx");
                        break;
                    default:
                        break;
                }
                skin_details_reload();
            }

        }
        //When CSP is added
        private void batch_copy_csp()
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
                copyCSP(file, type + number, (this.selected_skin_slot_csp).ToString());
            }
            console_write("All detected CSP were moved to slot "+this.selected_skin_slot_csp);
            skin_details_reload();
        }
        private void batch_copy_csp(String[] filelist,int i_slot)
        {
            if(filelist.Length != 0)
            {
                String slot = (i_slot ) < 10 ? "0" + (i_slot ).ToString() : (i_slot ).ToString();
                String path = filelist[0];

                String filter = "*.nut";

                Regex csp_type_regex = new Regex("(chr_|chrn_|stock_)");
                Regex csp_number_regex = new Regex("_([0-9]{1,3})_");

                String[] files = Directory.GetFiles(path, filter, SearchOption.AllDirectories);

                foreach (String file in files)
                {
                    console_write("File Detected :" + Path.GetFileName(file));
                    String type = Path.GetFileName(file).Split('_')[0];
                    String number = Path.GetFileName(file).Split('_')[1];
                    copyCSP(file, type + number, slot);
                }
                console_write("All detected CSP were moved to slot " + i_slot);
            }
            else
            {
                console_write("no csp detected " + i_slot);
            }
            
            skin_details_reload();
        }
        //When Slot is added
        private void batch_add_slot()
        {
            Regex slotname = new Regex("(slot_)([0-9]{1,3})(_)(p*)");
            foreach(String file in this.slot_file_list)
            {
                if (slotname.IsMatch(Path.GetFileName(file)))
                {
                    skin_ListBox_reload();
                    console_write("Slot Detected : "+ Path.GetFileName(file));
                    String skin_name = Path.GetFileName(file).Split('_')[2];
                    if(skin_name == "")
                    {
                        skin_name = "empty";
                    }

                    int skin_slot = SkinListBox.Items.Count + 1;

                    handler.add_skin(this.selected_char_name, skin_slot, skin_name);

                    //Model files check
                    if (Directory.Exists(file + "/model"))
                    {
                        console_write("Slot model folder detected");
                        batch_move_model(Directory.GetDirectories(file + "/model"), skin_slot);
                    }
                    else
                    {
                        console_write("Slot model folder missing");
                    }
                    //CSP Files check
                    if (Directory.Exists(file + "/csp/"))
                    {
                        console_write("Slot csp folder detected");
                        String[] folder = new string[] {file + "/csp/" };
                        batch_copy_csp(folder, skin_slot);
                    }
                    else
                    {
                        console_write("Slot csp folder missing");
                    }
                }
            }
            skin_ListBox_reload();
            SkinListBox.SelectedIndex = (SkinListBox.Items.Count - 1);
            skin_details_reload();
        }

        //Used to move a cXX folder to the correct location
        private void copyModel(String path, String slot, String type)
        {
            if (!Directory.Exists(this.model_destination))
            {
                Directory.CreateDirectory(this.model_destination);
            }

            String model = Path.GetDirectoryName(path);
            String modelpath = this.model_destination;

            switch (type)
            {
                case "cxx":
                    modelpath += "body/c"+slot;
                    if (!Directory.Exists(this.model_destination + "/body")) { Directory.CreateDirectory(this.model_destination + "/body"); }
                    break;
                case "lxx":
                    modelpath += "body/l" + slot;
                    if (!Directory.Exists(this.model_destination + "/body")) { Directory.CreateDirectory(this.model_destination + "/body"); }
                    break;
                default:
                    modelpath += "" + slot;
                    if (!Directory.Exists(this.model_destination + "/body")) { Directory.CreateDirectory(this.model_destination + "/body"); }
                    break;
            }
            
            if (Directory.Exists(modelpath))
            {
                String[] previousfiles = Directory.GetFiles(modelpath);
                foreach (String file in previousfiles)
                {
                    File.Delete(file);
                }

                String[] files = Directory.GetFiles(path);
                foreach(String file in files)
                {
                    String filepath = modelpath + "/" + Path.GetFileName(file);
                    File.Copy(file, filepath,true);
                }
                handler.add_skin_model(this.selected_char_name, int.Parse(slot)+1, type);
            }
            else
            {
                Directory.CreateDirectory(modelpath);
                String[] files = Directory.GetFiles(path);
                foreach (String file in files)
                {
                    String filepath = modelpath + "/" + Path.GetFileName(file);
                    File.Copy(file, filepath, true);
                }
                handler.add_skin_model(this.selected_char_name, int.Parse(slot)+1, type);
            }

            console_write("Moved to \"" + modelpath + "\"");
        }

        //Used to move a CSP file to the correct location
        private void copyCSP(String FilePath, String Filetype, String slot)
        {
            String destination = "workspace/data/ui/replace/" + dlc + "chr/" + Filetype + "/";
            Console.WriteLine(destination);

            String character_folder = this.selected_char_folder;
            String final_file_name = Filetype + "_" + character_folder + "_" + slot + ".nut";
            Console.WriteLine("Copied : "+final_file_name);
            if (!Directory.Exists(this.csp_destination+"/" + Filetype + "/"))
            {
                Directory.CreateDirectory(this.csp_destination + Filetype + "/");
            }
            String original_filename = Path.GetFileName(FilePath);
            File.Copy(FilePath, this.csp_destination + "/" + Filetype + "/" + final_file_name, true);
            handler.add_skin_csp(this.selected_char_name, int.Parse(slot), Filetype);

        }

        //Used to move a slot to the correct location
        private void copySlot()
        {

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

        private void clean_files_clicked(object sender, EventArgs e)
        {
            int index = SkinListBox.SelectedIndex + 1;
            int max = SkinListBox.Items.Count;
            Skin current = handler.get_skin(this.selected_char_name, index);

            String modelpath = this.model_destination + "/body/c" + this.selected_skin_slot_model;
            if (Directory.Exists(modelpath)) { Directory.Delete(modelpath, true); }

            
                handler.delete_skin_all_csp(this.selected_char_name, this.selected_skin_slot_csp);
                handler.delete_skin_all_model(this.selected_char_name, this.selected_skin_slot_csp);

                String[] files = Directory.GetFiles(this.csp_destination, "*" + this.selected_char_folder + "_" + this.selected_skin_slot_csp + ".nut", SearchOption.AllDirectories);
                foreach (String file in files)
                {
                    File.Delete(file);
                    
                }

            String[] modelfiles = Directory.GetFiles(this.model_destination, "*", SearchOption.AllDirectories);
            foreach (String file in modelfiles)
            {
                Console.WriteLine(file);
                File.Delete(file);

            }

            skin_details_reload();
            
        }

        private void console_write(String s)
        {
            textConsole.Text += s+"\n";
        }

        private void resetWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists("workspace"))
            {
                String[] files = Directory.GetFiles("workspace","*",SearchOption.AllDirectories);
                foreach(String file in files)
                {
                    File.Delete(file);
                }
                Directory.Delete("workspace", true);
                Directory.CreateDirectory("workspace");
            }
            else
            {
                Directory.CreateDirectory("workspace");
            }
            if (!Directory.Exists("workspace"))
            {
                Directory.CreateDirectory("workspace");
            }
            console_write("Workspace reset complete");
        }

        private void model_selected(object sender, EventArgs e)
        {
            if (models_ListView.SelectedItems.Count == 1)
            {
                this.selected_skin_model_name = models_ListView.SelectedItems[0].Text;
                label5.Text = "Selected Model : " + models_ListView.SelectedItems[0].Text;
                button6.Enabled = true;
            }
        }

        private void remove_selected_model_Click(object sender, EventArgs e)
        {
            //File.Delete("");
            handler.delete_skin_model(selected_char_name, int.Parse(selected_skin_slot_model), this.selected_skin_model_name);
            skin_details_reload();
        }

        private void openWorkspace(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("workspace");
        }
    }


}
