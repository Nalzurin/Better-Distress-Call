
using UnityEngine;
using Verse;

namespace BetterDistressCall.Settings
{
    public class BetterDistressCall_Mod : Mod
    {
        public static BetterDistressCall_Settings settings;

        public BetterDistressCall_Mod(ModContentPack content)
            : base(content)
        {
            settings = GetSettings<BetterDistressCall_Settings>();
        }

        public override string SettingsCategory()
        {
            return "Better Distress Call";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoWindowContents(inRect);
        }
    }
}
