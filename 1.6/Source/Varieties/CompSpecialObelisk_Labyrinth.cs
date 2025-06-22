using BetterDistressCall.Varieties;
using RimWorld;
using Verse;

public class CompSpecialObelisk_Labyrinth : CompInteractable
{
	public new CompProperties_SpecialObeliskLabyrinth Props => (CompProperties_SpecialObeliskLabyrinth)props;

	protected override void OnInteracted(Pawn caster)
	{
		Messages.Message(Props.messageActivating, parent, MessageTypeDefOf.NeutralEvent, historical: false);
		Log.Message("Closing map");
		parent.Map.GetComponent<SpecialLabyrinthMapComponent>().StartClosing();
	}
}