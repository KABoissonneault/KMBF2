using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace KMBF2.Blueprint
{    static class WarpriestFixes
    {
        public static void Apply()
        {
            Main.Log.Log("Starting Warpriest patches");

            // Warpriests use Feature rank instead of caster level to scale their Channeling
            // The Phylactery only increases caster level for "Channel" abilities, which would only
            // give the right numbers for Cleric progression (1 dice per two levels) anyway
            // Warpriest Positive Heal does add the Phylactery features explicitly, but not Positive Harm
            // or the two negative channels. So let's match that
            if(PatchUtils.StartPatch("Warpriest Channeling Phylactery"))
            {
                AbilityConfigurator.For(AbilityRefs.WarpriestChannelPositiveHarm)
                    .EditComponents<ContextRankConfig>(c =>
                    {
                        c.m_FeatureList = [
                            FeatureRefs.WarpriestChannelEnergyFeature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.WarpriestFervorHealDamageRank.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.PositiveChanneling1Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.PositiveChanneling2Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.PositiveChanneling2Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.PositiveChanneling4Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.PositiveChanneling4Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.PositiveChanneling4Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.PositiveChanneling4Feature.Cast<BlueprintFeatureReference>().Reference
                            ];
                    }, c => c.Type == AbilityRankType.Default)
                    .Configure();

                AbilityConfigurator.For(AbilityRefs.WarpriestChannelNegativeHeal)
                    .EditComponents<ContextRankConfig>(c =>
                    {
                        c.m_FeatureList = [
                            FeatureRefs.WarpriestChannelNegativeFeature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.WarpriestFervorHealDamageRank.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.NegativeChanneling1Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.NegativeChanneling2Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.NegativeChanneling2Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.NegativeChanneling4Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.NegativeChanneling4Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.NegativeChanneling4Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.NegativeChanneling4Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.DefilerFeature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.DefilerFeature.Cast<BlueprintFeatureReference>().Reference
                            ];
                    }, c => c.Type == AbilityRankType.Default)
                    .Configure();

                AbilityConfigurator.For(AbilityRefs.WarpriestChannelNegativeEnergy)
                    .EditComponents<ContextRankConfig>(c =>
                    {
                        c.m_FeatureList = [
                            FeatureRefs.WarpriestChannelNegativeFeature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.WarpriestFervorHealDamageRank.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.NegativeChanneling1Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.NegativeChanneling2Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.NegativeChanneling2Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.NegativeChanneling4Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.NegativeChanneling4Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.NegativeChanneling4Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.NegativeChanneling4Feature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.DefilerFeature.Cast<BlueprintFeatureReference>().Reference
                            , FeatureRefs.DefilerFeature.Cast<BlueprintFeatureReference>().Reference
                            ];
                    }, c => c.Type == AbilityRankType.Default)
                    .Configure();
            }
        }
    }
}
