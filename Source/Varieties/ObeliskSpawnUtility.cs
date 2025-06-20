using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BetterDistressCall.Varieties
{
    public static class ObeliskSpawnUtility
    {

        public static bool TryFindCell(out IntVec3 cell, Map map, ThingDef obeliskDef)
        {
            cell = default(IntVec3);
            return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.WarpedObeliskIncoming, map, out cell, 10, default(IntVec3), -1, allowRoofedCells: true, allowCellsWithItems: false, allowCellsWithBuildings: false, colonyReachable: false, avoidColonistsIfExplosive: true, alwaysAvoidColonists: true, delegate (IntVec3 x)
            {
                if ((float)x.DistanceToEdge(map) < 20f + (float)map.Size.x * 0.1f)
                {
                    return false;
                }
                foreach (IntVec3 item in CellRect.CenteredOn(x, obeliskDef.Size.x, obeliskDef.Size.z))
                {
                    if (!item.InBounds(map) || !item.Standable(map) || !item.GetTerrain(map).affordances.Contains(obeliskDef.terrainAffordanceNeeded))
                    {
                        return false;
                    }
                }
                return true;
            });
        }
        public static void SpawnObelisk(ThingDef obelisk, IntVec3 loc, Map map)
        {
            Thing thing = ThingMaker.MakeThing(obelisk);
            GenSpawn.Spawn(thing, loc, map, WipeMode.VanishOrMoveAside);
        }

        public static string TrySpawnRandomObelisk(Map map, List<Pawn> pawns, float threatPoints)
        {
            if (map == null)
            {
                Log.Error("Map is null");
                return "";
            }

            if (Rand.Chance(BetterDistressCall_Settings.ObeliskChance) && BetterDistressCall_Settings.Obelisks)
            {
                IntVec3 obeliskCell = default(IntVec3);
                switch (Rand.RangeInclusive(0, 2))
                {
                    //Mutator
                    case 0:
                        if (!TryFindCell(out obeliskCell, map, ThingDefOf.WarpedObelisk_Mutator))
                        {
                            return "";
                        }
                        SpawnObelisk(ThingDefOf.WarpedObelisk_Mutator, obeliskCell, map);
                        foreach(Pawn p in pawns)
                        {
                            Pawn victim = pawns.RandomElement();
                            TryGiveMutationMeatless(victim, new List<HediffDef> { HediffDefOf.FleshWhip, HediffDefOf.FleshmassLung, HediffDefOf.FleshmassStomach, HediffDefOf.Tentacle }.RandomElement());

                        }
                        return "Mutator";

                    //Abductor
                    case 1:
                        if (!TryFindCell(out obeliskCell, map, ThingDefOf.WarpedObelisk_Mutator))
                        {
                            return "";
                        }
                        SpawnObelisk(Definitions.WarpedObelisk_AbductorQuest, obeliskCell, map);
                        Thing obeliskAbudctor = obeliskCell.GetFirstThing(map, Definitions.WarpedObelisk_AbductorQuest);
                        CompObelisk_AbductorQuest comps = obeliskAbudctor.TryGetComp<CompObelisk_AbductorQuest>();
                        comps.ActivityComp.SetActivity(0.98f);
                        return "Abductor";
                    //Duplicator
                    case 2:
                        if (!TryFindCell(out obeliskCell, map, ThingDefOf.WarpedObelisk_Mutator))
                        {
                            return "";
                        }
                        SpawnObelisk(ThingDefOf.WarpedObelisk_Duplicator, obeliskCell, map);
                        Thing obeliskDuplicator = obeliskCell.GetFirstThing(map, ThingDefOf.WarpedObelisk_Duplicator);
                        Pawn original = pawns.RandomElement();
                        Pawn duplicate = Find.PawnDuplicator.Duplicate(original);
                        Find.PawnDuplicator.AddDuplicate(original.duplicate.duplicateOf, original);
                        Find.PawnDuplicator.AddDuplicate(duplicate.duplicate.duplicateOf, duplicate);
                        pawns.Replace(pawns.Where((c) => { return c != original; }).RandomElement(), duplicate);
                        return "Duplicator";

                    default:
                        return "";
                }
            }
            return "";
        }



        //Mutator
        public static bool TryGiveMutationMeatless(Pawn pawn, HediffDef mutationDef)
        {
            if (!ModsConfig.AnomalyActive)
            {
                return false;
            }

            if (mutationDef.defaultInstallPart == null)
            {
                Log.ErrorOnce("Attempted to use mutation hediff which didn't specify a default install part (hediff: " + mutationDef.label, 194783821);
                return false;
            }

            List<BodyPartRecord> list = (from part in pawn.RaceProps.body.GetPartsWithDef(mutationDef.defaultInstallPart)
                                         where pawn.health.hediffSet.HasMissingPartFor(part)
                                         select part).ToList();
            List<BodyPartRecord> list2 = (from part in pawn.RaceProps.body.GetPartsWithDef(mutationDef.defaultInstallPart)
                                          where !pawn.health.hediffSet.HasDirectlyAddedPartFor(part)
                                          select part).ToList();
            BodyPartRecord bodyPartRecord = null;
            if (list.Any())
            {
                bodyPartRecord = list.RandomElement();
            }
            else if (list2.Any())
            {
                bodyPartRecord = list2.RandomElement();
            }

            if (bodyPartRecord == null)
            {
                return false;
            }

            MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, bodyPartRecord, pawn.PositionHeld, pawn.MapHeld);
            pawn.health.RestorePart(bodyPartRecord);
            pawn.health.AddHediff(mutationDef, bodyPartRecord);
            Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BloodLoss, pawn);
            hediff.Severity = 0.2f;
            pawn.health.AddHediff(hediff);
            return true;
        }



    }
}
