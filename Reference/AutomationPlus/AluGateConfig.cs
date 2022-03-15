using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;

namespace AutomationPlus
{
    class AluGateConfig : IBuildingConfig
    {
        public static string ID = "AluGate";

        public override BuildingDef CreateBuildingDef()
        {
            string id = ID;
            float[] tieR0_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
            string[] refinedMetals = MATERIALS.REFINED_METALS;
            EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
            EffectorValues tieR0_2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER0;
            EffectorValues noise = none;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 2, 2, "4bit_alu_kanim", 30, 30f, tieR0_1, refinedMetals, 1600f, BuildLocationRule.Anywhere, tieR0_2, noise);
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
                LogicPorts.Port.RibbonInputPort(AluGate.INPUT_PORT_ID1, new CellOffset(0, 1), (string) AluGateStrings.INPUT_PORT1, (string) AluGateStrings.INPUT_PORT_ACTIVE, (string) AluGateStrings.INPUT_PORT_INACTIVE , true, true),
                LogicPorts.Port.RibbonInputPort(AluGate.INPUT_PORT_ID2, new CellOffset(0, 0), (string) AluGateStrings.INPUT_PORT2, (string) AluGateStrings.INPUT_PORT_ACTIVE, (string) AluGateStrings.INPUT_PORT_INACTIVE , false, true),
                new LogicPorts.Port(AluGate.OP_PORT_ID, new CellOffset(1, 1), AluGateStrings.OP_PORT_DESCRIPTION, AluGateStrings.OP_CODE_ACTIVE, AluGateStrings.OP_CODE_INACTIVE, false, LogicPortSpriteType.ControlInput, true),
            };
            buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()    {
                LogicPorts.Port.RibbonOutputPort(AluGate.OUTPUT_PORT_ID, new CellOffset(1, 0), AluGateStrings.OUTPUT_NAME, AluGateStrings.OUTPUT_ACTIVE, AluGateStrings.OUTPUT_INACTIVE, true, true),
            };
            SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Open_DoorInternal", TUNING.NOISE_POLLUTION.NOISY.TIER3);
            SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Close_DoorInternal", TUNING.NOISE_POLLUTION.NOISY.TIER3);
            GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<AluGate>();
            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
        }
    }
}
