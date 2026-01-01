// Copyright (c) 2026 Kévin Alexandre Boissonneault

// Use, modification, and distribution is subject to the Boost Software
// License, Version 1.0. (See accompanying file LICENSE or copy at
// http://www.boost.org/LICENSE_1_0.txt)

using BlueprintCore.Blueprints.Configurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;

namespace KMBF2.Blueprint
{
    // Fixes not big enough to have their own file
    static class MiscFixes
    {
        public static void Apply()
        {
            Main.Log.Log("Starting miscellaneous fixes");

            if (PatchUtils.StartPatch("Sword of Valor aggro"))
            {
                // Why would it
                AbilityAreaEffectConfigurator.For(AbilityAreaEffectRefs.SwordofValorArea)
                    .SetAggroEnemies(false)
                    .Configure();
            }

            if (PatchUtils.StartPatch("Point-Blank Master Requirements"))
            {
                // No such requirement in tabletop
                // Lets Crusaders and Shamans pick PBM while getting Weapon Specialization from their class
                ParametrizedFeatureConfigurator.For(ParametrizedFeatureRefs.PointBlankMaster)
                    .RemoveComponents(c => c is PrerequisiteClassLevel)
                    .Configure();
            }

            if (PatchUtils.StartPatch("Triceratops Rider Protection"))
            {
                void AddExtraEffect(BlueprintCore.Utils.Blueprint<BlueprintReference<BlueprintFeature>> feature)
                {
                    FeatureConfigurator.For(feature)
                        .AddComponent<BuffExtraEffects>(c =>
                        {
                            c.m_CheckedBuff = BuffRefs.MountedBuff.Cast<BlueprintBuffReference>().Reference;
                            c.m_ExtraEffectBuff = BuffRefs.AnimalCompanionFeatureTriceratopsBuff.Cast<BlueprintBuffReference>().Reference;
                        })
                        .Configure();
                }

                AddExtraEffect(FeatureRefs.AnimalCompanionFeatureTriceratops);
                AddExtraEffect(FeatureRefs.AnimalCompanionFeatureTriceratops_PreorderBonus);
                AddExtraEffect(FeatureRefs.TriceratopsStatuetteCorrectFeature);

                BuffConfigurator.For(BuffRefs.AnimalCompanionFeatureTriceratopsBuff)
                    .EditComponent<AddContextStatBonus>(c =>
                    {
                        c.Stat = StatType.AC;
                    })
                    .Configure();
            }
        }
    }
}
