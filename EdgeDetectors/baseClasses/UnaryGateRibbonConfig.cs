using System.Collections.Generic;
using TUNING;
using UnityEngine;
using STRINGS;


namespace EdgeDetectors
{
    public abstract class UnaryGateRibbonConfig : IBuildingConfig
    {
        public static string anim = "logic_unary_kanim";

        public static LocString NAME = (LocString)"UnaryGateRibbon";
        public static LocString DESC = (LocString)"Generic unary operation";
        public static LocString EFFECT = (LocString)"Generic unary operation";

        public static string LOGIC_INPUT_PORT = $"Input Port";
        public static string LOGIC_PORT_OUTPUT = $"Output Port";
        public static readonly HashedString INPUT_PORT_ID = new HashedString(LOGIC_INPUT_PORT);
        public static readonly HashedString OUTPUT_PORT_ID = new HashedString(LOGIC_PORT_OUTPUT);

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
                    UnaryGateRibbonConfig.INPUT_PORT_ID, 
                    new CellOffset(0, 0),
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_NAME,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_INACTIVE,
                    true, 
                    LogicPortSpriteType.RibbonInput, 
                    true
                    ),
            };
            buildingDef.LogicOutputPorts = new List<LogicPorts.Port>() {
                new LogicPorts.Port(
                    UnaryGateRibbonConfig.OUTPUT_PORT_ID,
                    new CellOffset(1, 0),
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_NAME,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_INACTIVE,
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
