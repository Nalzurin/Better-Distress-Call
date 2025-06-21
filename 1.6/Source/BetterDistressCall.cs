
using RimWorld.BaseGen;
using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using System.Linq;
using Verse.AI.Group;
using BetterDistressCall.Varieties;
using System.Runtime.CompilerServices;

namespace BetterDistressCall
{

    public class SitePartWorker_BetterDistressCall_Fleshbeasts : SitePartWorker_DistressCall
    {
        private static readonly IntRange BurrowCorpseCountRange = new IntRange(1, 2);
        public override void PostMapGenerate(Map map)
        {
            Site site = map.Parent as Site;
            int ticks = Find.TickManager.TicksGame - map.Parent.creationGameTicks;
            Faction faction = site.Faction ?? Find.FactionManager.RandomEnemyFaction();
            List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
            {
                faction = faction,
                groupKind = PawnGroupKindDefOf.Settlement,
                points = site.ActualThreatPoints * BetterDistressCall_Settings.FleshbeastsSettlementPointModifier,
                tile = map.Tile,
                inhabitants = true,

            }).ToList();
            float num = Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Fleshbeasts) * 1.05f;
            List<Pawn> fleshbeasts = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
            {
                groupKind = PawnGroupKindDefOf.Fleshbeasts,
                points = Rand.Range(num, site.ActualThreatPoints * 0.33f),
                faction = Faction.OfEntities,
                raidStrategy = RaidStrategyDefOf.ImmediateAttack
            }).ToList();
            List<Pawn> list2 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
            {
                groupKind = PawnGroupKindDefOf.Fleshbeasts,
                points = site.ActualThreatPoints * BetterDistressCall_Settings.FleshbeastsPointModifier,
                faction = Faction.OfEntities,
                raidStrategy = RaidStrategyDefOf.ImmediateAttack
            }).ToList();
            SplitFleshbeasts(ref fleshbeasts);


            string Obelisk = ObeliskSpawnUtility.TrySpawnRandomObelisk(map, list, site.ActualThreatPoints);
            int stage;
            if (ticks < 30000)
            {
                DistressCallUtility.SpawnPawns(map, list, map.Center, 20);
                DistressCallUtility.SpawnPawns(map, fleshbeasts.Concat(list2), map.Center, 20);
                Lord lord2 = LordMaker.MakeNewLord(faction, new LordJob_AssaultThings(Faction.OfEntities, new List<Thing>(list2.Concat(fleshbeasts).ToList()), 0.4f, false), map, list);
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
                BetterDistressCallHelper.WoundPawns(woundedPawns, fleshbeasts.Concat(list2).ToList());
                DistressCallUtility.SpawnPawns(map, woundedPawns, map.Center, 20);
                DistressCallUtility.SpawnPawns(map, list, map.Center, 20);
                DistressCallUtility.SpawnPawns(map, list2, map.Center, 20);
                DistressCallUtility.SpawnCorpses(map, fleshbeasts, list, map.Center, 20);
                ScatterCorpsesAroundPitBurrows(map, list);
                Lord lord2 = LordMaker.MakeNewLord(faction, new LordJob_AssaultThings(Faction.OfEntities, new List<Thing>(list2.Concat(fleshbeasts).ToList()), 0.4f, false), map, list);
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
                BetterDistressCallHelper.WoundPawns(woundedPawns, fleshbeasts.Concat(list2).ToList());
                DistressCallUtility.SpawnPawns(map, list, map.Center, 20);
                DistressCallUtility.SpawnPawns(map, woundedPawns, map.Center, 20);
                DistressCallUtility.SpawnCorpses(map, deadPawns, fleshbeasts.Concat(list2), map.Center, 20);
                DistressCallUtility.SpawnPawns(map, list2, map.Center, 20);
                DistressCallUtility.SpawnCorpses(map, fleshbeasts, list, map.Center, 20);
                ScatterCorpsesAroundPitBurrows(map, list);
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
                BetterDistressCallHelper.WoundPawns(woundedPawns, fleshbeasts.Concat(list2).ToList());
                DistressCallUtility.SpawnPawns(map, woundedPawns, map.Center, 20);
                DistressCallUtility.SpawnCorpses(map, list, fleshbeasts.Concat(list2), map.Center, 20);
                DistressCallUtility.SpawnPawns(map, list2, map.Center, 20);
                DistressCallUtility.SpawnCorpses(map, fleshbeasts, list, map.Center, 20);
                ScatterCorpsesAroundPitBurrows(map, list);
                stage = 4;
            }
            else
            {
                DistressCallUtility.SpawnCorpses(map, list, fleshbeasts.Concat(list2), map.Center, 20);
                DistressCallUtility.SpawnCorpses(map, fleshbeasts, list, map.Center, 20);
                DistressCallUtility.SpawnPawns(map, list2, map.Center, 20);
                ScatterCorpsesAroundPitBurrows(map, list);
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
                    if (allThing.Faction != null && allThing is not Pawn)
                    {
                        allThing.SetFaction(null);
                    }
                }
            }
            EnterSendLetter.SendLetter(stage.ToString(), "Fleshbeast", faction, Obelisk);

        }
        private void ScatterCorpsesAroundPitBurrows(Map map, IEnumerable<Pawn> killers)
        {
            foreach (Thing item in map.listerThings.ThingsOfDef(ThingDefOf.PitBurrow))
            {
                if (Rand.Chance(0.4f))
                {
                    int randomInRange = BurrowCorpseCountRange.RandomInRange;
                    List<Pawn> fleshbeasts = new List<Pawn>();
                    for (int i = 0; i < randomInRange; i++)
                    {
                        fleshbeasts.Add(PawnGenerator.GeneratePawn(FleshbeastUtility.AllFleshbeasts.RandomElement(), Faction.OfEntities));
                    }
                    SplitFleshbeasts(ref fleshbeasts);
                    DistressCallUtility.SpawnCorpses(map, fleshbeasts, killers, item.Position, 3);
                }
            }
        }

        private void SplitFleshbeasts(ref List<Pawn> fleshbeasts)
        {
            List<Pawn> list = new List<Pawn>();
            foreach (Pawn fleshbeast in fleshbeasts)
            {
                foreach (Pawn item in FleshbeastUtility.SplitFleshbeast(fleshbeast))
                {
                    list.Add(item);
                }
            }
            fleshbeasts = list;
        }
    }
}
