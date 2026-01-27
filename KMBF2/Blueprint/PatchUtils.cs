// Copyright (c) 2026 Kévin Alexandre Boissonneault

// Use, modification, and distribution is subject to the Boost Software
// License, Version 1.0. (See accompanying file LICENSE or copy at
// http://www.boost.org/LICENSE_1_0.txt)

namespace KMBF2.Blueprint
{
    static class PatchUtils
    {
        public static bool StartPatch(string patchName)
        {
            Main.Log.Log($"Patching '{patchName}'");
            return true;
        }

        public static bool StartSettingPatch(string patchName, string settingName)
        {
            if(ModMenu.ModMenu.GetSettingValue<bool>(settingName))
            {
                Main.Log.Log($"Patching '{patchName}': setting '{settingName}' enabled");
                return true;
            }
            else
            {
                Main.Log.Log($"Skipping patch '{patchName}': setting '{settingName}' disabled");
                return false;
            }
        }
    }
}
