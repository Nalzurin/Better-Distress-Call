using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace BetterDistressCall
{

    public class GenStep_DistressCall_Loot : GenStep
    {

        public SimpleCurve numLootFromPoints;

        public bool forceAtLeastOneShard;

        public override int SeedPart => 1234731256;

        public override void Generate(Map map, GenStepParams parms)
        {
            float x = parms.sitePart?.site?.desiredThreatPoints ?? StorytellerUtility.DefaultThreatPointsNow(map);
            int num = Mathf.RoundToInt(numLootFromPoints?.Evaluate(x) ?? 0f);
            for (int i = 0; i < num; i++)
            {
                IntVec3 result;
                if (!CellFinder.TryFindRandomCell(map, (IntVec3 c) => Validator(c, map, mustBeInRoom: false), out result))
                {
                    continue;
                }
                List<Thing> list;
                if (forceAtLeastOneShard && i == 0)
                {
                    list = Gen.YieldSingle(ThingMaker.MakeThing(ThingDefOf.Shard)).ToList();
                }
                else
                {
                    ThingSetMakerParams parms2 = default(ThingSetMakerParams);
                    parms2.qualityGenerator = QualityGenerator.Reward;
                    parms2.makingFaction = Faction.OfEntities;
                    list = ThingSetMakerDefOf.MapGen_FleshSackLoot.root.Generate(parms2);
                }
                for (int num2 = list.Count - 1; num2 >= 0; num2--)
                {
                    Thing thing = list[num2];
                    GenSpawn.Spawn(thing, result, map, WipeMode.VanishOrMoveAside);
                }
            }
        }

        private bool Validator(IntVec3 c, Map map, bool mustBeInRoom)
        {
            if (!c.Standable(map))
            {
                return false;
            }
            if (c.DistanceToEdge(map) <= 2)
            {
                return false;
            }
            if ((mustBeInRoom && c.GetRoom(map) == null) || !c.GetRoom(map).ProperRoom)
            {
                return false;
            }
            if (!map.generatorDef.isUnderground && !map.reachability.CanReachMapEdge(c, TraverseMode.PassDoors))
            {
                return false;
            }
            return true;
        }
    }
}