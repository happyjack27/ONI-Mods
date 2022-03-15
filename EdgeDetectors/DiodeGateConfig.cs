using System.Collections.Generic;
using TUNING;
using UnityEngine;
using STRINGS;


namespace EdgeDetectors
{
    public class DiodeGateConfig : UnaryGateConfig
    {
        public static string ID = "DiodeGate";

        public static LocString NAME = (LocString)"Diode";
        public static LocString DESC = (LocString)"Copies input to output.  (Use to make OR gates with any number of inputs, or to decouple logic.)";
        public static LocString EFFECT = DESC;

        public override BuildingDef CreateBuildingDef() => this.CreateBuildingDef(ID, anim, height: 1);
        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<DiodeGate>();
            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
        }
    }
}
