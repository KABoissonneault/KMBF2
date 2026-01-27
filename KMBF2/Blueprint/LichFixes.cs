// Copyright (c) 2026 Kévin Alexandre Boissonneault

// Use, modification, and distribution is subject to the Boost Software
// License, Version 1.0. (See accompanying file LICENSE or copy at
// http://www.boost.org/LICENSE_1_0.txt)

using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Armies.Components;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace KMBF2.Blueprint
{
    static class LichFixes
    {
        public static void Apply()
        {
            Main.Log.Log("Starting Mythic Lich patches");

            if(PatchUtils.StartPatch("Skeletal Champion Cold Immunity"))
            {
                void AddColdImmunity(BlueprintCore.Utils.Blueprint<BlueprintReference<BlueprintUnit>> unit)
                {
                    UnitConfigurator.For(unit)
                        .AddToAddFacts(FeatureRefs.ColdImmunity.Cast<BlueprintUnitFactReference>())
                        .Configure();
                }

                AddColdImmunity(UnitRefs.MythicLichSkeletonArcherUnit);
                AddColdImmunity(UnitRefs.MythicLichSkeletonDualWielderUnit);
                AddColdImmunity(UnitRefs.MythicLichSkeletonTankUnit);
                AddColdImmunity(UnitRefs.MythicLichSkeletonTwoHandedUnit);
            }

            if(PatchUtils.StartPatch("Lord Beyond the Grave Command Undead Duration"))
            {
                AbilityConfigurator.For(AbilityRefs.LichCommandUndead)
                    .EditComponent<ContextSetAbilityParams>(c =>
                    {
                        // -1 falls back to spellbook
                        c.CasterLevel = -1;
                    })
                    .Configure();
            }

            // Non-living units should not have morale
            if(PatchUtils.StartPatch("Non-Living Crusade Morale"))
            {
                void RemoveMorale(BlueprintCore.Utils.Blueprint<BlueprintReference<BlueprintUnit>> unit)
                {
                    UnitConfigurator.For(unit)
                        .EditComponent<ArmyUnitComponent>(c =>
                        {
                            c.IsHaveMorale = false;
                        })
                        .Configure();
                }

                RemoveMorale(UnitRefs.ArmyZombieStandard);
                RemoveMorale(UnitRefs.ArmyCyborgs);
                RemoveMorale(UnitRefs.ArmyPlagueDragon);
                RemoveMorale(UnitRefs.ArmyVampireNinjaPirates);
            }

            if(PatchUtils.StartPatch("Lord of Death Domains"))
            {
                // Description says the War domain should be supported
                FeatureConfigurator.For(FeatureRefs.LichDeityFeature)
                    .EditComponents<AddFacts>(c =>
                    {
                        c.m_Facts = [.. c.m_Facts, FeatureRefs.WarDomainAllowed.Cast<BlueprintUnitFactReference>().Reference];
                    }, c => c.name == "$AddFacts$20431575-0c00-4722-9799-52a01cb8231e")
                    .Configure();
            }

        }
    }
}
