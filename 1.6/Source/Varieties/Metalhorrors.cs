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
    public class SitePartWorker_BetterDistressCall_Metalhorrors : SitePartWorker_DistressCall
    {
        public override void PostMapGenerate(Map map)
        {
            Site site = map.Parent as Site;
            int ticks = Find.TickManager.TicksGame - map.Parent.creationGameTicks;


            Faction faction = site.Faction ?? Find.FactionManager.RandomEnemyFaction();
            List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
            {
                faction = faction,
                groupKind = PawnGroupKindDefOf.Settlement,
                points = site.ActualThreatPoints * BetterDistressCall_Settings.MetalhorrorsSettlementPointModifier,
                tile = map.Tile,
                inhabitants = true,


            }).ToList();

            BetterDistressCallHelper.ChildChance(list, faction, map);


            float num = Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Metalhorrors) * 1.05f;
            List<Pawn> Metalhorrors = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
            {
                groupKind = PawnGroupKindDefOf.Metalhorrors,
                points = Rand.Range(num, site.ActualThreatPoints * 0.33f),
                faction = Faction.OfEntities,
                raidStrategy = RaidStrategyDefOf.ImmediateAttack
            }).ToList();


            List<Pawn> list2 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
            {
                groupKind = PawnGroupKindDefOf.Metalhorrors,
                points = site.ActualThreatPoints * BetterDistressCall_Settings.MetalhorrorsPointModifier,
                faction = Faction.OfEntities,
                raidStrategy = RaidStrategyDefOf.ImmediateAttack
            }).ToList();
            Lord lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_ChimeraAssault(), map, list2.Concat(Metalhorrors));
            
            string Obelisk = ObeliskSpawnUtility.TrySpawnRandomObelisk(map, list, site.ActualThreatPoints);
            int stage;
            List<Pawn> BurstPawns = new List<Pawn>();
            for (int i = 0; i < Metalhorrors.Count() + list2.Count(); i++)
            {
                BurstPawns.Add(BetterDistressCallHelper.GenPawn(faction, map));
            }
            if (ticks < 30000)
            {
                DistressCallUtility.SpawnPawns(map, list, map.Center, 10);
                DistressCallUtility.SpawnPawns(map, Metalhorrors.Concat(list2), map.Center, 15);
                BetterDistressCallHelper.WoundPawns(BurstPawns, list2.Concat(Metalhorrors).ToList());
                DistressCallUtility.SpawnPawns(map, BurstPawns, map.Center, 10);
                if (lord.LordJob is LordJob_ChimeraAssault lordJob_ChimeraAssault)
                {
                    lordJob_ChimeraAssault.SwitchMode();
                }
                Lord lord2 = LordMaker.MakeNewLord(faction, new LordJob_AssaultThings(Faction.OfEntities, new List<Thing>(list2.Concat(Metalhorrors).ToList()), 0.4f, false), map, list);
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
                BetterDistressCallHelper.WoundPawns(woundedPawns, Metalhorrors.Concat(list2).ToList());
                DistressCallUtility.SpawnPawns(map, woundedPawns, map.Center, 20);
                DistressCallUtility.SpawnPawns(map, list, map.Center, 20);
                DistressCallUtility.SpawnPawns(map, list2, map.Center, 15);
                DistressCallUtility.SpawnCorpses(map, Metalhorrors, list, map.Center, 15);
                DistressCallUtility.SpawnCorpses(map, BurstPawns, list2, map.Center, 10);
                if (lord.LordJob is LordJob_ChimeraAssault lordJob_ChimeraAssault)
                {
                    lordJob_ChimeraAssault.SwitchMode();
                }
                Lord lord2 = LordMaker.MakeNewLord(faction, new LordJob_AssaultThings(Faction.OfEntities, new List<Thing>(list2.Concat(Metalhorrors).ToList()), 1, false), map, list);
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
                BetterDistressCallHelper.WoundPawns(woundedPawns, Metalhorrors.Concat(list2).ToList());
                DistressCallUtility.SpawnPawns(map, list, map.Center, 20);
                DistressCallUtility.SpawnPawns(map, woundedPawns, map.Center, 20);
                DistressCallUtility.SpawnCorpses(map, deadPawns, Metalhorrors.Concat(list2), map.Center, 20);
                DistressCallUtility.SpawnPawns(map, list2, map.Center, 15);
                DistressCallUtility.SpawnCorpses(map, Metalhorrors, list, map.Center, 15);
                DistressCallUtility.SpawnCorpses(map, BurstPawns, list2, map.Center, 10);

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
                BetterDistressCallHelper.WoundPawns(woundedPawns, Metalhorrors.Concat(list2).ToList());
                DistressCallUtility.SpawnPawns(map, woundedPawns, map.Center, 20);
                DistressCallUtility.SpawnCorpses(map, list, Metalhorrors.Concat(list2), map.Center, 20);
                DistressCallUtility.SpawnPawns(map, list2, map.Center, 15);
                DistressCallUtility.SpawnCorpses(map, Metalhorrors, list, map.Center, 15);
                DistressCallUtility.SpawnCorpses(map, BurstPawns, list2, map.Center, 10);

                stage = 4;
            }
            else
            {
                DistressCallUtility.SpawnCorpses(map, list, Metalhorrors.Concat(list2), map.Center, 20);
                DistressCallUtility.SpawnCorpses(map, Metalhorrors, list, map.Center, 20);
                DistressCallUtility.SpawnPawns(map, list2, map.Center, 10);
                DistressCallUtility.SpawnCorpses(map, BurstPawns, list2, map.Center, 10);

                stage = 5;
            }
            //DistressCallUtility.SpawnPawns(map, list2, map.Center, 20);
            //map.fogGrid.SetAllFogged();
            BetterDistressCallHelper.ForbidAndSetFactionless(map.listerThings.AllThings);

            EnterSendLetter.SendLetter(stage.ToString(), "Metalhorrors", faction, Obelisk);

        }
    }
}
