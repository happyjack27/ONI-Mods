using HarmonyLib;
using PeterHan.PLib.UI;
//using PeterHan.PLib.UI;
using System;
using System.Reflection;
using KBComputing.util;
//using util;
using static STRINGS.UI.BUILDCATEGORIES;

namespace KBComputing
{
    public class Patches : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);

        }


        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class SideScreenCreator
        {
            internal static void Postfix()
            {
                PUIUtils.AddSideScreenContent<SideScreens.MemoryContentsSideScreen>();
                PUIUtils.AddSideScreenContent<SideScreens.Rom4SideScreen>();

                PUIUtils.AddSideScreenContent<SideScreens.Stack8SideScreen>();

                //PUIUtils.AddSideScreenContent<MultiplexerSideScreen>();
            }
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
                StringUtils.AddBuildingStrings(Reg4Config.ID, Reg4Config.NAME, Reg4Config.DESC, Reg4Config.EFFECT);
                StringUtils.AddStringTypes(typeof(Reg4Config));
                ModUtil.AddBuildingToPlanScreen(PlanMenuCategory.Automation, Reg4Config.ID);

                StringUtils.AddBuildingStrings(TDeMuxConfig.ID, TDeMuxConfig.NAME, TDeMuxConfig.DESC, TDeMuxConfig.EFFECT);
                StringUtils.AddStringTypes(typeof(TDeMuxConfig));
                ModUtil.AddBuildingToPlanScreen(PlanMenuCategory.Automation, TDeMuxConfig.ID);

                StringUtils.AddBuildingStrings(TMuxConfig.ID, TMuxConfig.NAME, TMuxConfig.DESC, TMuxConfig.EFFECT);
                StringUtils.AddStringTypes(typeof(TMuxConfig));
                ModUtil.AddBuildingToPlanScreen(PlanMenuCategory.Automation, TMuxConfig.ID);

                StringUtils.AddBuildingStrings(EdgeCounterConfig.ID, EdgeCounterConfig.NAME, EdgeCounterConfig.DESC, EdgeCounterConfig.EFFECT);
                StringUtils.AddStringTypes(typeof(EdgeCounterConfig));
                ModUtil.AddBuildingToPlanScreen(PlanMenuCategory.Automation, EdgeCounterConfig.ID);

                
                StringUtils.AddBuildingStrings(Ram8Config.ID, Ram8Config.NAME, Ram8Config.DESC, Ram8Config.EFFECT);
                StringUtils.AddStringTypes(typeof(Ram8Config));
                ModUtil.AddBuildingToPlanScreen(PlanMenuCategory.Automation, Ram8Config.ID);

                StringUtils.AddBuildingStrings(Rom4Config.ID, Rom4Config.NAME, Rom4Config.DESC, Rom4Config.EFFECT);
                StringUtils.AddStringTypes(typeof(Rom4Config));
                ModUtil.AddBuildingToPlanScreen(PlanMenuCategory.Automation, Rom4Config.ID);

                
                StringUtils.AddBuildingStrings(Stack8Config.ID, Stack8Config.NAME, Stack8Config.DESC, Stack8Config.EFFECT);
                StringUtils.AddStringTypes(typeof(Stack8Config));
                ModUtil.AddBuildingToPlanScreen(PlanMenuCategory.Automation, Stack8Config.ID);
                
                

                /*
                StringUtils.AddBuildingStrings(Base2x2EveryTickConfig.ID, Base2x2EveryTickConfig.NAME, Base2x2EveryTickConfig.DESC, Base2x2EveryTickConfig.EFFECT);
                StringUtils.AddStringTypes(typeof(Base2x2EveryTickConfig));
                ModUtil.AddBuildingToPlanScreen(PlanMenuCategory.Automation, Base2x2EveryTickConfig.ID);

                StringUtils.AddBuildingStrings(Base2x2OnChangeConfig.ID, Base2x2OnChangeConfig.NAME, Base2x2OnChangeConfig.DESC, Base2x2EveryTickConfig.EFFECT);
                StringUtils.AddStringTypes(typeof(Base2x2OnChangeConfig));
                ModUtil.AddBuildingToPlanScreen(PlanMenuCategory.Automation, Base2x2OnChangeConfig.ID);
                */
            }

            [HarmonyPatch(typeof(Db))]
            [HarmonyPatch("Initialize")]
            public static class Db_Initialize_Patch
            {
                public static void Postfix()
                {
                    Db.Get().Techs.Get("Multiplexing").unlockedItemIDs.Add(Reg4Config.ID);
                    Db.Get().Techs.Get("Multiplexing").unlockedItemIDs.Add(TDeMuxConfig.ID);
                    Db.Get().Techs.Get("Multiplexing").unlockedItemIDs.Add(TMuxConfig.ID);
                    Db.Get().Techs.Get("Multiplexing").unlockedItemIDs.Add(EdgeCounterConfig.ID);
                    Db.Get().Techs.Get("Multiplexing").unlockedItemIDs.Add(Ram8Config.ID);
                    Db.Get().Techs.Get("Multiplexing").unlockedItemIDs.Add(Rom4Config.ID);

                    Db.Get().Techs.Get("Multiplexing").unlockedItemIDs.Add(Stack8Config.ID);

                    //Db.Get().Techs.Get("Multiplexing").unlockedItemIDs.Add(Base2x2EveryTickConfig.ID);
                    //Db.Get().Techs.Get("Multiplexing").unlockedItemIDs.Add(Base2x2OnChangeConfig.ID);
                    //Base2x2OnChangeConfig
                }
            }
        }
    }

}
