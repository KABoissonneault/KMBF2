// Copyright (c) 2026 Kévin Alexandre Boissonneault

// Use, modification, and distribution is subject to the Boost Software
// License, Version 1.0. (See accompanying file LICENSE or copy at
// http://www.boost.org/LICENSE_1_0.txt)

using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Armies.TacticalCombat.Components;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.FactLogic;

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

            // Lets it heal undead in your army. The Greater version does not have this issue
            if(PatchUtils.StartPatch("General Channel Negative Undead"))
            {
                AbilityConfigurator.For(AbilityRefs.RitualChannelNegativeEnergyAbility)
                    .AddComponent<TacticalCombatResurrection>()
                    .Configure();
            }

            if(PatchUtils.StartPatch("Undead Mind-Affecting Immunity"))
            {
                FeatureConfigurator.For(FeatureRefs.ArmyNonLiving)
                    .EditComponents<BuffDescriptorImmunity>(c =>
                    {
                        c.Descriptor |= SpellDescriptor.MindAffecting;
                    }, c => c.name == "$BuffDescriptorImmunity$171e8301-c5a3-4b5d-bb0e-6f72bb3b8942")
                    .Configure();
            }
        }
    }
}
