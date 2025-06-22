using HarmonyLib;
using RimWorld.QuestGen;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BetterDistressCall
{
    [HarmonyPatch]
    public static class Patch_QuestNode_Root_DistressCall_FactionUsable
    {

        public static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(QuestNode_Root_DistressCall), "FactionUsable");
        }
        public static void Postfix(ref bool __result, Faction f, float points)
        {
            if (!__result)
            {
                if (ModsConfig.RoyaltyActive && points < 2000f && f == Faction.OfEmpire)
                {
                    return;
                }
                if (!f.def.canGenerateQuestSites)
                {
                    return;
                }
                if (f.def.humanlikeFaction && !f.def.pawnGroupMakers.NullOrEmpty())
                {
                    __result = true;
                }
                return;
            }

        }
    }

}
