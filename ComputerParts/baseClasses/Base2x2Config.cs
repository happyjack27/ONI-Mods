using System.Collections.Generic;
using TUNING;
using UnityEngine;
using STRINGS;


namespace KBComputing.baseClasses
{
    public abstract class Base2x2Config : IBuildingConfig
    {
        public static string anim = "2x2base_kanim";

        public static LocString NAME = (LocString)"Base2x2";
        public static LocString DESC = (LocString)"Generic unary operation";
        public static LocString EFFECT = (LocString)"Generic unary operation";

        public static string LOGIC_INPUT_PORT = $"Input Port";
        public static string LOGIC_OUTPUT_PORT = $"Output Port";
        public static string LOGIC_CONTROL_PORT1 = $"Write Enable";
        public static string LOGIC_CONTROL_PORT2 = $"Read Enable";
        public static readonly HashedString INPUT_PORT_ID = new HashedString(LOGIC_INPUT_PORT);
        public static readonly HashedString OUTPUT_PORT_ID = new HashedString(LOGIC_OUTPUT_PORT);
        public static readonly HashedString CONTROL_PORT_ID1 = new HashedString(LOGIC_CONTROL_PORT1);
        public static readonly HashedString CONTROL_PORT_ID2 = new HashedString(LOGIC_CONTROL_PORT2);

        public static LocString OUTPUT_NAME = "Output";
        public static LocString OUTPUT_ACTIVE = ($"Sends a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} on each bit based on the stored value.");
        public static LocString OUTPUT_INACTIVE = ($"Sends a {UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby)} on each bit based on the stored value.");

        public static LocString INPUT_NAME = "Input";
        public static LocString INPUT_ACTIVE = $"The input number based on binary format.";
        public static LocString INPUT_INACTIVE = $"The input number based on binary format.";

        public static LocString CONTROL_PORT_WRITE = "Write Enable";
        public static LocString CONTROL_PORT_WRITE_ACTIVE = $"Stores the input value.";
        public static LocString CONTROL_PORT_WRITE_INACTIVE = $"Ignores the input value.";

        public static LocString CONTROL_PORT_READ = "Read Enable";
        public static LocString CONTROL_PORT_READ_ACTIVE = $"Sends the stored value to the output.";
        public static LocString CONTROL_PORT_READ_INACTIVE = $"Sends all {UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby)} to the output.";

        protected BuildingDef CreateBuildingDef(
            string ID,
            string anim,
            int width = 2,
            int height = 2
        )
        {
            string id = ID;
            int width1 = width;
            int height1 = height;
            string anim1 = anim;
            float[] tieR0_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
            string[] refinedMetals = MATERIALS.REFINED_METALS;
            EffectorValues none = NOISE_POLLUTION.NONE;
            EffectorValues tieR0_2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER0;
            EffectorValues noise = none;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width1, height1, anim1, 10, 3f, tieR0_1, refinedMetals, 1600f, BuildLocationRule.Anywhere, tieR0_2, noise);
            buildingDef.ViewMode = OverlayModes.Logic.ID;
            buildingDef.ObjectLayer = ObjectLayer.LogicGate;
            buildingDef.SceneLayer = Grid.SceneLayer.LogicGatesFront;
            buildingDef.ThermalConductivity = 0.05f;
            buildingDef.Floodable = false;
            buildingDef.Overheatable = false;
            buildingDef.Entombable = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.AudioSize = "small";
            buildingDef.BaseTimeUntilRepair = -1f;
            buildingDef.PermittedRotations = PermittedRotations.R360;
            buildingDef.DragBuild = true;
            LogicGateBase.uiSrcData = Assets.instance.logicModeUIData;
            buildingDef.LogicInputPorts = new List<LogicPorts.Port>() {
                new LogicPorts.Port(
                    Base2x2Config.INPUT_PORT_ID,
                    new CellOffset(0, 1),
                    (string)INPUT_NAME,
                    (string)INPUT_ACTIVE,
                    (string)INPUT_INACTIVE,
                    true,
                    LogicPortSpriteType.RibbonInput,
                    true
                    ),
                new LogicPorts.Port(
                    Base2x2Config.CONTROL_PORT_ID1,
                    new CellOffset(0, 0),
                    (string)CONTROL_PORT_WRITE,
                    (string)CONTROL_PORT_WRITE_ACTIVE,
                    (string)CONTROL_PORT_WRITE_INACTIVE,
                    false,
                    LogicPortSpriteType.ControlInput,
                    true
                    ),
                new LogicPorts.Port(
                    Base2x2Config.CONTROL_PORT_ID2,
                    new CellOffset(1, 0),
                    (string)CONTROL_PORT_READ,
                    (string)CONTROL_PORT_READ_ACTIVE,
                    (string)CONTROL_PORT_READ_INACTIVE,
                    false,
                    LogicPortSpriteType.ControlInput,
                    true
                    ),
            };
            buildingDef.LogicOutputPorts = new List<LogicPorts.Port>() {
                new LogicPorts.Port(
                    Base2x2Config.OUTPUT_PORT_ID,
                    new CellOffset(1, 1),
                    (string)OUTPUT_NAME,
                    (string)OUTPUT_ACTIVE,
                    (string)OUTPUT_INACTIVE,
                    true, 
                    LogicPortSpriteType.RibbonOutput,
                    true
                ),
            };

            GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
            return buildingDef;
        }

        //public abstract void DoPostConfigureComplete(GameObject go);
    }
}
