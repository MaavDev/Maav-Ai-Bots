public enum NPCState
{
    Idle,
    Walk,
    Run,
    Talk,
    Swim,
    BotTalk,
    AvoidObstacle
}

public interface INPCState
{
    void Enter(NPCController npc);
    void Update(NPCController npc);
    void Exit(NPCController npc);
}