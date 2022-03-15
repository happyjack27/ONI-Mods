// Decompiled with JetBrains decompiler
// Type: LogicEventHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1FE956DE-2521-4C50-80CE-B4D7F814C847
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;

internal class LogicEventHandler : 
  ILogicEventReceiver,
  ILogicNetworkConnection,
  ILogicUIElement,
  IUniformGridObject
{
  private int cell;
  private int value;
  private System.Action<int> onValueChanged;
  private System.Action<int, bool> onConnectionChanged;
  private LogicPortSpriteType spriteType;

  public LogicEventHandler(
    int cell,
    System.Action<int> on_value_changed,
    System.Action<int, bool> on_connection_changed,
    LogicPortSpriteType sprite_type)
  {
    this.cell = cell;
    this.onValueChanged = on_value_changed;
    this.onConnectionChanged = on_connection_changed;
    this.spriteType = sprite_type;
  }

  public void ReceiveLogicEvent(int value)
  {
    this.TriggerAudio(value);
    this.value = value;
    this.onValueChanged(value);
  }

  public int Value => this.value;

  public int GetLogicUICell() => this.cell;

  public LogicPortSpriteType GetLogicPortSpriteType() => this.spriteType;

  public Vector2 PosMin() => (Vector2) Grid.CellToPos2D(this.cell);

  public Vector2 PosMax() => (Vector2) Grid.CellToPos2D(this.cell);

  public int GetLogicCell() => this.cell;

  private void TriggerAudio(int new_value)
  {
    LogicCircuitNetwork networkForCell = Game.Instance.logicCircuitManager.GetNetworkForCell(this.cell);
    SpeedControlScreen instance1 = SpeedControlScreen.Instance;
    if (networkForCell == null || new_value == this.value || !((UnityEngine.Object) instance1 != (UnityEngine.Object) null) || instance1.IsPaused || KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayAutomation) && KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) != 1 && OverlayScreen.Instance.GetMode() != OverlayModes.Logic.ID)
      return;
    string name = "Logic_Building_Toggle";
    if (!CameraController.Instance.IsAudibleSound((Vector2) Grid.CellToPosCCC(this.cell, Grid.SceneLayer.BuildingFront)))
      return;
    LogicCircuitNetwork.LogicSoundPair logicSoundPair = new LogicCircuitNetwork.LogicSoundPair();
    Dictionary<int, LogicCircuitNetwork.LogicSoundPair> logicSoundRegister = LogicCircuitNetwork.logicSoundRegister;
    int id = networkForCell.id;
    if (!logicSoundRegister.ContainsKey(id))
    {
      logicSoundRegister.Add(id, logicSoundPair);
    }
    else
    {
      logicSoundPair.playedIndex = logicSoundRegister[id].playedIndex;
      logicSoundPair.lastPlayed = logicSoundRegister[id].lastPlayed;
    }
    if (logicSoundPair.playedIndex < 2)
    {
      logicSoundRegister[id].playedIndex = logicSoundPair.playedIndex + 1;
    }
    else
    {
      logicSoundRegister[id].playedIndex = 0;
      logicSoundRegister[id].lastPlayed = Time.time;
    }
    float num1 = (float) (((double) Time.time - (double) logicSoundPair.lastPlayed) / 3.0);
    EventInstance instance2 = KFMOD.BeginOneShot(GlobalAssets.GetSound(name), Grid.CellToPos(this.cell));
    int num2 = (int) instance2.setParameterByName("logic_volumeModifer", num1);
    int num3 = (int) instance2.setParameterByName("wireCount", (float) (networkForCell.WireCount % 24));
    int num4 = (int) instance2.setParameterByName("enabled", (float) new_value);
    KFMOD.EndOneShot(instance2);
  }

  public void OnLogicNetworkConnectionChanged(bool connected)
  {
    if (this.onConnectionChanged == null)
      return;
    this.onConnectionChanged(this.cell, connected);
  }
}
