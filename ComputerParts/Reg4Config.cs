using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;

namespace KBComputing
{
    class Reg4Config : baseClasses.Base2x2Config
    {
        public static string ID = "Reg4";
        public static string anim = "4bit_reg_kanim";

        public static LocString NAME = (LocString)"4-bit Register";
        public static LocString DESC = (LocString)"Stores 4 bits of data.";
        public static LocString EFFECT = DESC;

        public override BuildingDef CreateBuildingDef() => this.CreateBuildingDef(ID, anim);

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<Reg4>();
            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
        }
    }
}
