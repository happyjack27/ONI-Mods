using System.Collections.Generic;
using TUNING;
using UnityEngine;
using STRINGS;


namespace KBComputing
{
    public class EdgeCounterConfig : WireToRibbonConfig
    {
        public static string ID = "EdgeCounterKBComputing";
        public static string anim = "logic_pulse_counter_kanim";

        public static LocString NAME = (LocString)"Edge Counter";
        public static LocString DESC = (LocString)$"Counts the number of times the input chnages.";
        public static LocString EFFECT = DESC;

        public override BuildingDef CreateBuildingDef() => this.CreateBuildingDef(ID, anim, height: 1);
        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<EdgeCounter>();
            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
        }
    }
}
