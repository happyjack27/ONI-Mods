using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectors
{
    public static class StringUtils
    {
        public static void AddBuildingStrings(string buildingId, string name, string description, string effect)
        {
            Strings.Add($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.NAME", UI.FormatAsLink(name, buildingId));
            Strings.Add($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.DESC", description);
            Strings.Add($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.EFFECT", effect);
        }

        public static void AddPlantStrings(string plantId, string name, string description, string domesticatedDescription)
        {
            Strings.Add($"STRINGS.CREATURES.SPECIES.{plantId.ToUpperInvariant()}.NAME", UI.FormatAsLink(name, plantId));
            Strings.Add($"STRINGS.CREATURES.SPECIES.{plantId.ToUpperInvariant()}.DESC", description);
            Strings.Add($"STRINGS.CREATURES.SPECIES.{plantId.ToUpperInvariant()}.DOMESTICATEDDESC", domesticatedDescription);
        }

        public static void AddPlantSeedStrings(string plantId, string name, string description)
        {
            Strings.Add($"STRINGS.CREATURES.SPECIES.SEEDS.{plantId.ToUpperInvariant()}.NAME", UI.FormatAsLink(name, plantId));
            Strings.Add($"STRINGS.CREATURES.SPECIES.SEEDS.{plantId.ToUpperInvariant()}.DESC", description);
        }

        public static void AddFoodStrings(string foodId, string name, string description, string recipeDescription = null)
        {
            Strings.Add($"STRINGS.ITEMS.FOOD.{foodId.ToUpperInvariant()}.NAME", UI.FormatAsLink(name, foodId));
            Strings.Add($"STRINGS.ITEMS.FOOD.{foodId.ToUpperInvariant()}.DESC", description);

            if (recipeDescription != null)
                Strings.Add($"STRINGS.ITEMS.FOOD.{foodId.ToUpperInvariant()}.RECIPEDESC", recipeDescription);
        }

        public static void AddSideScreenStrings(Type classType) {
            Strings.Add($"{classType.FullName.Replace('+', '.')}.TITLE", (LocString)classType.GetField("TITLE", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(null));
            var toolTip = classType.GetField("TOOLTIP", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            if (toolTip != null)
            {
                Strings.Add($"{classType.FullName.Replace('+', '.')}.TOOLTIP", (LocString)toolTip.GetValue(null));
            }
        }

        public static void AddStringTypes(Type classType)
        {
            var x = classType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (var item in x)
            {
                var value = item.GetValue(null) as LocString;

                if (value != null)
                {
                    Strings.Add(GetStringKey(classType, item.Name), value);
                }
            }

            foreach (var item in classType.GetNestedTypes())
            {
                AddStringTypes(item);
            }
        }

        public static string GetStringKey(Expression<Func<LocString>> exp)
        {
            var me = exp.Body as MemberExpression;
            return GetStringKey(me.Member.DeclaringType, me.Member.Name);
        }

        public static string GetStringKey(Type declaringType, string memberName)
        {
            return $"{declaringType}.{memberName}".Replace("#", ".")
                    .Replace("+", ".");
        }

        public static StringEntry Get(Expression<Func<LocString>> exp)
        {
            var key = GetStringKey(exp);
            var str = Strings.Get(key);
            return str;
        }

    }

}
