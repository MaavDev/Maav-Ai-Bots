using UnityEngine;


public class SwimState : INPCState
{
    public void Enter(NPCController npc)
    {
        npc.StopMovement();
        npc.animator.SetFloat("Speed", 6f);
        npc.animator.Play("Swim_Underwater_Male");
    }

    public void Exit(NPCController npc)
    {

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
}



//public class SwimState : INPCState
//{
//    private float switchTimer = 0f;
//    private float switchInterval = 3f; // Time between mode changes
//    private float currentSwimBlend = 0f;

//    public void Enter(NPCController npc)
//    {
//        npc.StopMovement();

//        currentSwimBlend = GetRandomSwimBlend();
//        npc.animator.SetFloat("SwimBlend", currentSwimBlend);

//        switchTimer = 0f;
//    }

//    public void Update(NPCController npc)
//    {
//        switchTimer += Time.deltaTime;
//        if (switchTimer >= switchInterval)
//        {
//            switchTimer = 0f;
//            currentSwimBlend = GetRandomSwimBlend();
//            npc.animator.SetFloat("SwimBlend", currentSwimBlend);
//        }

//        // Only move if not in idle swim mode (i.e. swimming surface/underwater)
//        if (currentSwimBlend > 0f)
//        {
//            if (npc.waypoints.Length == 0) return;

//            npc.MoveTowards(npc.targetWaypoint.position);

//            if (Vector3.Distance(npc.transform.position, npc.targetWaypoint.position) < npc.StopDistance)
//            {
//                npc.currentWaypointIndex = (npc.currentWaypointIndex + 1) % npc.waypoints.Length;
//                npc.targetWaypoint = npc.waypoints[npc.currentWaypointIndex];
//            }
//        }
//    }

//    public void Exit(NPCController npc)
//    {
//        npc.animator.SetFloat("SwimBlend", 0f); // Reset to idle swim
//    }

//    private float GetRandomSwimBlend()
//    {
//        int randomMode = Random.Range(0, 3); // 0 = idle, 1 = surface, 2 = underwater
//        return randomMode switch
//        {
//            0 => 0f,   // Idle swim
//            1 => 0.5f, // Surface swimming
//            2 => 1f,   // Underwater swimming
//            _ => 0f
//        };
//    }
//}




