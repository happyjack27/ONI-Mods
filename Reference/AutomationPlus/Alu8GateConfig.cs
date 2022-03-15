using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;

namespace AutomationPlus
{
    class Alu8GateConfig : IBuildingConfig
    {
        public static string ID = "Alu8Gate";

        public override BuildingDef CreateBuildingDef()
        {
            string id = ID;
            float[] tieR0_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
            string[] refinedMetals = MATERIALS.REFINED_METALS;
            EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
            EffectorValues tieR0_2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER0;
            EffectorValues noise = none;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 2,4, "8bit_alu_kanim", 30, 30f, tieR0_1, refinedMetals, 1600f, BuildLocationRule.Anywhere, tieR0_2, noise);
            buildingDef.Overheatable = false;
            buildingDef.Floodable = false;
            buildingDef.Entombable = false;
            buildingDef.PermittedRotations = PermittedRotations.R360;
            buildingDef.ViewMode = OverlayModes.Logic.ID;
            buildingDef.AudioCategory = "Metal";
            buildingDef.ObjectLayer = ObjectLayer.LogicGate;
            buildingDef.SceneLayer = Grid.SceneLayer.LogicGatesFront;
            buildingDef.AlwaysOperational = true;
            buildingDef.LogicInputPorts = new List<LogicPorts.Port>()   {
                LogicPorts.Port.RibbonInputPort(Alu8Gate.INPUT_PORT_ID1B, new CellOffset(0, 3), (string) Alu8GateStrings.INPUT_PORT1B, (string) Alu8GateStrings.INPUT_PORT_ACTIVE, (string) Alu8GateStrings.INPUT_PORT_INACTIVE , true, true),
                LogicPorts.Port.RibbonInputPort(AluGate.INPUT_PORT_ID1, new CellOffset(0, 2), (string) Alu8GateStrings.INPUT_PORT1, (string) Alu8GateStrings.INPUT_PORT_ACTIVE, (string) Alu8GateStrings.INPUT_PORT_INACTIVE , true, true),
                LogicPorts.Port.RibbonInputPort(Alu8Gate.INPUT_PORT_ID2B, new CellOffset(0, 1), (string) Alu8GateStrings.INPUT_PORT2B, (string) Alu8GateStrings.INPUT_PORT_ACTIVE, (string) Alu8GateStrings.INPUT_PORT_INACTIVE , true, true),
                LogicPorts.Port.RibbonInputPort(AluGate.INPUT_PORT_ID2, new CellOffset(0, 0), (string) Alu8GateStrings.INPUT_PORT2, (string) Alu8GateStrings.INPUT_PORT_ACTIVE, (string) Alu8GateStrings.INPUT_PORT_INACTIVE , true, true),
                new LogicPorts.Port(AluGate.OP_PORT_ID, new CellOffset(1, 0), Alu8GateStrings.OP_PORT_DESCRIPTION, Alu8GateStrings.OP_CODE_ACTIVE, Alu8GateStrings.OP_CODE_INACTIVE, false, LogicPortSpriteType.ControlInput, true),
            };
            buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()    {
                LogicPorts.Port.RibbonOutputPort(Alu8Gate.OUTPUT_PORT_IDB, new CellOffset(1, 3), Alu8GateStrings.OUTPUT_NAMEB, Alu8GateStrings.OUTPUT_ACTIVE, Alu8GateStrings.OUTPUT_INACTIVE, true, true),
                LogicPorts.Port.RibbonOutputPort(AluGate.OUTPUT_PORT_ID, new CellOffset(1, 2), Alu8GateStrings.OUTPUT_NAME, Alu8GateStrings.OUTPUT_ACTIVE, Alu8GateStrings.OUTPUT_INACTIVE, true, true),
            };
            SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Open_DoorInternal", TUNING.NOISE_POLLUTION.NOISY.TIER3);
            SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Close_DoorInternal", TUNING.NOISE_POLLUTION.NOISY.TIER3);
            GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<Alu8Gate>();
            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
        }
    }
}
