﻿using HarmonyLib;
using RimWorld;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BetterDistressCall
{
    [HarmonyPatch]
    public static class DistressCall_Transpiler
    {
        public static bool IsDisabled(SitePartDef def)
        {
            switch (def.defName)
            {
                case "DistressCall_Shamblers":
                    return BetterDistressCall_Settings.ShamblersVariant;
                case "DistressCall_Chimeras":
                    return BetterDistressCall_Settings.ChimerasVariant;
                case "DistressCall_Horax":
                    return BetterDistressCall_Settings.HoraxVariant;

                default:
                    return true;
            }
        }
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(AccessTools.Inner(typeof(QuestNode_Root_DistressCall), "<>c"), "<RunInt>b__10_0");
        }
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            bool foundBreak = false;
            bool insert = false;
            CodeInstruction codeInstruction = null;
            foreach (CodeInstruction instruction in instructions)
            {
                if (insert)
                {
                    insert = false;
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(DistressCall_Transpiler), "IsDisabled"));
                    yield return codeInstruction;
                }
                if (!foundBreak)
                {
                    if (instruction.opcode == OpCodes.Brfalse_S)
                    {
                        codeInstruction = instruction;
                        foundBreak = true;
                        insert = true;
                    }
                }


                yield return instruction;
            }
        }


    }

}
