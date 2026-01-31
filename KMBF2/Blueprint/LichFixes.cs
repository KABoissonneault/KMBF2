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
using Kingmaker.Blueprints.Classes.Spells;
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

            if(PatchUtils.StartSettingPatch("Deadly Magic Spell Trigger", "kmbf2-deadly-magic"))
            {
                // Description says it should only affect spells.
                // While this is generally a nerf, this also prevents lots of invisible effects like the Belt of Demonc Shadow from silencing the party
                BuffConfigurator.For(BuffRefs.DeadlyMagicBuff)
                    .EditComponent<AddAbilityUseTrigger>(c =>
                    {
                        c.CheckAbilityType = true;
                        c.Type = AbilityType.Spell;
                    })
                    .Configure();
            }

            // Bring parity to the actual undead immunities
            // I could respect TTT's removal of the Nauseated/Sickened immunities, but given that TTT does not implement the rule
            // where undead are immune to the effects of Fortitude saving throws, I think it's more accurate to keep them
            if(PatchUtils.StartPatch("Blessing of Unlife Immunities"))
            {
                BuffConfigurator.For(BuffRefs.BlessingOfUnlifeBuff)
                    .AddComponent<AddConditionImmunity>(c =>
                    {
                        c.Condition = Kingmaker.UnitLogic.UnitCondition.Paralyzed;
                    })
                    .AddComponent<AddConditionImmunity>(c =>
                    {
                        c.Condition = Kingmaker.UnitLogic.UnitCondition.Exhausted;
                    })
                    .EditComponents<BuffDescriptorImmunity>(c =>
                    {
                        c.Descriptor = SpellDescriptor.Sickened | SpellDescriptor.Fatigue | SpellDescriptor.Nauseated | SpellDescriptor.Exhausted | SpellDescriptor.Paralysis 
                        | SpellDescriptor.Death | SpellDescriptor.Bleed | SpellDescriptor.VilderavnBleed | SpellDescriptor.Petrified | SpellDescriptor.NegativeLevel;
                    }, c => c.name == "$BuffDescriptorImmunity$eb929088-4f9e-4c60-92ee-89a0fa13d8f1")
                    .EditComponents<BuffDescriptorImmunity>(c =>
                    {
                        c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Fear | SpellDescriptor.Compulsion | SpellDescriptor.Emotion | SpellDescriptor.Charm
                        | SpellDescriptor.Daze | SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.Stun | SpellDescriptor.Confusion | SpellDescriptor.Sleep;
                        c.m_IgnoreFeature = FeatureRefs.UndeadMindAffection.Cast<BlueprintUnitFactReference>().Reference;
                    }, c => c.name == "$BuffDescriptorImmunity$d4fb14f4-7d7b-45b3-ab7f-d7eb6f9f7a63")
                    .EditComponents<SpellImmunityToSpellDescriptor>(c =>
                    {
                        c.Descriptor = SpellDescriptor.Sickened | SpellDescriptor.Fatigue | SpellDescriptor.Nauseated | SpellDescriptor.Exhausted | SpellDescriptor.Paralysis
                        | SpellDescriptor.Death | SpellDescriptor.Bleed | SpellDescriptor.VilderavnBleed | SpellDescriptor.Petrified | SpellDescriptor.NegativeLevel;
                    }, c => c.name == "$SpellImmunityToSpellDescriptor$c0976aae-8934-4994-9b1a-f5614f7d4f26")
                    .EditComponents<SpellImmunityToSpellDescriptor>(c =>
                    {
                        c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Fear | SpellDescriptor.Compulsion | SpellDescriptor.Emotion | SpellDescriptor.Charm
                        | SpellDescriptor.Daze | SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.Stun | SpellDescriptor.Confusion | SpellDescriptor.Sleep;
                        c.m_CasterIgnoreImmunityFact = FeatureRefs.UndeadMindAffection.Cast<BlueprintUnitFactReference>().Reference;
                    }, c => c.name == "$SpellImmunityToSpellDescriptor$fb56d182-0078-4f5e-a1dd-5730215f7e72")
                    .Configure();
            }
        }
    }
}
