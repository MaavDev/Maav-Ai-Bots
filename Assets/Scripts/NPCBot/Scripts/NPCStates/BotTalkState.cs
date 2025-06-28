using UnityEngine;

public class BotTalkState : INPCState
{
    private float talkDuration = 10f; // how long bots should talk
    private float timer = 0f;

    public void Enter(NPCController npc)
    {
        npc.StopMovement();
        npc.animator.Play("Talk"); // make sure "Talk" animation exists
        timer = 0f;
    }

    public void Update(NPCController npc)
    {
        timer += Time.deltaTime;
        if (timer >= talkDuration)
        {
            npc.animator.Play("Walk");
            npc.ChangeState(NPCState.Walk); // go back to walking after 30 seconds
        }
    }

    public void Exit(NPCController npc)
    {
        // Optional: Reset animation triggers or states
        npc.animator.SetFloat("Speed", 0f);
    }
}
