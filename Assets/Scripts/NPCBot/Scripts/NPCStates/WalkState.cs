using UnityEngine;

public class WalkState : INPCState
{
    public void Enter(NPCController npc) { }

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

    public void Exit(NPCController npc) { }
}