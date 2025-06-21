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
        public static bool SightstealersVariant = true;
        public static bool DevourersVariant = true;
        public static bool GorehulksVariant = true;
        public static bool MetalhorrorsVariant = true;
        public static bool Obelisks = true;
        public static float ObeliskChance = 0.1f;
        public static float DeathPallChance = 0.35f;
        public static float ChanceOfSurvivorBeingAChild = 0.05f;
        public static float ShamblersSettlementPointModifier = 1f;
        private string ShamblersSettlementPointModifierBuffer;

        public static float ShamblersPointModifier = 1f;
        private string ShamblersPointModifierBuffer;

        public static float ChimerasSettlementPointModifier = 1f;
        private string ChimerasSettlementPointModifierBuffer;

        public static float ChimerasPointModifier = 1f;
        private string ChimerasPointModifierBuffer;

        public static float HoraxSettlementPointModifier = 1f;
        private string HoraxSettlementPointModifierBuffer;

        public static float HoraxPointModifier = 1f;
        private string HoraxPointModifierBuffer;

        public static float SightstealersSettlementPointModifier = 1f;
        private string SightstealersSettlementPointModifierBuffer;

        public static float SightstealersPointModifier = 1f;
        private string SightstealersPointModifierBuffer;

        public static float DevourersSettlementPointModifier = 1f;
        private string DevourersSettlementPointModifierBuffer;

        public static float DevourersPointModifier = 1f;
        private string DevourersPointModifierBuffer;

        public static float FleshbeastsSettlementPointModifier = 1f;
        private string FleshbeastsSettlementPointModifierBuffer;

        public static float FleshbeastsPointModifier = 1f;
        private string FleshbeastsPointModifierBuffer;

        public static float GorehulksSettlementPointModifier = 1f;
        private string GorehulksSettlementPointModifierBuffer;

        public static float GorehulksPointModifier = 1f;
        private string GorehulksPointModifierBuffer;

        public static float MetalhorrorsSettlementPointModifier = 1f;
        private string MetalhorrorsSettlementPointModifierBuffer;

        public static float MetalhorrorsPointModifier = 1f;
        private string MetalhorrorsPointModifierBuffer;

        private Vector2 _scrollPosition;
        public override void ExposeData()
        {
            Scribe_Values.Look(ref ShamblersVariant, "ShamblersVariant", defaultValue: true);
            Scribe_Values.Look(ref ChimerasVariant, "ChimerasVariant", defaultValue: true);
            Scribe_Values.Look(ref HoraxVariant, "HoraxVariant", defaultValue: true);
            Scribe_Values.Look(ref SightstealersVariant, "SightstealersVariant", defaultValue: true);

            Scribe_Values.Look(ref Obelisks, "Obelisks", defaultValue: true);
            Scribe_Values.Look(ref ObeliskChance, "ObeliskChance", defaultValue: 0.1f);
            Scribe_Values.Look(ref DeathPallChance, "DeathPallChance", defaultValue: 0.35f);

            Scribe_Values.Look(ref ShamblersSettlementPointModifier, "ShamblersSettlementPointModifier", 1f);
            Scribe_Values.Look(ref ShamblersPointModifier, "ShamblersPointModifier", 1f);
            Scribe_Values.Look(ref ChimerasSettlementPointModifier, "ChimerasSettlementPointModifier", 1f);
            Scribe_Values.Look(ref ChimerasPointModifier, "ChimerasPointModifier", 1f);
            Scribe_Values.Look(ref HoraxSettlementPointModifier, "HoraxSettlementPointModifier", 1f);
            Scribe_Values.Look(ref HoraxPointModifier, "HoraxPointModifier", 1f);
            Scribe_Values.Look(ref SightstealersSettlementPointModifier, "SightstealersSettlementPointModifier", 1f);
            Scribe_Values.Look(ref SightstealersPointModifier, "SightstealersPointModifier", 1f);
            Scribe_Values.Look(ref FleshbeastsSettlementPointModifier, "FleshbeastsSettlementPointModifier", 1f);
            Scribe_Values.Look(ref FleshbeastsPointModifier, "FleshbeastsPointModifier", 1f);
            Scribe_Values.Look(ref DevourersSettlementPointModifier, "DevourersSettlementPointModifier", 1f);
            Scribe_Values.Look(ref DevourersPointModifier, "DevourersPointModifier", 1f);
            Scribe_Values.Look(ref GorehulksSettlementPointModifier, "GorehulksSettlementPointModifier", 1f);
            Scribe_Values.Look(ref GorehulksPointModifier, "GorehulksPointModifier", 1f);
            Scribe_Values.Look(ref MetalhorrorsSettlementPointModifier, "MetalhorrorsSettlementPointModifier", 1f);
            Scribe_Values.Look(ref MetalhorrorsPointModifier, "MetalhorrorsPointModifier", 1f);
            Scribe_Values.Look(ref ChanceOfSurvivorBeingAChild, "ChanceOfSurvivorBeingAChild", 0.05f);
            base.ExposeData();
        }

        public void DoWindowContents(Rect inRect)
        {
            Rect rect2 = new Rect(inRect);
            rect2.height = 2000f;
            Rect rect3 = rect2;
            Widgets.AdjustRectsForScrollView(inRect, ref rect2, ref rect3);
            Widgets.BeginScrollView(inRect, ref _scrollPosition, rect3, showScrollbars: false);

            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(rect3);

            // Fleshbeasts
            listing_Standard.Label("Fleshbeasts".Translate());
            listing_Standard.Label("FleshbeastsSettlementPointModifier".Translate());
            listing_Standard.TextFieldNumeric(ref FleshbeastsSettlementPointModifier, ref FleshbeastsSettlementPointModifierBuffer, 0.1f);
            listing_Standard.Label("FleshbeastsPointModifier".Translate());
            listing_Standard.TextFieldNumeric(ref FleshbeastsPointModifier, ref FleshbeastsPointModifierBuffer, 0.1f);
            listing_Standard.Gap();

            // Shamblers
            listing_Standard.CheckboxLabeled("ShamblersVariant".Translate(), ref ShamblersVariant);
            listing_Standard.Label("ShamblersSettlementPointModifier".Translate());
            listing_Standard.TextFieldNumeric(ref ShamblersSettlementPointModifier, ref ShamblersSettlementPointModifierBuffer, 0.1f);
            listing_Standard.Label("ShamblersPointModifier".Translate());
            listing_Standard.TextFieldNumeric(ref ShamblersPointModifier, ref ShamblersPointModifierBuffer, 0.1f);
            listing_Standard.Label("DeathPallChance".Translate() + ": " + (DeathPallChance * 100f).ToString("F0") + "%");
            DeathPallChance = (float)Math.Round(listing_Standard.Slider(DeathPallChance, 0f, 1f), 2);
            listing_Standard.Gap();

            // Chimeras
            listing_Standard.CheckboxLabeled("ChimerasVariant".Translate(), ref ChimerasVariant);
            listing_Standard.Label("ChimerasSettlementPointModifier".Translate());
            listing_Standard.TextFieldNumeric(ref ChimerasSettlementPointModifier, ref ChimerasSettlementPointModifierBuffer, 0.1f);
            listing_Standard.Label("ChimerasPointModifier".Translate());
            listing_Standard.TextFieldNumeric(ref ChimerasPointModifier, ref ChimerasPointModifierBuffer, 0.1f);
            listing_Standard.Gap();

            // Horax
            listing_Standard.CheckboxLabeled("HoraxVariant".Translate(), ref HoraxVariant);
            listing_Standard.Label("HoraxSettlementPointModifier".Translate());
            listing_Standard.TextFieldNumeric(ref HoraxSettlementPointModifier, ref HoraxSettlementPointModifierBuffer, 0.1f);
            listing_Standard.Label("HoraxPointModifier".Translate());
            listing_Standard.TextFieldNumeric(ref HoraxPointModifier, ref HoraxPointModifierBuffer, 0.1f);
            listing_Standard.Gap();

            // Sightstealers
            listing_Standard.CheckboxLabeled("SightstealersVariant".Translate(), ref SightstealersVariant);
            listing_Standard.Label("SightstealersSettlementPointModifier".Translate());
            listing_Standard.TextFieldNumeric(ref SightstealersSettlementPointModifier, ref SightstealersSettlementPointModifierBuffer, 0.1f);
            listing_Standard.Label("SightstealersPointModifier".Translate());
            listing_Standard.TextFieldNumeric(ref SightstealersPointModifier, ref SightstealersPointModifierBuffer, 0.1f);
            listing_Standard.Gap();

            // Devourers
            listing_Standard.CheckboxLabeled("DevourersVariant".Translate(), ref DevourersVariant);
            listing_Standard.Label("DevourersSettlementPointModifier".Translate());
            listing_Standard.TextFieldNumeric(ref DevourersSettlementPointModifier, ref DevourersSettlementPointModifierBuffer, 0.1f);
            listing_Standard.Label("DevourersPointModifier".Translate());
            listing_Standard.TextFieldNumeric(ref DevourersPointModifier, ref DevourersPointModifierBuffer, 0.1f);
            listing_Standard.Gap();

            // Gorehulks
            listing_Standard.CheckboxLabeled("GorehulksVariant".Translate(), ref GorehulksVariant);
            listing_Standard.Label("GorehulksSettlementPointModifier".Translate());
            listing_Standard.TextFieldNumeric(ref GorehulksSettlementPointModifier, ref GorehulksSettlementPointModifierBuffer, 0.1f);
            listing_Standard.Label("GorehulksPointModifier".Translate());
            listing_Standard.TextFieldNumeric(ref GorehulksPointModifier, ref GorehulksPointModifierBuffer, 0.1f);
            listing_Standard.Gap();

            // Metalhorrors
            listing_Standard.CheckboxLabeled("MetalhorrorsVariant".Translate(), ref MetalhorrorsVariant);
            listing_Standard.Label("MetalhorrorsSettlementPointModifier".Translate());
            listing_Standard.TextFieldNumeric(ref MetalhorrorsSettlementPointModifier, ref MetalhorrorsSettlementPointModifierBuffer, 0.1f);
            listing_Standard.Label("MetalhorrorsPointModifier".Translate());
            listing_Standard.TextFieldNumeric(ref MetalhorrorsPointModifier, ref MetalhorrorsPointModifierBuffer, 0.1f);
            listing_Standard.Gap();

            // Obelisks
            listing_Standard.CheckboxLabeled("Obelisks".Translate(), ref Obelisks);
            listing_Standard.Label("ObeliskChance".Translate() + ": " + (ObeliskChance * 100f).ToString("F0") + "%");
            ObeliskChance = (float)Math.Round(listing_Standard.Slider(ObeliskChance, 0f, 1f), 2);

            if (ModLister.CheckBiotech("Biotech"))
            {
                listing_Standard.Label("ChanceOfSurvivorBeingAChild".Translate() + ": " + (ChanceOfSurvivorBeingAChild * 100f).ToString("F0") + "%");
                ChanceOfSurvivorBeingAChild = (float)Math.Round(listing_Standard.Slider(ChanceOfSurvivorBeingAChild, 0f, 1f), 2);
            }

            listing_Standard.End();
            Widgets.EndScrollView();

        }
    }
}