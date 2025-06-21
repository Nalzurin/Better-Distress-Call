using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BetterDistressCall
{
    public static class BetterDistressCallHelper
    {
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
