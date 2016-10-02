using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeteorSkinLibrary
{
    class Skin
    {
        public int slot;
        public String origin;
        public String name;
        public Boolean model;
        public ArrayList csps;
        public ArrayList models;


        public Skin(int slot,String origin,String name,Boolean model)
        {
            this.slot = slot;
            this.origin = origin;
            this.name = name;
            this.model = model;
            csps = new ArrayList();
            models = new ArrayList();
        }

        public Skin()
        {
            this.slot = 0;
            this.origin = "dummy";
            this.name = "dummy";
            this.model = false;
            csps = new ArrayList();
            models = new ArrayList();
        }






    }
}
