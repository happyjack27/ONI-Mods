using System.Collections.Generic;
using TUNING;
using UnityEngine;
using STRINGS;


namespace KBComputing.baseClasses
{
    public abstract class Base4x2Config : IBuildingConfig
    {
        public static string anim = "4x2_kanim";

        public static LocString NAME = (LocString)"Base4x2";
        public static LocString DESC = (LocString)"Generic";
        public static LocString EFFECT = DESC;

        //public static string[][] LOGIC_PORT = new string[][] {
        //  new string[] {

        public static string LOGIC_PORT00 = "Input Port1";
        public static string LOGIC_PORT01 = "Input Port2";
        public static string LOGIC_PORT02 = "Input Port3";
        public static string LOGIC_PORT03 = "Input Port4";
        //},
        //new string[] {
        public static string LOGIC_PORT10 = "Output Port1";
        public static string LOGIC_PORT11 = "Output Port2";
        public static string LOGIC_PORT12 = "Not a port";
        public static string LOGIC_PORT13 = "Control Port";
        //},
        //};
        public static readonly HashedString PORT_ID00 = new HashedString(LOGIC_PORT00);
        public static readonly HashedString PORT_ID01 = new HashedString(LOGIC_PORT01);
        public static readonly HashedString PORT_ID02 = new HashedString(LOGIC_PORT02);
        public static readonly HashedString PORT_ID03 = new HashedString(LOGIC_PORT03);

        public static readonly HashedString PORT_ID10 = new HashedString(LOGIC_PORT10);
        public static readonly HashedString PORT_ID11 = new HashedString(LOGIC_PORT11);
        public static readonly HashedString PORT_ID12 = new HashedString(LOGIC_PORT12);
        public static readonly HashedString PORT_ID13 = new HashedString(LOGIC_PORT13);

        protected BuildingDef CreateBuildingDef(
            string ID,
            string anim,
            int width = 2,
            int height = 4
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
                    PORT_ID00,
                    new CellOffset(0, 3),
                    (string)LOGIC_PORT00,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_INACTIVE,
                    false,
                    LogicPortSpriteType.RibbonInput,
                    true
                    ),
                new LogicPorts.Port(
                    PORT_ID01,
                    new CellOffset(0, 2),
                    (string)LOGIC_PORT01,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_INACTIVE,
                    false,
                    LogicPortSpriteType.RibbonInput,
                    true
                    ),
                new LogicPorts.Port(
                    PORT_ID02,
                    new CellOffset(0, 1),
                    (string)LOGIC_PORT02,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_INACTIVE,
                    true,
                    LogicPortSpriteType.RibbonInput,
                    true
                    ),
                new LogicPorts.Port(
                    PORT_ID03,
                    new CellOffset(0, 0),
                    (string)LOGIC_PORT03,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_INACTIVE,
                    true,
                    LogicPortSpriteType.RibbonInput,
                    true
                    ),
                new LogicPorts.Port(
                    PORT_ID13,
                    new CellOffset(1, 0),
                    (string)LOGIC_PORT13,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_INACTIVE,
                    true,
                    LogicPortSpriteType.ControlInput,
                    true
                    ),
            };
            buildingDef.LogicOutputPorts = new List<LogicPorts.Port>() {
                new LogicPorts.Port(
                    PORT_ID10,
                    new CellOffset(1, 3),
                    (string)LOGIC_PORT10,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_INACTIVE,
                    false,
                    LogicPortSpriteType.RibbonOutput,
                    true
                    ),
                new LogicPorts.Port(
                    PORT_ID11,
                    new CellOffset(1, 2),
                    (string)LOGIC_PORT11,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_INACTIVE,
                    false,
                    LogicPortSpriteType.RibbonOutput,
                    true
                    ),
                /*
                new LogicPorts.Port(
                    Base4x2Config.PORT_ID[1][2],
                    new CellOffset(1, 1),
                    (string)LOGIC_PORT[1][2],
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_INACTIVE,
                    true,
                    LogicPortSpriteType.RibbonOutput,
                    true
                    ),
                */
            };

            GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
            return buildingDef;
        }

        //public abstract void DoPostConfigureComplete(GameObject go);
    }
}
