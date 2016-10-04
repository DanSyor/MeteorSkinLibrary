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
        LibraryHandler Library;
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
                properties.add("current_library", "config/Library.xml");
                console_write("Config loaded : config/Config.xml");

                //Checks Library.xml presence, if not creates one based on Default_Library.xml
                if (!File.Exists("config/Library.xml"))
                {
                    console_write("Creating Library");
                    File.Copy(properties.get("default_library"), "config/Library.xml");
                }


                Library = new LibraryHandler(properties.get("current_library"));
                console_write("Library loaded : " + properties.get("current_library"));

                //Loads Character List
                Characters = Library.get_character_list();
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
            Library.add_skin(this.selected_char_name, SkinListBox.Items.Count + 1);
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
                skin_ListBox_reload();
                console_write("Library reset complete");
            }

        }
        //Workspace reset button
        private void reset_workspace(object sender, EventArgs e)
        {
            if (MessageBox.Show("Doing this will erase all contents of the workspace folder which contains every file you've added. Continue with this destruction?", "Segtendo WARNING", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                batch_delete("workspace");
                Directory.CreateDirectory("workspace");
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
        //Reset all button
        private void reset_all(object sender, EventArgs e)
        {
            if (MessageBox.Show("Doing this will erase all configuration changes. It will erase all files of every mod you've added. The library containing skin information will be deleted. Continue with this Supermassive black-hole type destruction?", "Super Segtendo WARNING", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                File.Delete("config/Config.xml");
                File.Copy("config/Default_Config.xml", "config/Config.xml");

                console_write("Config reset complete");

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

                File.Delete("config/Library.xml");
                File.Copy("config/Default_Library.xml", "config/Library.xml");

                console_write("Library reset complete");
                skin_ListBox_reload();
                state_check();
            }
        }
        //Reset all button
        private void reset_all()
        {
            if (MessageBox.Show("Doing this will erase all configuration changes. It will erase all files of every mod you've added. The library containing skin information will be deleted. Continue with this Supermassive black-hole type destruction?", "Super Segtendo WARNING", MessageBoxButtons.YesNo) == DialogResult.Yes)
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

                File.Delete("config/Library.xml");
                File.Copy("config/Default_Library.xml", "config/Library.xml");

                console_write("Library reset complete");
                skin_ListBox_reload();
                state_check();
            }
        }
        #endregion
        #region SmashExplorerMenu
        private void launch_se_import(object sender, EventArgs e)
        {
            batch_import_SE();
        }
        #endregion
        #endregion

        #region CharacterAction
        //When a character is selected
        private void character_selected(object sender, EventArgs e)
        {
            state_check();
            this.selected_char_name = CharacterList.SelectedItem.ToString();
            this.selected_char_folder = Library.get_folder_name(this.selected_char_name);

            this.model_destination = "workspace/data/fighter/" + this.selected_char_folder + "/model/";
            this.meta_destination = "config/meta/" + this.selected_char_folder + "/";

            if (Library.get_dlc_status(selected_char_name) == "yes")
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
            Library.set_skin(this.selected_char_name, this.selected_skin_slot + 1, current);
            skin_ListBox_reload();
            state_check();
        }

        //When Delete is pressed
        private void skin_delete(object sender, EventArgs e)
        {
            int index = SkinListBox.SelectedIndex + 1;
            int max = SkinListBox.Items.Count;
            Skin current = Library.get_skin(this.selected_char_name, index);

            if (index < max)
            {
                Library.clean_skin(this.selected_char_name, index);
                skin_ListBox_reload();
                console_write("Deleted slot " + index);
            }
            else
            {
                Library.delete_skin(this.selected_char_name, index);
                skin_ListBox_reload();
                console_write("Deleted slot " + index);
            }
            String filter = ""+this.selected_skin_slot;
            String[] modeldirectories = Directory.GetDirectories("workspace/data/fighter/" + this.selected_char_folder + "/model/", filter, SearchOption.AllDirectories);
            if (modeldirectories.Length > 0)
            {
                foreach(String dir in modeldirectories)
                {
                    Directory.Delete(dir, true);
                    Console.WriteLine(dir);
                }
            }


            String csppath = "workspace/data/ui/replace/" + dlc + "chr/ ";
            if (!Directory.Exists(csppath))
            {
                Directory.CreateDirectory(csppath);
            }
            String[] files = Directory.GetFiles(csppath, "*" + this.selected_char_folder + "_" + this.selected_skin_slot_csp + ".nut", SearchOption.AllDirectories);
            if(files.Length > 0)
            {
                foreach (String file in files)
                {
                    Console.WriteLine(file);
                    File.Delete(file);
                }
            }

            state_check();



        }

        //When Clean Files is pressed
        private void clean_files_clicked(object sender, EventArgs e)
        {
            int index = SkinListBox.SelectedIndex + 1;
            int max = SkinListBox.Items.Count;
            Skin current = Library.get_skin(this.selected_char_name, index);

            Library.clean_skin(this.selected_char_name, index);
            skin_ListBox_reload();
            console_write("Cleaned slot " + index);

            String filter = "" + this.selected_skin_slot;
            if(Directory.Exists("workspace/data/fighter/" + this.selected_char_folder + "/model/"))
            {
                String[] modeldirectories = Directory.GetDirectories("workspace/data/fighter/" + this.selected_char_folder + "/model/", filter, SearchOption.AllDirectories);
                if (modeldirectories.Length > 0)
                {
                    foreach (String dir in modeldirectories)
                    {
                        Directory.Delete(dir, true);
                        Console.WriteLine(dir);
                    }
                }

            }


            String csppath = "workspace/data/ui/replace/" + dlc + "chr/ ";
            if (!Directory.Exists(csppath))
            {
                Directory.CreateDirectory(csppath);
            }
            String[] files = Directory.GetFiles(csppath, "*" + this.selected_char_folder + "_" + this.selected_skin_slot_csp + ".nut", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                foreach (String file in files)
                {
                    Console.WriteLine(file);
                    File.Delete(file);
                }
            }

            state_check();

        }
        
        //packages skin into meteor skin
        private void package_meteor(object sender, EventArgs e)
        {

            String skin_name = textBox3.Text;
            String package_name = "meteor_xx_" + skin_name + "/";
            String package_destination = "packages/" + package_name;
            String csp_slot = (this.selected_skin_slot + 1) <10 ? "0"+ (this.selected_skin_slot + 1).ToString() : (this.selected_skin_slot + 1).ToString();
            String model_slot = (this.selected_skin_slot) < 10 ? "0" + (this.selected_skin_slot).ToString() : (this.selected_skin_slot).ToString();

            String model_dest = package_destination + "model/";
            String csp_dest = package_destination + "csp/";
            String meta_dest = package_destination + "meta/";

            String meta_source = this.meta_destination + "slot_"+int.Parse(csp_slot)+"/";
            String model_source = this.model_destination;
            String csp_source = this.csp_destination;

            if (Directory.Exists(package_destination))
            {
                batch_delete(package_destination);
            }

            Directory.CreateDirectory(package_destination);
            Directory.CreateDirectory(model_dest);
            Directory.CreateDirectory(csp_dest);
            Directory.CreateDirectory(meta_dest);
            console_write(csp_slot);
            File.Copy(meta_source + "meta.xml", meta_dest + "meta.xml",true);

            if (Directory.Exists(model_source))
            {
                
                String[] folders = Directory.GetDirectories(model_source);
                // /model/ directory
                if(folders.Length > 0)
                {
                    foreach(String model_folders in folders)
                    {
                        //Body n stuff
                        if (Directory.GetDirectories(model_folders).Length > 0)
                        {
                            //cXX n stuff
                            foreach (String model in Directory.GetDirectories(model_folders))
                            {
                                if(Directory.GetFiles(model).Length > 0)
                                {
                                    Console.WriteLine(model_dest + Path.GetFileName(model_folders) + "/" + Path.GetFileName(model));
                                    if (Directory.Exists(model_dest + Path.GetFileName(model_folders) + "/" + Path.GetFileName(model)))
                                    {
                                        Directory.Delete(model_dest + Path.GetFileName(model_folders) + "/" + Path.GetFileName(model), true);
                                    }
                                    Directory.CreateDirectory(model_dest + Path.GetFileName(model_folders) + "/" + Path.GetFileName(model));

                                    foreach(String file in Directory.GetFiles(model))
                                    {
                                        File.Copy(file, model_dest + Path.GetFileName(model_folders) + "/" + Path.GetFileName(model) + "/" + Path.GetFileName(file));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (Directory.Exists(csp_source))
            {
                String filter = "*" + this.selected_char_folder + "_" + csp_slot + ".nut";
                String[] csps = Directory.GetFiles(csp_source, filter, SearchOption.AllDirectories);
                if (csps.Length > 0)
                {
                    foreach (String csp in csps)
                    {
                        console_write("Adding " + Path.GetFileName(csp) + " to the package");
                        String csp_folder = Path.GetFileName(csp).Split('_')[0] + "_" + Path.GetFileName(csp).Split('_')[1];
                        Console.WriteLine(csp_dest + csp_folder + "/" + Path.GetFileName(csp));
                        if (!Directory.Exists(csp_dest + csp_folder))
                        {
                            Directory.CreateDirectory(csp_dest + csp_folder);
                        }
                        File.Copy(csp, csp_dest + csp_folder + "/" + Path.GetFileName(csp), true);
                    }
                }

            }

            console_write("Packaged skin to " + package_destination);


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
            Regex cXX = new Regex("(c){1}([0-9]{2}|xx)", RegexOptions.IgnoreCase);
            Regex lXX = new Regex("(l){1}([0-9]{2}|xx)", RegexOptions.IgnoreCase);
            String folder = this.model_destination;
            if (cXX.IsMatch(this.selected_skin_model_name.Split('/')[1]))
            {
                folder += this.selected_skin_model_name.Split('/')[0] + "/"+"c";
            }
            if (lXX.IsMatch(this.selected_skin_model_name.Split('/')[1]))
            {
                folder += this.selected_skin_model_name.Split('/')[0] + "/" + "l";
            }
            folder += this.selected_skin_slot_model;

            Console.WriteLine(folder);
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
            Library.delete_skin_model(selected_char_name, int.Parse(this.selected_skin_slot_model) + 1, this.selected_skin_model_name);
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
            Library.delete_skin_csp(selected_char_name, int.Parse(selected_skin_slot_csp), this.selected_skin_csp_name);
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
        //When metadata is added to library
        void add_meta(String metapath, int slot)
        {
            if(!Directory.Exists(meta_destination + "slot_" + slot))
            {
                Directory.CreateDirectory(meta_destination + "slot_" + slot);
            }
            File.Copy(metapath, meta_destination + "slot_" + slot + "/meta.xml", true);
            console_write("Meta detected and moved to " + meta_destination + "/slot_" + slot + "/meta.xml");
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
            skin = Library.get_skin(this.selected_char_name, SkinListBox.SelectedIndex + 1);
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
            Skins = Library.get_skin_list(selected_char_name);
            foreach (String skin in Skins)
            {
                SkinListBox.Items.Add(skin);
            }
        }
        //Reloads MetaData
        private void metadata_reload()
        {
            String meta_slot_path = this.meta_destination + "slot_" + (this.selected_skin_slot + 1);
            PropertyHandler properties = new PropertyHandler("config/Config.xml");

            //Reset boxes
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

            //Check meta destination folder
            if (!Directory.Exists(this.meta_destination))
            {
                Directory.CreateDirectory(this.meta_destination);
            }
            //Check meta destination slot folder
            if (!Directory.Exists(meta_slot_path))
            {
                //If non existant, create folder and append meta based on default
                Directory.CreateDirectory(meta_slot_path);
                File.Copy(properties.get("default_meta"), meta_slot_path + "/meta.xml");
                meta.set_library_path(meta_slot_path + "/meta.xml");
                textBox1.Text = meta.get("author");
                textBox2.Text = meta.get("version");
                textBox3.Text = meta.get("name");
                textBox4.Text = meta.get("textidfix");
            }
            else
            {
                if (File.Exists(meta_slot_path + "/meta.xml"))
                {
                    meta.set_library_path(meta_slot_path + "/meta.xml");
                    textBox1.Text = meta.get("author");
                    textBox2.Text = meta.get("version");
                    textBox3.Text = meta.get("name");
                    textBox4.Text = meta.get("textidfix");
                }
                else
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
            batch_copy_model(this.model_folder_list, int.Parse(this.selected_skin_slot_model),this.selected_char_name);
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
            batch_add_slot(SkinListBox.Items.Count + 1);
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
        private void batch_copy_model(String[] folderlist, int i_slot, String char_folder_name)
        {

            //defines model default location in workspace, /fighter/model/
            String model_dest = "workspace/data/fighter/" + Library.get_folder_name(char_folder_name) + "/model/";

            foreach (String folder in folderlist)
            {
                // Base folder level
                //It means, folders are inside
                if (Path.GetFileName(folder) == "model")
                {
                    // Model folder level
                    //Getting directories inside
                    String[] model_folders = Directory.GetDirectories(folder);
                    //Checking folder presence
                    if (model_folders.Length > 0)
                    {
                        foreach(String folder2 in model_folders)
                        {
                            //body others level
                            
                            String[] body_folders = Directory.GetDirectories(folder2);
                            foreach (String folder3 in body_folders)
                            {
                                //cXX / lXX level
                                if (Directory.GetFiles(folder3).Length > 0)
                                {
                                    Console.WriteLine(folder3);
                                    copyModel(folder3, i_slot.ToString(), Path.GetFileName(folder2), Path.GetFileName(folder3),char_folder_name,model_dest);        
                                }
                                
                            }
                        }
                    }
                }
                //body others level
                //moving a folder that's inside model
                else
                {
                    String[] body_folders = Directory.GetDirectories(folder);
                    if(Directory.GetFiles(folder).Length == 0)
                    {
                        foreach (String folder2 in body_folders)
                        {
                            //cXX / lXX level
                            if (Directory.GetFiles(folder2).Length > 0)
                            {
                                Console.WriteLine(folder2);
                                copyModel(folder2, i_slot.ToString(), Path.GetFileName(folder), Path.GetFileName(folder2), char_folder_name, model_dest);
                            }

                        }
                    }else
                    {
                        Regex clXX = new Regex("^[cl]([0-9]{2}|xx)$",RegexOptions.IgnoreCase);
                        if (clXX.IsMatch(Path.GetFileName(folder)))
                        {
                            copyModel(folder, i_slot.ToString(), "body", Path.GetFileName(folder), char_folder_name, model_dest);
                        }
                    }
                }
            }
            skin_details_reload();

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
                copyCSP(file, type + "_" + number, this.selected_skin_slot_csp);
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
                    copyCSP(file, type + "_" + number, slot);
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
        private void batch_add_slot(int slot)
        {
            Regex slotname = new Regex("(meteor_)(x{2})(_)(p*)");
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

                    Library.add_skin(this.selected_char_name, skin_slot, skin_name);

                    //Model files check
                    if (Directory.Exists(file + "/model"))
                    {
                        console_write("Slot model folder detected");
                        batch_copy_model(Directory.GetDirectories(file + "/model"), SkinListBox.Items.Count, this.selected_char_name);
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
                    //Meta File Check
                    if (Directory.Exists(file + "/meta"))
                    {
                        if (File.Exists(file + "/meta/meta.xml"))
                        {
                            add_meta(file + "/meta/meta.xml", slot);
                        }
                        else
                        {
                            Console.WriteLine("no Meta detected");
                        }

                    }
                }
            }
            skin_ListBox_reload();
            SkinListBox.SelectedIndex = (SkinListBox.Items.Count - 1);
            skin_details_reload();
        }
        //Used to move a cXX folder to the correct location
        private void copyModel(String path, String slot, String folder, String name,String char_folder_name, String model_dest)
        {
            String final_name = "xxx";
            String slot_folder_name = "";
            Regex cXX = new Regex("(c){1}([0-9]{2}|xx)", RegexOptions.IgnoreCase);
            Regex lXX = new Regex("(l){1}([0-9]{2}|xx)", RegexOptions.IgnoreCase);

            //Checks /fighter/model/ folder
            if (!Directory.Exists(model_dest))
            {
                Directory.CreateDirectory(model_dest);
            }
            //getting folder name
            String model = Path.GetDirectoryName(path);

            //checking folder type
            if(cXX.IsMatch(name))
            {
                final_name = "cXX";
                slot_folder_name = "c";
            }
            if(lXX.IsMatch(name))
            {
                final_name = "lXX";
                slot_folder_name = "l";
            }

            slot_folder_name += int.Parse(slot) < 10 && slot.Length == 1 ? "0" + slot : slot;
            //Check destination folder
            if (Directory.Exists(model_dest + folder + "/" + slot_folder_name))
            {
                Directory.Delete(model_dest + folder + "/" + slot_folder_name, true);
            }
            //Creating destination
            Directory.CreateDirectory(model_dest + folder + "/" + slot_folder_name);

            //Copying files to destination
            String[] origin_files = Directory.GetFiles(path);
            Console.WriteLine("there");
            foreach(String file in origin_files)
            {
                File.Copy(file, model_dest+""+folder+ "/"+ slot_folder_name + "/" + Path.GetFileName(file));
            }
            //Add model to Library
            Library.add_skin_model(char_folder_name, int.Parse(slot) + 1, folder+"/"+final_name);

            console_write("Model moved to \"" +folder+"/"+ slot_folder_name+  "\"");
        }
        //Used to move a CSP file to the correct location
        private void copyCSP(String FilePath, String Filetype, String slot)
        {
            String destination = "workspace/data/ui/replace/" + dlc + "chr/" + Filetype + "/";
            Console.WriteLine(destination);

            String character_folder = this.selected_char_folder;
            String final_file_name = Filetype + "_" + char.ToUpper(character_folder[0]) + character_folder.Substring(1) + "_" + slot + ".nut";

            Console.WriteLine("Copied : " + final_file_name);
            if (!Directory.Exists(this.csp_destination + "/" + Filetype + "/"))
            {
                Directory.CreateDirectory(this.csp_destination + Filetype + "/");
            }
            String original_filename = Path.GetFileName(FilePath);
            File.Copy(FilePath, this.csp_destination + "/" + Filetype + "/" + final_file_name, true);
            Library.add_skin_csp(this.selected_char_name, int.Parse(slot), Filetype);

        }
        //Used when copying a csp to a specific character
        private void copyCSP(String FilePath, String Filetype, String slot, String folder, String char_name)
        {
            String destination = "workspace/data/ui/replace/" + dlc + "chr/" + Filetype + "/";
            Console.WriteLine(destination);

            String character_folder = folder;
            String final_file_name = Filetype + "_" + char.ToUpper(character_folder[0]) + character_folder.Substring(1) + "_" + slot + ".nut";

            Console.WriteLine("Copied : " + final_file_name);
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
            String original_filename = Path.GetFileName(FilePath);
            File.Copy(FilePath, destination + final_file_name, true);
            Library.add_skin_csp(char_name, int.Parse(slot), Filetype);

        }
        private void copyCSP2(String FilePath, String Filetype, String slot,String char_name)
        {

            String destination = "workspace/data/ui/replace/chr/" + Filetype + "/";
            Console.WriteLine(destination);

            String character_folder = Path.GetFileName(FilePath).Split('_')[2];
            if(Library.get_dlc_status(char_name) == "yes")
            {
                destination = "workspace/data/ui/replace/append/chr/" + Filetype + "/";
            }

            String final_file_name = Filetype + "_" + character_folder + "_" + slot + ".nut";

            Console.WriteLine("Copied : " + final_file_name);
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
            String original_filename = Path.GetFileName(FilePath);
            File.Copy(FilePath, destination + final_file_name, true);
            Library.add_skin_csp(char_name, int.Parse(slot), Filetype);
        }
        //Used to move a slot to the correct location
        private void copySlot()
        {

        }
        //Used to import SmashExplorer workspace into Library
        private void batch_import_SE()
        {
            //Reseting all to avoid conflicts
            reset_all();
            //Setting SE paths
            String se_workspace_path = properties.get("explorer_workspace");
            String se_model_path = se_workspace_path + "/content/patch/data/fighter/";
            String se_csp_path = se_workspace_path + "/content/patch/data/ui/replace/chr/";
            String se_csp_path_dlc = se_workspace_path + "/content/patch/data/ui/replace/append/chr/";

            //Setting destinations
            String mmsl_workspace_path = "workspace";
            String mmsl_model_path = "workspace/data/fighter/";
            String mmsl_csp_path = "workspace" + "/data/ui/replace/";

            //Setting slot numbers
            String slot_model = (SkinListBox.Items.Count +1).ToString();
            String slot_csp = "";

            //Workspace folder check
            if (Directory.Exists(se_workspace_path))
            {
                #region ModelImporting
                //characters folder check
                if (Directory.Exists(se_model_path))
                {
                    //Character folders based on folders
                    String[] characters = Directory.GetDirectories(se_model_path);
                    foreach (String character in characters)
                    {
                        //Checking said character has folders 
                        if (Directory.GetDirectories(character).Length != 0)
                        {
                            //Checking model folder
                            if (Directory.Exists(character + "/model"))
                            {
                                mmsl_model_path = "workspace/data/fighter/" + Path.GetFileName(character) + "/model/";
                                console_write("Detected character: " + Library.get_skin_character_name(Path.GetFileName(character)));
                                for (int i = 0; i < 256; i++)
                                {
                                    String slot = i < 10 ? "0" + i : i.ToString();
                                    //Checking subfolders
                                    String[] Directories = Directory.GetDirectories(character + "/model", "*" + slot, SearchOption.AllDirectories);
                                    if (Directories.Length > 0)
                                    {
                                        console_write("Detected model files");
                                        foreach (String dir in Directories)
                                        {
                                            console_write("Detected: " + Path.GetFileName(Directory.GetParent(dir).FullName) + "/" + Path.GetFileName(dir));
                                            if (!Library.check_skin(Library.get_skin_character_name(Path.GetFileName(character)), int.Parse(slot) + 1))
                                            {
                                                Library.add_skin(Library.get_skin_character_name(Path.GetFileName(character)), int.Parse(slot) + 1);
                                            }
                                            Console.WriteLine(Path.GetFullPath(dir));
                                            copyModel(dir, slot, Path.GetFileName(Directory.GetParent(dir).FullName), Path.GetFileName(dir), Library.get_skin_character_name(Path.GetFileName(character)), mmsl_model_path);
                                        }

                                    }
                                }

                            }
                        }

                    }
                }
                #endregion
                #region CspImporting
                if (Directory.Exists(se_csp_path))
                {
                    //chr folders
                     
                    String[] csps_dlc = Directory.GetDirectories(se_csp_path);
                    if (csps_dlc.Length > 0)
                    {
                        foreach (String cspformat in csps_dlc)
                        {
                            //check folder
                            if(Directory.GetFiles(cspformat).Length > 0)
                            {
                                //For all slot values
                                for(int i = 0; i < 256; i++)
                                {
                                    foreach (String csp in Directory.GetFiles(cspformat))
                                    {
                                        //got every info for file
                                        String test = Path.GetFileName(csp);
                                        String slot = Path.GetFileName(csp).Split('_')[3].Split('.')[0];
                                        int teste;
                                        if (int.TryParse(slot,out teste))
                                        {
                                            if (int.Parse(slot) == (i + 1))
                                            {
                                                String foldername = Path.GetFileName(csp).Split('_')[2].ToLower();
                                                if (Library.check_character_foldername(foldername))
                                                {
                                                    copyCSP2(csp, Path.GetFileName(cspformat), slot, Library.get_skin_character_name(foldername));
                                                    if (Library.check_skin(Library.get_skin_character_name(foldername), int.Parse(slot)))
                                                    {
                                                        Library.add_skin_csp(Library.get_skin_character_name(foldername), int.Parse(slot), Path.GetFileName(cspformat));
                                                    }
                                                    else
                                                    {
                                                        Library.add_skin(Library.get_skin_character_name(foldername), int.Parse(slot));
                                                        Library.add_skin_csp(Library.get_skin_character_name(foldername), int.Parse(slot), Path.GetFileName(cspformat));
                                                    }

                                                    console_write("Detected: " + Path.GetFileName(csp));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                }
                #endregion
                #region DlcCspImporting
                //chr folders

                String[] csps = Directory.GetDirectories(se_csp_path_dlc);
                if (csps.Length > 0)
                {
                    foreach (String cspformat in csps)
                    {
                        //check folder
                        if (Directory.GetFiles(cspformat).Length > 0)
                        {
                            //For all slot values
                            for (int i = 0; i < 256; i++)
                            {
                                foreach (String csp in Directory.GetFiles(cspformat))
                                {
                                    //got every info for file
                                    String test = Path.GetFileName(csp);
                                    String slot = Path.GetFileName(csp).Split('_')[3].Split('.')[0];
                                    int teste;
                                    if (int.TryParse(slot, out teste))
                                    {
                                        if (int.Parse(slot) == (i + 1))
                                        {
                                            String foldername = Path.GetFileName(csp).Split('_')[2].ToLower();
                                            if (Library.check_character_foldername(foldername))
                                            {
                                                copyCSP2(csp, Path.GetFileName(cspformat), slot, Library.get_skin_character_name(foldername));
                                                if (Library.check_skin(Library.get_skin_character_name(foldername), int.Parse(slot)))
                                                {
                                                    Library.add_skin_csp(Library.get_skin_character_name(foldername), int.Parse(slot), Path.GetFileName(cspformat));
                                                }
                                                else
                                                {
                                                    Library.add_skin(Library.get_skin_character_name(foldername), int.Parse(slot));
                                                    Library.add_skin_csp(Library.get_skin_character_name(foldername), int.Parse(slot), Path.GetFileName(cspformat));
                                                }

                                                console_write("Detected: " + Path.GetFileName(csp));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {

            }
        }
        //Used to import file info and skins from workspace into Library
        private void batch_detect_Library()
        {

        }
        //used to delete to empty and delete directory with all subs
        private void batch_delete(String foldername)
        {
            if (Directory.Exists(foldername))
            {
                String[] directories = Directory.GetDirectories(foldername, "*", SearchOption.AllDirectories);
                String[] files = Directory.GetFiles(foldername, "*", SearchOption.AllDirectories);
                if (files.Length != 0)
                {
                    foreach(String file in files)
                    {
                        File.Delete(file);
                    }
                }
                if (directories.Length != 0)
                {
                    foreach (String directory in directories)
                    {
                        batch_delete(directory);
                    }

                }

            }
        }
        #endregion
        #region Console
        //Writes string to console
        private void console_write(String s)
        {
            textConsole.Text = s + "\n" + textConsole.Text;
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
                button5.Enabled = false;
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
                button5.Enabled = true;
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
        private void set_property(String property_name, String property_value)
        {
            properties.set(property_name, property_value);
        }



        #endregion

        #endregion

    }


}
