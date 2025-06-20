using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BetterDistressCall.Varieties
{
    

    public class GenStep_DamageWalls : GenStep
    {
        public override int SeedPart => 874575948;
        public override void Generate(Map map, GenStepParams parms)
        {
            List<Thing> list = map.listerThings.ThingsOfDef(ThingDefOf.Wall);

            list.ForEach((thing) =>
            {
                
                if (thing.def == ThingDefOf.Wall && Rand.Chance(0.3f))
                {

                    thing.TakeDamage(new DamageInfo(DamageDefOf.Scratch,Rand.RangeInclusive(1,thing.HitPoints-1)));
                }
            });
        }
    }
}
