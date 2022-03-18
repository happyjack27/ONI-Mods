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

        public static string[][] LOGIC_PORT = new string[][] {
            new string[] {
                "Input Port1",
                "Input Port2",
                "Input Port3",
                "Input Port4",
            },
            new string[] {
                "Output Port1",
                "Output Port2",
                "Not a port",
                "Control Port",
            },
        };
        public static readonly HashedString[][] PORT_ID;

        static Base4x2Config()
        {
            PORT_ID = new HashedString[2][];
            for( int i = 0; i < 2; i++)
            {
                PORT_ID[i] = new HashedString[4];
                for (int j = 0; j < 4; j++)
                {
                    PORT_ID[i][j] = new HashedString(LOGIC_PORT[i][j]);
                }
            }
        }

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
                    PORT_ID[0][0],
                    new CellOffset(0, 3),
                    (string)LOGIC_PORT[0][0],
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_INACTIVE,
                    true,
                    LogicPortSpriteType.RibbonInput,
                    true
                    ),
                new LogicPorts.Port(
                    PORT_ID[0][1],
                    new CellOffset(0, 2),
                    (string)LOGIC_PORT[0][1],
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_INACTIVE,
                    true,
                    LogicPortSpriteType.RibbonInput,
                    true
                    ),
                new LogicPorts.Port(
                    PORT_ID[0][2],
                    new CellOffset(0, 1),
                    (string)LOGIC_PORT[0][2],
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_INACTIVE,
                    true,
                    LogicPortSpriteType.RibbonInput,
                    true
                    ),
                new LogicPorts.Port(
                    PORT_ID[0][3],
                    new CellOffset(0, 0),
                    (string)LOGIC_PORT[0][3],
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_INACTIVE,
                    true,
                    LogicPortSpriteType.RibbonInput,
                    true
                    ),
                new LogicPorts.Port(
                    PORT_ID[1][3],
                    new CellOffset(1, 0),
                    (string)LOGIC_PORT[1][3],
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_INACTIVE,
                    true,
                    LogicPortSpriteType.ControlInput,
                    true
                    ),
            };
            buildingDef.LogicOutputPorts = new List<LogicPorts.Port>() {
                new LogicPorts.Port(
                    PORT_ID[1][0],
                    new CellOffset(1, 3),
                    (string)LOGIC_PORT[1][0],
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_INACTIVE,
                    true,
                    LogicPortSpriteType.RibbonOutput,
                    true
                    ),
                new LogicPorts.Port(
                    PORT_ID[1][1],
                    new CellOffset(1, 2),
                    (string)LOGIC_PORT[1][1],
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_INACTIVE,
                    true,
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
