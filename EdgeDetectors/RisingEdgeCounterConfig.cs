using System.Collections.Generic;
using TUNING;
using UnityEngine;
using STRINGS;


namespace EdgeDetectors
{
    public class RisingEdgeCounterConfig : WireToRibbonConfig
    {
        public static string ID = "RisingEdgeCounter";
        public static string anim = "logic_pulse_counter_kanim";

        public static LocString NAME = (LocString)"Rising Edge Counter";
        public static LocString DESC = (LocString)$"Counts the number of times the input goes from {UI.FormatAsAutomationState("red", UI.AutomationState.Standby)} to {UI.FormatAsAutomationState("green", UI.AutomationState.Active)}.";
        public static LocString EFFECT = DESC;

        public override BuildingDef CreateBuildingDef() => this.CreateBuildingDef(ID, anim, height: 1);
        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<RisingEdgeCounter>();
            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
        }
    }
}
