using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using STRINGS;
using UnityEngine;

namespace KBComputing
{
    class Rom4Config : IBuildingConfig
    {
        public static string ID = "Rom4";
        public static string anim = "4rom_kanim";

        public static LocString NAME = (LocString)"4-bit ROM / PLC";
        public static LocString DESC = (LocString)"Maps 4-bit inputs to 4-bit outputs";
        public static LocString EFFECT = (LocString)"4-bit fully programmable logic gate.\n" +
            "Maps every 4-bit combination input to a 4-bit combination output.";

        public static string LOGIC_PORT00 = "Data Input";
        //},
        //new string[] {
        public static string LOGIC_PORT10 = "Data Output";

        public static readonly HashedString PORT_ID00 = new HashedString(LOGIC_PORT00);

        public static readonly HashedString PORT_ID10 = new HashedString(LOGIC_PORT10);
        public override BuildingDef CreateBuildingDef()
        {
            string id = ID;
            int width1 = 2;
            int height1 = 1;
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
                    new CellOffset(0, 0),
                    (string)LOGIC_PORT00,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_INACTIVE,
                    false,
                    LogicPortSpriteType.RibbonInput,
                    true
                    ),
            };
            buildingDef.LogicOutputPorts = new List<LogicPorts.Port>() {
                new LogicPorts.Port(
                    PORT_ID10,
                    new CellOffset(1, 0),
                    (string)LOGIC_PORT10,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_ACTIVE,
                    (string)UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_INACTIVE,
                    false,
                    LogicPortSpriteType.RibbonOutput,
                    true
                    ),
            };

            GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
            return buildingDef;
        }


        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<Rom4>();
            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
        }
    }
}
