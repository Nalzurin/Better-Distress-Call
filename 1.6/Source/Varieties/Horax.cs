using RimWorld.BaseGen;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld.QuestGen;
using Verse.Noise;
using Verse.AI.Group;

namespace BetterDistressCall.Varieties
{
    public class SitePartWorker_BetterDistressCall_Horax : SitePartWorker_DistressCall
    {


        private const float CorpsePointFactor = 0.33f;

        private const int SpawnRadius = 20;
        private bool makeMutations = false;
        private int mutationCount = 0;
        public override void PostMapGenerate(Map map)
        {
            Site site = map.Parent as Site;
            int ticks = Find.TickManager.TicksGame - map.Parent.creationGameTicks;


            Faction faction = site.Faction ?? Find.FactionManager.RandomEnemyFaction();
            List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
            {
                faction = faction,
                groupKind = PawnGroupKindDefOf.Settlement,
                points = site.ActualThreatPoints * BetterDistressCall_Settings.HoraxSettlementPointModifier,
                tile = map.Tile,
                inhabitants = true,


            }).ToList();
            bool biotech = ModLister.CheckBiotech("Biotech");
            if (biotech)
            {
                list.ForEach((pawn) => { if (Rand.Chance(0.05f)) { pawn = GenChild(faction, map); } });

            }

            float num = Faction.OfHoraxCult.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.PsychicRitualSiege) * 1.05f;
            List<Pawn> Cultists = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
            {
                groupKind = PawnGroupKindDefOf.PsychicRitualSiege,
                points = Rand.Range(num, site.ActualThreatPoints * 0.33f),
                faction = Faction.OfHoraxCult,
                raidStrategy = RaidStrategyDefOf.ImmediateAttack
            }).ToList();


            List<Pawn> list2 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
            {
                groupKind = PawnGroupKindDefOf.PsychicRitualSiege,
                points = site.ActualThreatPoints * BetterDistressCall_Settings.HoraxPointModifier,
                faction = Faction.OfHoraxCult,
                raidStrategy = RaidStrategyDefOf.ImmediateAttack
            }).ToList();

            foreach (Pawn p in list2)
            {
                Pawn victim = list2.RandomElement();
                ObeliskSpawnUtility.TryGiveMutationMeatless(victim, new List<HediffDef> { HediffDefOf.FleshWhip, HediffDefOf.FleshmassLung, HediffDefOf.FleshmassStomach, HediffDefOf.Tentacle }.RandomElement());
            }
            for (int i = 0; i < list2.Count / 5; i++)
            {
                MutantUtility.SetFreshPawnAsMutant(list2[i], MutantDefOf.Ghoul);
            }
            Lord lord = LordMaker.MakeNewLord(Faction.OfHoraxCult, new LordJob_AssaultColony(), map, list2.Concat(Cultists));

            string Obelisk = ObeliskSpawnUtility.TrySpawnRandomObelisk(map, list, site.ActualThreatPoints);
            int stage;
            if (ticks < 30000)
            {
                DistressCallUtility.SpawnPawns(map, list, map.Center, 10);
                DistressCallUtility.SpawnPawns(map, Cultists.Concat(list2), map.Center, 20);
                if (lord.LordJob is LordJob_ChimeraAssault lordJob_ChimeraAssault)
                {
                    lordJob_ChimeraAssault.SwitchMode();
                }
                Lord lord2 = LordMaker.MakeNewLord(faction, new LordJob_AssaultThings(Faction.OfEntities, new List<Thing>(list2.Concat(Cultists).ToList()), 0.4f, false), map, list);
                stage = 1;
            }
            else if (ticks < 60000)
            {
                List<Pawn> woundedPawns = new List<Pawn>();
                for (int i = 0; i < list.Count / 2; i++)
                {
                    woundedPawns.Add(list.Last());
                    list.RemoveLast();
                }
                WoundPawns(woundedPawns, Cultists.Concat(list2).ToList());
                DistressCallUtility.SpawnPawns(map, woundedPawns, map.Center, 20);
                DistressCallUtility.SpawnPawns(map, list, map.Center, 20);
                DistressCallUtility.SpawnPawns(map, list2, map.Center, 20);
                DistressCallUtility.SpawnCorpses(map, Cultists, list, map.Center, 20);
                if (lord.LordJob is LordJob_ChimeraAssault lordJob_ChimeraAssault)
                {
                    lordJob_ChimeraAssault.SwitchMode();
                }
                Lord lord2 = LordMaker.MakeNewLord(faction, new LordJob_AssaultThings(Faction.OfEntities, new List<Thing>(list2.Concat(Cultists).ToList()), 1, false), map, list);
                stage = 2;


            }
            else if (ticks < 120000)
            {
                List<Pawn> woundedPawns = new List<Pawn>();
                List<Pawn> deadPawns = new List<Pawn>();
                for (int i = 0; i < list.Count / 4; i++)
                {
                    deadPawns.Add(list.Last());
                    list.RemoveLast();
                }
                for (int i = 0; i < list.Count * 0.75; i++)
                {
                    woundedPawns.Add(list.Last());
                    list.RemoveLast();
                }
                WoundPawns(woundedPawns, Cultists.Concat(list2).ToList());
                DistressCallUtility.SpawnPawns(map, list, map.Center, 20);
                DistressCallUtility.SpawnPawns(map, woundedPawns, map.Center, 20);
                DistressCallUtility.SpawnCorpses(map, deadPawns, Cultists.Concat(list2), map.Center, 20);
                DistressCallUtility.SpawnPawns(map, list2, map.Center, 20);
                DistressCallUtility.SpawnCorpses(map, Cultists, list, map.Center, 20);
                if (lord.LordJob is LordJob_ChimeraAssault lordJob_ChimeraAssault)
                {
                    lordJob_ChimeraAssault.SwitchMode();
                }
                stage = 3;

            }
            else if (ticks < 180000)
            {
                List<Pawn> woundedPawns = new List<Pawn>();
                for (int i = 0; i < list.Count / 4; i++)
                {
                    woundedPawns.Add(list.Last());
                    list.RemoveLast();
                }
                WoundPawns(woundedPawns, Cultists.Concat(list2).ToList());
                DistressCallUtility.SpawnPawns(map, woundedPawns, map.Center, 20);
                DistressCallUtility.SpawnCorpses(map, list, Cultists.Concat(list2), map.Center, 20);
                DistressCallUtility.SpawnPawns(map, list2, map.Center, 20);
                DistressCallUtility.SpawnCorpses(map, Cultists, list, map.Center, 20);
                stage = 4;


            }
            else
            {
                DistressCallUtility.SpawnCorpses(map, list, Cultists.Concat(list2), map.Center, 20);
                DistressCallUtility.SpawnCorpses(map, Cultists, list, map.Center, 20);
                DistressCallUtility.SpawnPawns(map, list2, map.Center, 20);
                stage = 5;

            }

            //DistressCallUtility.SpawnPawns(map, list2, map.Center, 20);
            //map.fogGrid.SetAllFogged();
            foreach (Thing allThing in map.listerThings.AllThings)
            {
                if (allThing.def.category == ThingCategory.Item)
                {
                    CompForbiddable compForbiddable = allThing.TryGetComp<CompForbiddable>();
                    if (compForbiddable != null && !compForbiddable.Forbidden)
                    {
                        allThing.SetForbidden(value: true, warnOnFail: false);

                    }
                }
                if (allThing.Faction != null && allThing is not Pawn)
                {
                    allThing.SetFaction(null);
                }

            }
            EnterSendLetter.SendLetter(stage.ToString(), "Horax", faction, Obelisk);

        }

        private Pawn GenChild(Faction faction, Map map)
        {
            PawnGenerationRequest request = new PawnGenerationRequest(tile: map.Tile, mustBeCapableOfViolence: false, colonistRelationChanceFactor: 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: true, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, biocodeWeaponChance: 0.1f, kind: PawnKindDefOf.Villager, faction: faction, context: PawnGenerationContext.NonPlayer, forceGenerateNewPawn: true, allowDead: false, allowDowned: true, canGeneratePawnRelations: true, biocodeApparelChance: 1f, validatorPreGear: null, validatorPostGear: null, minChanceToRedressWorldPawn: null, fixedBiologicalAge: null, fixedChronologicalAge: null, fixedLastName: null, fixedBirthName: null, fixedTitle: null, fixedIdeo: null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, forcedXenogenes: null, forcedEndogenes: null, forcedXenotype: null, forcedCustomXenotype: null, allowedXenotypes: null, forceBaselinerChance: 0f, developmentalStages: DevelopmentalStage.Child);

            return PawnGenerator.GeneratePawn(request);
        }
        private void WoundPawns(List<Pawn> pawns, List<Pawn> attackers)
        {
            foreach (Pawn pawn in pawns)
            {
                HealthUtility.DamageUntilDowned(pawn, Rand.Bool, null, attackers.RandomElement().def, null);
            }
        }


    }
}
