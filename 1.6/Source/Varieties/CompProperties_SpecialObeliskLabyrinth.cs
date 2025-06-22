using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BetterDistressCall.Varieties
{

    public class CompProperties_SpecialObeliskLabyrinth : CompProperties_Interactable
    {
        [MustTranslate]
        public string messageActivating;

        public CompProperties_SpecialObeliskLabyrinth()
        {
            compClass = typeof(CompSpecialObelisk_Labyrinth);
        }
    }
}
