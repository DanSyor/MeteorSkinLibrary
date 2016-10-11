using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace MeteorSkinLibrary
{
    class FileManager
    {
        LibraryHandler Library;
        PropertyHandler properties = new PropertyHandler("mmsl_config/Default_Config.xml");
        MetaHandler meta = new MetaHandler("mmsl_config/meta/Default_Meta.xml");
        UICharDBHandler uichar;

        public FileManager(LibraryHandler lib, UICharDBHandler ui)
        {
            this.Library = lib;
            this.uichar = ui;
        }

        private void batch_import_SE(int skincount)
        {
            //Setting SE paths
            String se_mmsl_workspace_path = properties.get("explorer_workspace");
            String se_model_path = se_mmsl_workspace_path + "/content/patch/data/fighter/";
            String se_csp_path = se_mmsl_workspace_path + "/content/patch/data/ui/replace/chr/";
            String datafolder = properties.get("datafolder");
            String se_csp_path_dlc = se_mmsl_workspace_path + "/content/patch/" + datafolder + "/ui/replace/append/chr/";

            String slot_model = (skincount + 1).ToString();



            //mmsl_workspace folder check
            if (Directory.Exists(se_mmsl_workspace_path))
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

                                if (Library.check_character_foldername(Path.GetFileName(character)))
                                {
                                    Console.WriteLine("Detected character: " + Library.get_fullname_modelfolder(Path.GetFileName(character)));
                                    for (int i = 0; i < 256; i++)
                                    {
                                        String slot = i < 10 ? "0" + i : i.ToString();
                                        //Checking subfolders
                                        String[] Directories = Directory.GetDirectories(character + "/model", "*" + slot, SearchOption.AllDirectories);
                                        if (Directories.Length > 0)
                                        {
                                            Console.WriteLine("Detected model files");
                                            foreach (String dir in Directories)
                                            {
                                                Console.WriteLine("Detected: " + Path.GetFileName(Directory.GetParent(dir).FullName) + "/" + Path.GetFileName(dir));
                                                if (!Library.check_skin(Library.get_fullname_modelfolder(Path.GetFileName(character)), int.Parse(slot) + 1))
                                                {
                                                    Library.add_skin(Library.get_fullname_modelfolder(Path.GetFileName(character)), int.Parse(slot) + 1);
                                                }
                                                new Skin(Library.get_fullname_modelfolder(Path.GetFileName(character)), i + 1, "Imported skin", "Sm4sh Explorer").add_model(dir, Directory.GetParent(dir).Name);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                #region CspImporting

                for (int z = 0; z < 2; z++)
                {
                    if (z == 1)
                    {
                        se_csp_path = se_csp_path_dlc;
                    }
                    if (Directory.Exists(se_csp_path))
                    {
                        //chr folders

                        String[] csps = Directory.GetDirectories(se_csp_path);
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
                                            Regex cspr = new Regex("^((?:chrn|chr|stock)_[0-9][0-9])_([a-zA-Z]+)_[0-9][0-9].nut$");
                                            if (cspr.IsMatch(Path.GetFileName(csp)))
                                            {
                                                //got every info for file
                                                String test = Path.GetFileName(csp);
                                                String slot = Path.GetFileName(csp).Split('_')[3].Split('.')[0];
                                                int teste;
                                                if (int.TryParse(slot, out teste))
                                                {
                                                    //Same slot
                                                    if (int.Parse(slot) == (i + 1))
                                                    {
                                                        //Gettin foldername
                                                        String foldername = Path.GetFileName(csp).Split('_')[2];
                                                        if (Library.check_fullname_cspname(foldername))
                                                        {

                                                            if (!Library.check_skin(Library.get_fullname_cspfolder(foldername), int.Parse(slot)))
                                                            {
                                                                Library.add_skin(Library.get_fullname_cspfolder(foldername), int.Parse(slot));
                                                            }
                                                            new Skin(Library.get_fullname_cspfolder(foldername), i + 1, "Imported skin", "Sm4sh Explorer").add_csp(csp);
                                                            Console.WriteLine("Detected: " + Path.GetFileName(csp));
                                                        }
                                                    }
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
            }
            else
            {

            }
        }
    }
}
