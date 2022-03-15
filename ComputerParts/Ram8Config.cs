using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;

namespace KBComputing
{
    class Ram8Config : baseClasses.Base4x2Config
    {
        public static string ID = "Ram8";
        public static string anim = "8bit_ram_module_kanim";

        public static LocString NAME = (LocString)"8-bit RAM Module";
        public static LocString DESC = (LocString)"Stores 256 bytes of data.";
        public static LocString EFFECT = (LocString)"Addressable random access memory. Set the address bits and set the Read Enable bit (First bit on the control input) to read or the Write Enable bit (Second bit on the control input) to write, or both for a zero-delay buffer.";

        public override BuildingDef CreateBuildingDef() => this.CreateBuildingDef(ID, anim);

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<Ram8>();
            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
        }
    }
}
