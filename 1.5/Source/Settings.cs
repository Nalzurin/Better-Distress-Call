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
    public class BetterDistressCall_Settings : ModSettings
    {
        public static bool ShamblersVariant = true;
        public static bool ChimerasVariant = true;
        public static bool HoraxVariant = true;

        public static bool Obelisks = true;
        public static float ObeliskChance = 0.1f;

        public override void ExposeData()
        {
            
            Scribe_Values.Look(ref ShamblersVariant, "ShamblersVariant", defaultValue: true, forceSave: true);
            Scribe_Values.Look(ref ChimerasVariant, "ChimerasVariant", defaultValue: true, forceSave: true);
            Scribe_Values.Look(ref HoraxVariant, "HoraxVariant", defaultValue: true, forceSave: true);
            Scribe_Values.Look(ref Obelisks, "Obelisks", defaultValue: true, forceSave: true);
            Scribe_Values.Look(ref ObeliskChance, "ObeliskChance", defaultValue: 0.1f, forceSave: true);


            base.ExposeData();
        }

        public void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.CheckboxLabeled("ShamblersVariant".Translate(), ref ShamblersVariant);
            listing_Standard.CheckboxLabeled("ChimerasVariant".Translate(), ref ChimerasVariant);
            listing_Standard.CheckboxLabeled("HoraxVariant".Translate(), ref HoraxVariant);
            listing_Standard.CheckboxLabeled("Obelisks".Translate(), ref Obelisks);
            listing_Standard.Label("ObeliskChance".Translate() + ": " + (ObeliskChance*100f).ToString() + "%");
            ObeliskChance = (float)Math.Round((double)listing_Standard.Slider(ObeliskChance, 0f, 1f), 2);
            listing_Standard.End();
        }
    }
}