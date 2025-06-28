public class TalkState : INPCState
{
    public void Enter(NPCController npc)
    {
        npc.StopMovement();
        npc.animator.Play("Talk");
        //npc.StartSpeech();
    }

    public void Update(NPCController npc) { }

    public void Exit(NPCController npc)
    {
        npc.IsSpeeched = false;
    }
}