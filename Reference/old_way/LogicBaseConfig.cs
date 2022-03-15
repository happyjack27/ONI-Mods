// Decompiled with JetBrains decompiler
// Type: LogicGateBaseConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1FE956DE-2521-4C50-80CE-B4D7F814C847
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

namespace SomeLogicGates
{
    public abstract class LogicBaseConfig : IBuildingConfig
    {
        protected BuildingDef CreateBuildingDef(
          string ID,
          string anim,
          int width = 2,
          int height = 2)
        {
            string id = ID;
            int width1 = width;
            int height1 = height;
            string anim1 = anim;
            float[] tieR0_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
            string[] refinedMetals = MATERIALS.REFINED_METALS;
            EffectorValues none = NOISE_POLLUTION.NONE;
            EffectorValues tieR0_2 = BUILDINGS.DECOR.PENALTY.TIER0;
            EffectorValues noise = none;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width1, height1, anim1, 10, 3f, tieR0_1, refinedMetals, 1600f, BuildLocationRule.Anywhere, tieR0_2, noise);
            buildingDef.ViewMode = OverlayModes.Logic.ID;
            buildingDef.ObjectLayer = ObjectLayer.LogicGate;
            buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
            buildingDef.ThermalConductivity = 0.05f;
            buildingDef.Floodable = false;
            buildingDef.Overheatable = false;
            buildingDef.Entombable = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.AudioSize = "small";
            buildingDef.BaseTimeUntilRepair = -1f;
            buildingDef.PermittedRotations = PermittedRotations.R360;
            buildingDef.DragBuild = true;
            LogicBase.uiSrcData = Assets.instance.logicModeUIData;
            GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
            return buildingDef;
        }

        protected abstract CellOffset[] InputPortOffsets { get; }

        protected abstract CellOffset[] OutputPortOffsets { get; }

        protected abstract CellOffset[] ControlPortOffsets { get; }

        protected abstract LogicGate.LogicGateDescriptions GetDescriptions();

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            base.DoPostConfigurePreview(def, go);
            MoveableLogicGateVisualizer logicGateVisualizer = go.AddComponent<MoveableLogicGateVisualizer>();
            logicGateVisualizer.op = this.GetLogicOp();
            logicGateVisualizer.inputPortOffsets = this.InputPortOffsets;
            logicGateVisualizer.outputPortOffsets = this.OutputPortOffsets;
            logicGateVisualizer.controlPortOffsets = this.ControlPortOffsets;
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            base.DoPostConfigureUnderConstruction(go);
            LogicGateVisualizer logicGateVisualizer = go.AddComponent<LogicGateVisualizer>();
            logicGateVisualizer.op = this.GetLogicOp();
            logicGateVisualizer.inputPortOffsets = this.InputPortOffsets;
            logicGateVisualizer.outputPortOffsets = this.OutputPortOffsets;
            logicGateVisualizer.controlPortOffsets = this.ControlPortOffsets;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Logic logicGate = go.AddComponent<Logic>();
            logicGate.op = this.GetLogicOp();
            logicGate.inputPortOffsets = this.InputPortOffsets;
            logicGate.outputPortOffsets = this.OutputPortOffsets;
            logicGate.controlPortOffsets = this.ControlPortOffsets;
            go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn)(game_object => game_object.GetComponent<Logic>().SetPortDescriptions(this.GetDescriptions()));
            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
        }
    }
}