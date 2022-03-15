// Decompiled with JetBrains decompiler
// Type: LogicVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1FE956DE-2521-4C50-80CE-B4D7F814C847
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SomeLogicGates
{
    [SkipSaveFileSerialization]
    public class LogicVisualizer : LogicBase
    {
        private List<LogicVisualizer.IOVisualizer> visChildren = new List<LogicVisualizer.IOVisualizer>();

        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.Register();
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            this.Unregister();
        }

        private void Register()
        {
            this.Unregister();
            this.visChildren.Add(new LogicVisualizer.IOVisualizer(this.OutputCellOne, false));
            if (this.RequiresFourOutputs)
            {
                this.visChildren.Add(new LogicVisualizer.IOVisualizer(this.OutputCellTwo, false));
                this.visChildren.Add(new LogicVisualizer.IOVisualizer(this.OutputCellThree, false));
                this.visChildren.Add(new LogicVisualizer.IOVisualizer(this.OutputCellFour, false));
            }
            this.visChildren.Add(new LogicVisualizer.IOVisualizer(this.InputCellOne, true));
            if (this.RequiresTwoInputs)
                this.visChildren.Add(new LogicVisualizer.IOVisualizer(this.InputCellTwo, true));
            else if (this.RequiresFourInputs)
            {
                this.visChildren.Add(new LogicVisualizer.IOVisualizer(this.InputCellTwo, true));
                this.visChildren.Add(new LogicVisualizer.IOVisualizer(this.InputCellThree, true));
                this.visChildren.Add(new LogicVisualizer.IOVisualizer(this.InputCellFour, true));
            }
            if (this.RequiresControlInputs)
            {
                this.visChildren.Add(new LogicVisualizer.IOVisualizer(this.ControlCellOne, true));
                this.visChildren.Add(new LogicVisualizer.IOVisualizer(this.ControlCellTwo, true));
            }
            LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
            foreach (LogicVisualizer.IOVisualizer visChild in this.visChildren)
                logicCircuitManager.AddVisElem((ILogicUIElement)visChild);
        }

        private void Unregister()
        {
            LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
            foreach (LogicVisualizer.IOVisualizer visChild in this.visChildren)
                logicCircuitManager.RemoveVisElem((ILogicUIElement)visChild);
            this.visChildren.Clear();
        }

        private class IOVisualizer : ILogicUIElement, IUniformGridObject
        {
            private int cell;
            private LogicPortSpriteType type;

            public IOVisualizer(int cell, LogicPortSpriteType type)
            {
                this.cell = cell;
                this.type = type;
            }

            public int GetLogicUICell() => this.cell;

            public LogicPortSpriteType GetLogicPortSpriteType() => this.type;

            public Vector2 PosMin() => (Vector2)Grid.CellToPos2D(this.cell);

            public Vector2 PosMax() => this.PosMin();
        }
    }
}
