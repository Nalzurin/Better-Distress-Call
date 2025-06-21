using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static System.Net.Mime.MediaTypeNames;

namespace BetterDistressCall
{
    public static class EnterSendLetter
    {

        public static void SendLetter(string stage, string anomaly, Faction faction, string obelisk = null)
        {
            TaggedString titleText = "BDC_LetterTitle".Translate();
            TaggedString stageLevelText = ("BDC_" + faction.def.techLevel.ToString() + "Stage"+ stage).Translate();
            TaggedString anomalyText = ("BDC_" + anomaly).Translate();
            TaggedString obeliskText = obelisk.NullOrEmpty() ? "" : ("BDC_" + obelisk).Translate();
            TaggedString letterText = anomalyText + "\n" + stageLevelText + "\n" + obeliskText;
            if (LanguageDatabase.activeLanguage.TryGetTextFromKey("BDC_" + faction.def.defName, out TaggedString factionText))
            {
                letterText += "\n" + factionText;
            }
            DiaNode diaNode = new DiaNode(letterText);
            DiaOption item = new DiaOption("Close".Translate())
            {
                resolveTree = true
            };
            diaNode.options.Add(item);
            Find.WindowStack.Add(new Dialog_NodeTree(diaNode, delayInteractivity: true, radioMode: true, titleText));
        }
    }
}
