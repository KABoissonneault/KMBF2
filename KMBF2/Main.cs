// Copyright (c) 2026 Kévin Alexandre Boissonneault

// Use, modification, and distribution is subject to the Boost Software
// License, Version 1.0. (See accompanying file LICENSE or copy at
// http://www.boost.org/LICENSE_1_0.txt)

using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items.Slots;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Parts;
using KMBF2.Blueprint;
using ModMenu.Settings;
using System.Reflection;
using UnityModManagerNet;

namespace KMBF2;

public static class Main {
    internal static Harmony HarmonyInstance;
    internal static UnityModManager.ModEntry.ModLogger Log;
    static Assembly EntryAssembly;

    public static bool Load(UnityModManager.ModEntry modEntry) 
    {
        Log = modEntry.Logger;
        HarmonyInstance = new Harmony(modEntry.Info.Id);
        EntryAssembly = modEntry.Assembly;

        // Apply code patches
        try
        {
            HarmonyInstance.CreateClassProcessor(typeof(BlueprintsCaches_Patch)).Patch();
        }
        catch
        {
            HarmonyInstance.UnpatchAll(HarmonyInstance.Id);
            throw;
        }

        return true;
    }    

    public static void OnBoolSettingChanged(string settingKey, bool value)
    {
        SettingHarmonyPatch.OnSettingUpdated(settingKey, value);
    }

    public static Toggle MakeToggle(string settingKey, string localizationKey, bool defaultValue)
    {
        return Toggle.New(settingKey, defaultValue, LocalizationTool.GetString(localizationKey)).OnValueChanged(v => OnBoolSettingChanged(settingKey, v));
    }

    [HarmonyPatch(typeof(BlueprintsCache))]
    public static class BlueprintsCaches_Patch
    {
        private static bool Initialized = false;

        [HarmonyPriority(Priority.First)]
        [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
        public static void Init_Postfix()
        {
            try
            {
                if (Initialized)
                {
                    Log.Log("Already initialized blueprints cache.");
                    return;
                }
                Initialized = true;

                // Settings!
                ModMenu.ModMenu.AddSettings(
                    SettingsBuilder.New("kmbf2-settings", LocalizationTool.GetString("KMBF2.SettingsName"))
                        .AddToggle(MakeToggle("kmbf2-nonstack-warning", "KMBF2.SettingNonStackWarning", defaultValue: false))
                    );

                SettingHarmonyPatch.RunPatches();

                LichFixes.Apply();
                MiscFixes.Apply();
            }
            catch (Exception e)
            {
                Log.Log(string.Concat("Failed to initialize.", e));
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SettingHarmonyPatch : Attribute
    {
        public string PatchName;
        public string SettingKey;

        public SettingHarmonyPatch(string patchName, string settingKey)
        {
            PatchName = patchName;
            SettingKey = settingKey;
        }

        private static Dictionary<string, List<Type>> settingPatches = new Dictionary<string, List<Type>>();

        public static void RunPatches()
        {
            var types = EntryAssembly.GetTypes()
                .Where((p) => p.GetCustomAttribute<SettingHarmonyPatch>() != null);

            foreach (var type in EntryAssembly.GetTypes())
            {
                SettingHarmonyPatch attribute = type.GetCustomAttribute<SettingHarmonyPatch>();
                if (attribute == null) continue;

                bool setting = ModMenu.ModMenu.GetSettingValue<bool>(attribute.SettingKey);
                Log.Log($"Patching {attribute.PatchName}: Enabled={setting}");
                if (setting)
                {
                    HarmonyInstance.CreateClassProcessor(type).Patch();
                }

                if(!settingPatches.TryGetValue(attribute.SettingKey, out var patches))
                {
                    patches = new List<Type>();
                    settingPatches.Add(attribute.SettingKey, patches);
                }
                patches.Add(type);
            }
        }

        public static void OnSettingUpdated(string settingKey, bool value)
        {
            if (!settingPatches.TryGetValue(settingKey, out var patches))
            {
                return;
            }

            if (value)
            {
                foreach (var patch in patches)
                {
                    var attribute = patch.GetCustomAttribute<SettingHarmonyPatch>();
                    Log.Log($"Patching {attribute.PatchName}: Enabled=true");
                    HarmonyInstance.CreateClassProcessor(patch).Patch();
                }
            }
            else
            {
                foreach (var patch in patches)
                {
                    var attribute = patch.GetCustomAttribute<SettingHarmonyPatch>();
                    Log.Log($"Patching {attribute.PatchName}: Enabled=false");
                    HarmonyInstance.CreateClassProcessor(patch).Unpatch();
                }
            }
        }
    }

    [SettingHarmonyPatch("Non-Stacking Bonuses Warning", "kmbf2-nonstack-warning")]
    [HarmonyPatch(typeof(UnitPartNonStackBonuses))]
    class UnitPartNonStackBonuses_Patches
    {
        [HarmonyPatch(nameof(UnitPartNonStackBonuses.ShouldShowWarning), [typeof(ItemSlot)]), HarmonyPrefix]
        static bool ShouldShowWarning(ItemSlot slot, ref bool __result)
        {
            __result = false;
            return false;
        }

        [HarmonyPatch(nameof(UnitPartNonStackBonuses.ShouldShowWarning), [typeof(Buff)]), HarmonyPrefix]
        static bool ShouldShowWarning(Buff buff, ref bool __result)
        {
            __result = false;
            return false;
        }
    }
}
