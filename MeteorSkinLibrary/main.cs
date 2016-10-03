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
    public partial class main : Form
    {
        //Variables
        #region Handlers
        LibraryHandler handler;
        PropertyHandler properties = new PropertyHandler("config/Default_Config.xml");
        MetaHandler meta = new MetaHandler("config/meta/Default_Meta.xml");
        #endregion
        #region SelectedInfo
        //Name in the list
        String selected_char_name;
        //workspace folder name
        String selected_char_folder;
        //Two digit representation of skin slot
        String selected_skin_slot_model;
        String selected_skin_slot_csp;
        //ID of selected skin slot
        int selected_skin_slot;
        //Selected CSP
        string selected_skin_csp_name;
        //Selected Model
        private string selected_skin_model_name;
        //Character is DLC
        String dlc = "";
        #endregion
        #region Lists
        //Lists for soft
        ArrayList Characters = new ArrayList();
        ArrayList Skins = new ArrayList();
        #endregion
        #region Files
        //Selected Files
        String[] model_folder_list;
        String[] csp_file_list;
        String[] slot_file_list;
        //destination
        String csp_destination;
        String model_destination;
        String meta_destination;
        #endregion

        public main()
        {
            InitializeComponent();
            //Checks Default_Library.xml presence
            if (!File.Exists("config/Default_Library.xml"))
            {
                console_write("Default Library not found, please add Default_Library.xml in the /config folder.");
            }
            if (!File.Exists("config/Default_Config.xml"))
            {
                console_write("Default Config not found, please add Default_Config.xml in the /config folder.");
            }
            else
            {
                //Checks Config.xml presence, if not creates one based on Default_Config.xml
                if (!File.Exists("config/Config.xml"))
                {
                    console_write("Creating Config");
                    File.Copy(properties.get("default_config"), "config/Config.xml");
                }
                    properties.set_library_path("config/Config.xml");
                console_write("Config loaded : config/Config.xml");

                //Checks Library.xml presence, if not creates one based on Default_Library.xml
                if (!File.Exists("config/Library.xml"))
                {
                    console_write("Creating Library");
                    File.Copy(properties.get("default_library"), "config/Library.xml");
                    properties.add("current_library", "config/Library.xml");
                }

                
                handler = new LibraryHandler(properties.get("current_library"));
                console_write("Library loaded : "+properties.get("current_library"));

                //Loads Character List
                Characters = handler.get_character_list();
                init_character_ListBox();
                state_check();
            }

        }

        //Functions
        #region Menu
        #region FileMenu
        //Menu Exit Function
        private void menu_software_exit(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //Open Workspace Function
        private void openWorkspace(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("workspace");
        }
        #endregion
        #region SkinMenu
        //When Add Skin is pressed
        private void skin_add(object sender, EventArgs e)
        {
            handler.add_skin(this.selected_char_name, SkinListBox.Items.Count + 1);
            console_write("Skin added for " + this.selected_char_name + " in slot " + (SkinListBox.Items.Count + 1));
            skin_ListBox_reload();
            state_check();
        }
        #endregion
        #region OptionMenu
        //Menu Config Function
        public void menu_config(object sender, EventArgs e)
        {
            config cnf = new config();
            
            cnf.Show();
            state_check();
        }
        //Menu Reset Library
        private void menu_reset_library(object sender, EventArgs e)
        {
            if (MessageBox.Show("Doing this will erase all entries in the Library. Skins are still present in the workspace folder. Continue with this destruction?", "Segtendo WARNING", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                File.Delete("config/Library.xml");
                File.Copy("config/Default_Library.xml", "config/Library.xml");

                state_check();
                console_write("Library reset complete");
            }

        }
        //Workspace reset button
        private void reset_workspace(object sender, EventArgs e)
        {
            if (MessageBox.Show("Doing this will erase all contents of the workspace folder which contains every file you've added. Continue with this destruction?", "Segtendo WARNING", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (Directory.Exists("workspace"))
                {
                    String[] files = Directory.GetFiles("workspace", "*", SearchOption.AllDirectories);
                    foreach (String file in files)
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
            state_check();
        }
        //Config Reset button
        private void reset_config(object sender, EventArgs e)
        {
            if (MessageBox.Show("Doing this will erase all configuration changes. Continue with this destruction?", "Segtendo WARNING", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                File.Delete("config/Config.xml");
                File.Copy("config/Default_Config.xml", "config/Config.xml");

                state_check();
                console_write("Config reset complete");
            }
        }
        #endregion
        #endregion

        #region CharacterAction
        //When a character is selected
        private void character_selected(object sender, EventArgs e)
        {
            state_check();
            this.selected_char_name = CharacterList.SelectedItem.ToString();
            this.selected_char_folder = handler.get_folder_name(this.selected_char_name);
            
            this.model_destination = "workspace/data/fighter/" + this.selected_char_folder + "/";
            this.meta_destination = "config/meta/" + this.selected_char_folder + "/";

            if (handler.get_dlc_status(selected_char_name) == "yes")
            {
                this.csp_destination = "workspace/data/ui/replace/append/chr/";
            }
            else
            {
                this.csp_destination = "workspace/data/ui/replace/chr/";
            }
            state_check();
            skin_ListBox_reload();
        }

        #endregion
        #region SkinAction
        //When a skin is selected
        private void skin_selected(object sender, EventArgs e)
        {
            skin_details_reload();
            state_check();
            metadata_reload();
        }

        //Skin Info Saved button is pressed
        private void save_skin_info(object sender, EventArgs e)
        {
            Skin current = new MeteorSkinLibrary.Skin(int.Parse(SlotNumberText.Text), SkinOriginText.Text, SkinNameText.Text, false);
            handler.set_skin(this.selected_char_name, this.selected_skin_slot + 1, current);
            skin_ListBox_reload();
            state_check();
        }

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
                console_write("Deleted slot " + index);
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

            state_check();



        }

        //When Clean Files is pressed
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
            if (Directory.Exists(this.model_destination))
            {
                String[] modelfiles = Directory.GetFiles(this.model_destination, "*", SearchOption.AllDirectories);
                foreach (String file in modelfiles)
                {
                    Console.WriteLine(file);
                    File.Delete(file);

                }
            }

            state_check();
            skin_details_reload();

        }
        #endregion
        #region ModelAction
        //On model selected
        private void model_selected(object sender, EventArgs e)
        {
            if (models_ListView.SelectedItems.Count == 1)
            {
                this.selected_skin_model_name = models_ListView.SelectedItems[0].Text;
                label5.Text = "Selected Model : " + models_ListView.SelectedItems[0].Text;
                button6.Enabled = true;
            }
            state_check();
        }
        //On model delete
        private void remove_selected_model_Click(object sender, EventArgs e)
        {
            if (this.selected_skin_model_name == "cxx")
            {
                foreach (String file in Directory.GetFiles(this.model_destination + "/body/c" + this.selected_skin_slot_model))
                {
                    File.Delete(file);
                }
                Directory.Delete(this.model_destination + "/body/c" + this.selected_skin_slot_model);
            }
            else
            {
                foreach (String file in Directory.GetFiles(this.model_destination + "/body/l" + this.selected_skin_slot_model))
                {
                    File.Delete(file);
                }
                Directory.Delete(this.model_destination + "/body/l" + this.selected_skin_slot_model);
            }

            handler.delete_skin_model(selected_char_name, int.Parse(this.selected_skin_slot_model) + 1, this.selected_skin_model_name);
            skin_details_reload();
            skin_ListBox_reload();
            state_check();
        }
        #endregion
        #region CspAction
        //When a csp is selected
        private void csp_selected(object sender, EventArgs e)
        {
            if (csps_ListView.SelectedItems.Count == 1)
            {
                this.selected_skin_csp_name = csps_ListView.SelectedItems[0].Text;
                selected_csp_name.Text = "Selected CSP : " + csps_ListView.SelectedItems[0].Text;
                remove_selected_csp.Enabled = true;
            }
            state_check();
        }
        //When a csp is deleted
        private void remove_selected_csp_Click(object sender, EventArgs e)
        {
            String file_path = "workspace/data/ui/replace/" + dlc + "chr/" + this.selected_skin_csp_name + "/" + this.selected_skin_csp_name + "_" + this.selected_char_folder + "_" + this.selected_skin_slot_csp + ".nut";
            File.Delete(file_path);
            handler.delete_skin_csp(selected_char_name, int.Parse(selected_skin_slot_csp), this.selected_skin_csp_name);
            skin_details_reload();
            state_check();
        }
        #endregion
        #region MetaDataAction
        //When you save all metadata
        void meta_save(object sender, EventArgs e)
        {
            String author = textBox1.Text;
            String version = textBox2.Text;
            String name = textBox3.Text;
            String textidfix = textBox4.Text;
            meta.set("author", author);
            meta.set("version", version);
            meta.set("name", name);
            meta.set("textidfix", textidfix);

            metadata_reload();
        }
        #endregion

        #region Interface
        #region Reloads
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
        //Reloads MetaData
        private void metadata_reload()
        {
            String meta_slot_path=this.meta_destination+"slot_"+(this.selected_skin_slot+1);
            Console.WriteLine("slot path :" + meta_slot_path);
            PropertyHandler properties = new PropertyHandler("config/Config.xml");

            //Reset boxes
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

            //Check meta destination folder
            if (!Directory.Exists(this.meta_destination)){
                Directory.CreateDirectory(this.meta_destination);
            }
            //Check meta destination slot folder
            if (!Directory.Exists(meta_slot_path)){
                //If non existant, create folder and append meta based on default
                Directory.CreateDirectory(meta_slot_path);
                File.Copy(properties.get("default_meta"),meta_slot_path+"/meta.xml");
                meta.set_library_path(meta_slot_path + "/meta.xml");
                textBox1.Text = meta.get("author");
                textBox2.Text = meta.get("version");
                textBox3.Text = meta.get("name");
                textBox4.Text = meta.get("textidfix");
            }
            else
            {
                if(File.Exists(meta_slot_path + "/meta.xml"))
                {
                    meta.set_library_path(meta_slot_path + "/meta.xml");
                    textBox1.Text = meta.get("author");
                    textBox2.Text = meta.get("version");
                    textBox3.Text = meta.get("name");
                    textBox4.Text = meta.get("textidfix");
                }else
                {
                    File.Copy(properties.get("default_meta"), meta_slot_path + "/meta.xml");
                    meta.set_library_path(meta_slot_path + "/meta.xml");
                    textBox1.Text = meta.get("author");
                    textBox2.Text = meta.get("version");
                    textBox3.Text = meta.get("name");
                    textBox4.Text = meta.get("textidfix");
                }

            }
            


        }

        #endregion
        #region Drag&Drop
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
            state_check();
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
                state_check();
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
            state_check();
        }
        #endregion
        #region Inits
        //Filling Character list
        public void init_character_ListBox()
        {
            foreach (String chars in Characters)
            {
                CharacterList.Items.Add(chars);
            }

        }
        #endregion
        #region Batch
        //batch copy with drag&dropped folder and current slot
        private void batch_move_model()
        {

            String slot = this.selected_skin_slot_model;
            foreach (String folder in this.model_folder_list)
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
        //batch copy with specified source and destination slot
        private void batch_move_model(String[] filelist, int i_slot)
        {

            String slot = (i_slot - 1) < 10 ? "0" + (i_slot - 1).ToString() : (i_slot - 1).ToString();
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
        //batch copy with drag&dropped folder and current slot
        private void batch_copy_csp()
        {
            String path = csp_file_list[0];
            String filter = "*" + this.selected_char_folder + "_" + "*.nut";

            String[] files = Directory.GetFiles(path, filter, SearchOption.AllDirectories);

            foreach (String file in files)
            {
                console_write("File Detected :" + Path.GetFileName(file));
                String type = Path.GetFileName(file).Split('_')[0];
                String number = Path.GetFileName(file).Split('_')[1];
                copyCSP(file, type + number, this.selected_skin_slot_csp);
            }
            console_write("All detected CSP were moved to slot " + this.selected_skin_slot_csp);
            skin_details_reload();
        }
        //batch copy with specified source and destination slot
        private void batch_copy_csp(String[] filelist, int i_slot)
        {
            if (filelist.Length != 0)
            {
                String slot = (i_slot) < 10 ? "0" + (i_slot).ToString() : (i_slot).ToString();
                String path = filelist[0];

                String filter = "*.nut";

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
        //batch copy with specified source and new next slot
        private void batch_add_slot()
        {
            Regex slotname = new Regex("(slot_)([0-9]{1,3})(_)(p*)");
            foreach (String file in this.slot_file_list)
            {
                if (slotname.IsMatch(Path.GetFileName(file)))
                {
                    skin_ListBox_reload();
                    console_write("Slot Detected : " + Path.GetFileName(file));
                    String skin_name = Path.GetFileName(file).Split('_')[2];
                    if (skin_name == "")
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
                        String[] folder = new string[] { file + "/csp/" };
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
                    modelpath += "body/c" + slot;
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
                foreach (String file in files)
                {
                    String filepath = modelpath + "/" + Path.GetFileName(file);
                    File.Copy(file, filepath, true);
                }
                handler.add_skin_model(this.selected_char_name, int.Parse(slot) + 1, type);
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
                handler.add_skin_model(this.selected_char_name, int.Parse(slot) + 1, type);
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
            Console.WriteLine("Copied : " + final_file_name);
            if (!Directory.Exists(this.csp_destination + "/" + Filetype + "/"))
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
        #endregion
        #region Console
        //Writes string to console
        private void console_write(String s)
        {
            textConsole.Text += s + "\n";
        }
        #endregion
        #region State
        //State Checker
        private void state_check()
        {
            int character = CharacterList.SelectedIndex;
            int skin = SkinListBox.SelectedIndex;
            int model = models_ListView.SelectedIndices.Count;
            int csp = csps_ListView.SelectedIndices.Count;
            String origin = SkinOriginText.Text;

            if (character == -1)
            {
                //Interactions
                models_ListView.AllowDrop = false;
                csps_ListView.AllowDrop = false;
                slotbox.AllowDrop = false;
                button1.Enabled = false;

                //State
            }
            else
            {
                //Interactions
                slotbox.AllowDrop = true;
            }

            if (skin == -1)
            {
                //Interactions
                models_ListView.AllowDrop = false;
                csps_ListView.AllowDrop = false;
                SkinNameText.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                //State
            }
            else
            {
                //Interactions
                models_ListView.AllowDrop = true;
                csps_ListView.AllowDrop = true;
                SkinNameText.Enabled = true;
                button1.Enabled = true;
                button4.Enabled = true;
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                textBox3.ReadOnly = false;
                textBox4.ReadOnly = false;
                if (origin == "default")
                {
                    button2.Enabled = true;
                    button3.Enabled = true;
                }
                else
                {
                    button2.Enabled = true;
                }
            }
            if (model == 0)
            {
                button6.Enabled = false;
            }
            else
            {
                remove_selected_csp.Enabled = true;
            }
            if (csp == 0)
            {
                remove_selected_csp.Enabled = false;
            }
            else
            {
                remove_selected_csp.Enabled = true;
            }



        }
        #endregion
        #region Config
        //Gets a property
        private void get_property(String property_name)
        {
            properties.get(property_name);
        }
        //Gets a property
        private void set_property(String property_name,String property_value)
        {
            properties.set(property_name, property_value);
        }


        #endregion

        #endregion


    }


}
