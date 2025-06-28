public class IdleState : INPCState
{
    public void Enter(NPCController npc)
    {
        npc.StopMovement();
    }

    public void Update(NPCController npc) { }

    public void Exit(NPCController npc) { }
}