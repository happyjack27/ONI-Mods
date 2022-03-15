// Decompiled with JetBrains decompiler
// Type: LogicWire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1FE956DE-2521-4C50-80CE-B4D7F814C847
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/LogicWire")]
public class LogicWire : 
  KMonoBehaviour,
  IFirstFrameCallback,
  IHaveUtilityNetworkMgr,
  IBridgedNetworkItem,
  IBitRating,
  IDisconnectable
{
  [SerializeField]
  public LogicWire.BitDepth MaxBitDepth;
  [SerializeField]
  private bool disconnected = true;
  public static readonly KAnimHashedString OutlineSymbol = new KAnimHashedString("outline");
  private static readonly EventSystem.IntraObjectHandler<LogicWire> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<LogicWire>((System.Action<LogicWire, object>) ((component, data) => component.OnBuildingBroken(data)));
  private static readonly EventSystem.IntraObjectHandler<LogicWire> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<LogicWire>((System.Action<LogicWire, object>) ((component, data) => component.OnBuildingFullyRepaired(data)));
  private System.Action firstFrameCallback;

  public static int GetBitDepthAsInt(LogicWire.BitDepth rating)
  {
    if (rating == LogicWire.BitDepth.OneBit)
      return 1;
    return rating == LogicWire.BitDepth.FourBit ? 4 : 0;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Game.Instance.logicCircuitSystem.AddToNetworks(Grid.PosToCell(this.transform.GetPosition()), (object) this, false);
    this.Subscribe<LogicWire>(774203113, LogicWire.OnBuildingBrokenDelegate);
    this.Subscribe<LogicWire>(-1735440190, LogicWire.OnBuildingFullyRepairedDelegate);
    this.Connect();
    this.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(LogicWire.OutlineSymbol, false);
  }

  protected override void OnCleanUp()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    BuildingComplete component = this.GetComponent<BuildingComplete>();
    if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || (UnityEngine.Object) Grid.Objects[cell, (int) component.Def.ReplacementLayer] == (UnityEngine.Object) null)
      Game.Instance.logicCircuitSystem.RemoveFromNetworks(cell, (object) this, false);
    this.Unsubscribe<LogicWire>(774203113, LogicWire.OnBuildingBrokenDelegate);
    this.Unsubscribe<LogicWire>(-1735440190, LogicWire.OnBuildingFullyRepairedDelegate);
    base.OnCleanUp();
  }

  public bool IsConnected => Game.Instance.logicCircuitSystem.GetNetworkForCell(Grid.PosToCell(this.transform.GetPosition())) is LogicCircuitNetwork;

  public bool IsDisconnected() => this.disconnected;

  public bool Connect()
  {
    BuildingHP component = this.GetComponent<BuildingHP>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || component.HitPoints > 0)
    {
      this.disconnected = false;
      Game.Instance.logicCircuitSystem.ForceRebuildNetworks();
    }
    return !this.disconnected;
  }

  public void Disconnect()
  {
    this.disconnected = true;
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.WireDisconnected);
    Game.Instance.logicCircuitSystem.ForceRebuildNetworks();
  }

  public UtilityConnections GetWireConnections() => Game.Instance.logicCircuitSystem.GetConnections(Grid.PosToCell(this.transform.GetPosition()), true);

  public string GetWireConnectionsString() => Game.Instance.logicCircuitSystem.GetVisualizerString(this.GetWireConnections());

  private void OnBuildingBroken(object data) => this.Disconnect();

  private void OnBuildingFullyRepaired(object data) => this.Connect();

  public void SetFirstFrameCallback(System.Action ffCb)
  {
    this.firstFrameCallback = ffCb;
    this.StartCoroutine(this.RunCallback());
  }

  private IEnumerator RunCallback()
  {
    yield return (object) null;
    if (this.firstFrameCallback != null)
    {
      this.firstFrameCallback();
      this.firstFrameCallback = (System.Action) null;
    }
    yield return (object) null;
  }

  public LogicWire.BitDepth GetMaxBitRating() => this.MaxBitDepth;

  public IUtilityNetworkMgr GetNetworkManager() => (IUtilityNetworkMgr) Game.Instance.logicCircuitSystem;

  public void AddNetworks(ICollection<UtilityNetwork> networks)
  {
    UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(Grid.PosToCell(this.transform.GetPosition()));
    if (networkForCell == null)
      return;
    networks.Add(networkForCell);
  }

  public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
  {
    UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(Grid.PosToCell(this.transform.GetPosition()));
    return networks.Contains(networkForCell);
  }

  public int GetNetworkCell() => Grid.PosToCell((KMonoBehaviour) this);

  public enum BitDepth
  {
    OneBit,
    FourBit,
    NumRatings,
  }
}
