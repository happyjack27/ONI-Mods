// Decompiled with JetBrains decompiler
// Type: LogicGateBufferConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1FE956DE-2521-4C50-80CE-B4D7F814C847
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

namespace SomeLogicGates
{
    public class LogicGateDiodeConfig : LogicGateBaseConfig2
    {
        public const string ID = "LogicGateDiode";

        protected override LogicGateBase.Op GetLogicOp() => LogicGateBase.Op.CustomSingle;

        protected override CellOffset[] InputPortOffsets => new CellOffset[1]
        {
    CellOffset.none
        };

        protected override CellOffset[] OutputPortOffsets => new CellOffset[1]
        {
    new CellOffset(1, 0)
        };

        protected override CellOffset[] ControlPortOffsets => (CellOffset[])null;

        protected override LogicGate2.LogicGateDescriptions GetDescriptions() => new LogicGate2.LogicGateDescriptions()
        {
            outputOne = new LogicGate2.LogicGateDescriptions.Description()
            {
                name = (string)BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_NAME,
                active = (string)BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_ACTIVE,
                inactive = (string)BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_INACTIVE
            }
        };

        public override BuildingDef CreateBuildingDef() => this.CreateBuildingDef(ID, "logic_buffer_kanim", height: 1);

        public override void DoPostConfigureComplete(GameObject go)
        {
            LogicGateDiode logicGateBuffer = go.AddComponent<LogicGateDiode>();
            logicGateBuffer.op = this.GetLogicOp();
            logicGateBuffer.inputPortOffsets = this.InputPortOffsets;
            logicGateBuffer.outputPortOffsets = this.OutputPortOffsets;
            logicGateBuffer.controlPortOffsets = this.ControlPortOffsets;
            go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn)(game_object => game_object.GetComponent<LogicGateDiode>().SetPortDescriptions(this.GetDescriptions()));
            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
        }
    }
}