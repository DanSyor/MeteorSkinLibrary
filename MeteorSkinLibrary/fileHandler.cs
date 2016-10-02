using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeteorSkinLibrary
{
    class fileHandler
    {
        
        public fileHandler()
        {

        }

        public Boolean[] SkinCheck(Skin skin, String CharName)
        {
            String[] files = new string[] { "chr_00", "chr_10", "chr_13", "stock_90" };
            String CharFolderName="";
            String slotNumber = "00";
            Boolean[] checks = new Boolean[4];
            int current = 0;

            if(skin.slot.ToString().Length == 1)
            {
                slotNumber = "0" + skin.slot;
            }

            foreach(String file in files)
            {
                if (File.Exists("workspace/data/ui/replace/" + file + "/" + "file" + "_" + CharFolderName + "_" + slotNumber))
                {
                    checks[current] = true;
                }
                else
                {
                    checks[current] = false;
                }

                current++;
            }
            
            return checks;
        }
    }

}
