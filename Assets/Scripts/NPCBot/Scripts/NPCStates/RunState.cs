using UnityEngine;

public class RunState : INPCState
{
    public void Enter(NPCController npc)
    {
        npc.MoveSpeed = 5.0f;
        npc.animator.SetFloat("Speed", 1.0f);
    }

    public void Update(NPCController npc)
    {
        if (npc.waypoints.Length == 0) return;

        npc.MoveTowards(npc.targetWaypoint.position);

        if (Vector3.Distance(npc.transform.position, npc.targetWaypoint.position) < npc.StopDistance)
        {
            npc.currentWaypointIndex = (npc.currentWaypointIndex + 1) % npc.waypoints.Length;
            npc.targetWaypoint = npc.waypoints[npc.currentWaypointIndex];
        }
    }

    public void Exit(NPCController npc)
    {
        // Optional: reset speed or animation
    }
}
