// Decompiled with JetBrains decompiler
// Type: LogicEventSender
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1FE956DE-2521-4C50-80CE-B4D7F814C847
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

internal class LogicEventSender : 
  ILogicEventSender,
  ILogicNetworkConnection,
  ILogicUIElement,
  IUniformGridObject
{
  private HashedString id;
  private int cell;
  private int logicValue;
  private System.Action<int> onValueChanged;
  private System.Action<int, bool> onConnectionChanged;
  private LogicPortSpriteType spriteType;

  public LogicEventSender(
    HashedString id,
    int cell,
    System.Action<int> on_value_changed,
    System.Action<int, bool> on_connection_changed,
    LogicPortSpriteType sprite_type)
  {
    this.id = id;
    this.cell = cell;
    this.onValueChanged = on_value_changed;
    this.onConnectionChanged = on_connection_changed;
    this.spriteType = sprite_type;
  }

  public HashedString ID => this.id;

  public int GetLogicCell() => this.cell;

  public int GetLogicValue() => this.logicValue;

  public int GetLogicUICell() => this.GetLogicCell();

  public LogicPortSpriteType GetLogicPortSpriteType() => this.spriteType;

  public Vector2 PosMin() => (Vector2) Grid.CellToPos2D(this.cell);

  public Vector2 PosMax() => (Vector2) Grid.CellToPos2D(this.cell);

  public void SetValue(int value)
  {
    this.logicValue = value;
    this.onValueChanged(value);
  }

  public void LogicTick()
  {
  }

  public void OnLogicNetworkConnectionChanged(bool connected)
  {
    if (this.onConnectionChanged == null)
      return;
    this.onConnectionChanged(this.cell, connected);
  }
}
