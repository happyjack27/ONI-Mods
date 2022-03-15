using System.Collections.Generic;
using TUNING;
using UnityEngine;
using STRINGS;


namespace EdgeDetectors
{
    public class FallingEdgeGateConfig : UnaryGateConfig
    {
        public static string ID = "FallingEdgeGate";
        public static string anim = "logic_falling_kanim";

        public static LocString NAME = (LocString)"Falling Edge Detector";
        public static LocString DESC = (LocString)$"Sends a single-tick {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} when input goes from {UI.FormatAsAutomationState("green", UI.AutomationState.Active)} to {UI.FormatAsAutomationState("red", UI.AutomationState.Standby)}.";
        public static LocString EFFECT = DESC;

        public override BuildingDef CreateBuildingDef() => this.CreateBuildingDef(ID, anim, height: 1);
        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<FallingEdgeGate>();
            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
        }
    }
}
