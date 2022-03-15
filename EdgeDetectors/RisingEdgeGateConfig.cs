using System.Collections.Generic;
using TUNING;
using UnityEngine;
using STRINGS;


namespace EdgeDetectors
{
    public class RisingEdgeGateConfig : UnaryGateConfig
    {
        public static string ID = "RisingEdgeGate";
        public static string anim = "logic_rising_kanim";

        public static LocString NAME = (LocString)"Rising Edge Detector";
        public static LocString DESC = (LocString)$"Sends a single-tick {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} when input goes from {UI.FormatAsAutomationState("red", UI.AutomationState.Standby)} to {UI.FormatAsAutomationState("green", UI.AutomationState.Active)}.";
        public static LocString EFFECT = DESC;

        public override BuildingDef CreateBuildingDef() => this.CreateBuildingDef(ID, anim, height: 1);
        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<RisingEdgeGate>();
            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
        }
    }
}
