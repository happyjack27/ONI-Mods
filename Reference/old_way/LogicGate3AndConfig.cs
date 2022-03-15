// Decompiled with JetBrains decompiler
// Type: LogicGateAndConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1FE956DE-2521-4C50-80CE-B4D7F814C847
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

namespace SomeLogicGates
{
    public class LogicGate3AndConfig : LogicGateBaseConfig2
    {
        public const string ID = "LogicGate3AND";

        protected override LogicGateBase.Op GetLogicOp() => LogicGateBase.Op.And;

        protected override CellOffset[] InputPortOffsets => new CellOffset[3]
        {
    CellOffset.none,
    new CellOffset(0, 1),
    new CellOffset(1, 1)
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
                name = (string)BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_NAME,
                active = (string)BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_ACTIVE,
                inactive = (string)BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_INACTIVE
            }
        };

        public override BuildingDef CreateBuildingDef() => this.CreateBuildingDef(ID, "logic_and_kanim");
    }
}