using HarmonyLib;
//using PeterHan.PLib.UI;
using System;
using System.Reflection;
//using util;
using static STRINGS.UI.BUILDCATEGORIES;

namespace EdgeDetectors
{
    public class Patches : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);

        }


        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch("Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Prefix()
            {
            }

            public static void Postfix()
            {
                var assem = Assembly.GetExecutingAssembly();
                Debug.Log($"Loaded MOD: {assem?.FullName}");
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
        public class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                StringUtils.AddBuildingStrings(DiodeGateConfig.ID, DiodeGateConfig.NAME, DiodeGateConfig.DESC, DiodeGateConfig.EFFECT);
                StringUtils.AddStringTypes(typeof(DiodeGateConfig));
                ModUtil.AddBuildingToPlanScreen(PlanMenuCategory.Automation, DiodeGateConfig.ID);

                StringUtils.AddBuildingStrings(RisingEdgeGateConfig.ID, RisingEdgeGateConfig.NAME, RisingEdgeGateConfig.DESC, RisingEdgeGateConfig.EFFECT);
                StringUtils.AddStringTypes(typeof(RisingEdgeGateConfig));
                ModUtil.AddBuildingToPlanScreen(PlanMenuCategory.Automation, RisingEdgeGateConfig.ID);

                StringUtils.AddBuildingStrings(FallingEdgeGateConfig.ID, FallingEdgeGateConfig.NAME, FallingEdgeGateConfig.DESC, FallingEdgeGateConfig.EFFECT);
                StringUtils.AddStringTypes(typeof(FallingEdgeGateConfig));
                ModUtil.AddBuildingToPlanScreen(PlanMenuCategory.Automation, FallingEdgeGateConfig.ID);

                StringUtils.AddBuildingStrings(RisingEdgeCounterConfig.ID, RisingEdgeCounterConfig.NAME, RisingEdgeCounterConfig.DESC, RisingEdgeCounterConfig.EFFECT);
                StringUtils.AddStringTypes(typeof(RisingEdgeCounterConfig));
                ModUtil.AddBuildingToPlanScreen(PlanMenuCategory.Automation, RisingEdgeCounterConfig.ID);
            }

            /*
            [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
            public static class SideScreenCreator
            {
                internal static void Postfix()
                {
                    PUIUtils.AddSideScreenContent<AluGateSideScreen>();
                    PUIUtils.AddSideScreenContent<DisplayAdaptorSideScreen>();
                }
            }
            */

            [HarmonyPatch(typeof(Db))]
            [HarmonyPatch("Initialize")]
            public static class Db_Initialize_Patch
            {
                public static void Postfix()
                {
                    Db.Get().Techs.Get("GenericSensors").unlockedItemIDs.Add(DiodeGateConfig.ID);
                    Db.Get().Techs.Get("LogicCircuits").unlockedItemIDs.Add(RisingEdgeGateConfig.ID);
                    Db.Get().Techs.Get("LogicCircuits").unlockedItemIDs.Add(FallingEdgeGateConfig.ID);
                    Db.Get().Techs.Get("Multiplexing").unlockedItemIDs.Add(RisingEdgeCounterConfig.ID);
                    //BuildingUtils.AddBuildingToTechnology("Multiplexing", LogicGateDiodeConfig.ID);
                    //BuildingUtils.AddBuildingToTechnology("LogicCircuits", LogicGate3AndConfig.ID);
                }
            }
        }
    }

}
