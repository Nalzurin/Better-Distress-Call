using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BetterDistressCall.Varieties
{
    public class GenStep_DestroyDoors : GenStep
    {
        public override int SeedPart => 874575948;

        public override void Generate(Map map, GenStepParams parms)
        {
            List<Building> list = map.listerBuildings.AllBuildingsNonColonistOfDef(ThingDefOf.Door).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].def == ThingDefOf.Door)
                {
                    list[i].TakeDamage(new DamageInfo(DamageDefOf.Scratch, list[i].HitPoints));
                }
            }
        }
    }
}
