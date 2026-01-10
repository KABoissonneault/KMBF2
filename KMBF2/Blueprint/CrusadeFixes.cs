using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Blueprints.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMBF2.Blueprint
{
    static class CrusadeFixes
    {
        public static void Apply()
        {
            Main.Log.Log("Starting Crusade patches");

            // Army Hellknights apply their Charisma bonus to Attack, Damage, and AC against Chaotic units after the first attack
            // Their native Charisma modifiers is 0, rendering them useless in many cases
            // Adding a small +2 bonus to give them proper flavor
            if(PatchUtils.StartSettingPatch("Army Hellknight Charisma Bonus (+2)", "kmbf2-crusade-balance"))
            {
                UnitConfigurator.For(UnitRefs.ArmyHellknight)
                    .SetCharisma(14)
                    .Configure();
            }
        }
    }
}
