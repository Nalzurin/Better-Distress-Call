﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static System.Collections.Specialized.BitVector32;
using Verse.Noise;

namespace BetterDistressCall
{
    public static class BetterDistressCallHelper
    {
        public static void ForbidAndSetFactionless(List<Thing> things)
        {
            foreach (Thing allThing in things)
            {
                if (allThing.def.category == ThingCategory.Item)
                {
                    CompForbiddable compForbiddable = allThing.TryGetComp<CompForbiddable>();
                    if (compForbiddable != null && !compForbiddable.Forbidden)
                    {
                        allThing.SetForbidden(value: true, warnOnFail: false);

                    }
                }
                if (allThing.Faction != null && allThing is not Pawn && allThing is not Building_Turret)
                {
                    allThing.SetFaction(null);
                }

            }
        }
        public static void ChildChance(List<Pawn> list, Faction faction, Map map)
        {
            if (!ModLister.CheckBiotech("Biotech"))
            {
                return;
            }
            for(int i = 0; i <  list.Count; i++)
            {

                if (Rand.Chance(BetterDistressCall_Settings.ChanceOfSurvivorBeingAChild))
                {
                    list[i] = GenChild(faction, map);
                }
            }

        }
        public static Pawn GenChild(Faction faction, Map map)
        {
            PawnGenerationRequest request = new PawnGenerationRequest(tile: map.Tile, mustBeCapableOfViolence: false, colonistRelationChanceFactor: 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: true, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, biocodeWeaponChance: 0.1f, kind: PawnKindDefOf.Villager, faction: faction, context: PawnGenerationContext.NonPlayer, forceGenerateNewPawn: true, allowDead: false, allowDowned: true, canGeneratePawnRelations: true, biocodeApparelChance: 1f, validatorPreGear: null, validatorPostGear: null, minChanceToRedressWorldPawn: null, fixedBiologicalAge: null, fixedChronologicalAge: null, fixedLastName: null, fixedBirthName: null, fixedTitle: null, fixedIdeo: null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, forcedXenogenes: null, forcedEndogenes: null, forcedXenotype: null, forcedCustomXenotype: null, allowedXenotypes: null, forceBaselinerChance: 0f, developmentalStages: DevelopmentalStage.Child);

            return PawnGenerator.GeneratePawn(request);
        }
        public static void WoundPawns(List<Pawn> pawns, List<Pawn> attackers)
        {
            foreach (Pawn pawn in pawns)
            {
                HealthUtility.DamageUntilDowned(pawn, Rand.Bool, null, attackers.RandomElement().def, null);
            }
        }
        public static Pawn GenPawn(Faction faction, Map map)
        {
            PawnGenerationRequest request = new PawnGenerationRequest(tile: map.Tile, mustBeCapableOfViolence: false, colonistRelationChanceFactor: 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: true, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, biocodeWeaponChance: 0.1f, kind: PawnKindDefOf.Villager, faction: faction, context: PawnGenerationContext.NonPlayer, forceGenerateNewPawn: true, allowDead: false, allowDowned: true, canGeneratePawnRelations: true, biocodeApparelChance: 1f, validatorPreGear: null, validatorPostGear: null, minChanceToRedressWorldPawn: null, fixedBiologicalAge: null, fixedChronologicalAge: null, fixedLastName: null, fixedBirthName: null, fixedTitle: null, fixedIdeo: null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, forcedXenogenes: null, forcedEndogenes: null, forcedXenotype: null, forcedCustomXenotype: null, allowedXenotypes: null, forceBaselinerChance: 0f, developmentalStages: DevelopmentalStage.Adult);

            return PawnGenerator.GeneratePawn(request);
        }
    }
}
