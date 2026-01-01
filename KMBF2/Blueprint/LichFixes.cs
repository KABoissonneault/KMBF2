// Copyright (c) 2026 Kévin Alexandre Boissonneault

// Use, modification, and distribution is subject to the Boost Software
// License, Version 1.0. (See accompanying file LICENSE or copy at
// http://www.boost.org/LICENSE_1_0.txt)

using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;

namespace KMBF2.Blueprint
{
    static class LichFixes
    {
        public static void Apply()
        {
            Main.Log.Log("Starting Mythic Lich fixes");

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
        }
    }
}
