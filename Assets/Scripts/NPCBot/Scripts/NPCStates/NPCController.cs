using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class NPCController : MonoBehaviour
{
    public float MoveSpeed = 2.0f;
    public float RotationSpeed = 5.0f;
    public float StopDistance = 0.5f;
    public float RaycastDistance = 3f;
    public Transform[] waypoints;
    public string NPCname;

    public Animator animator;

    [HideInInspector] public Transform targetWaypoint;
    [HideInInspector] public int currentWaypointIndex = 0;
    [HideInInspector] public float inputMagnitude;
    [HideInInspector] public float animationBlend;
    [HideInInspector] public bool IsPlayerInRange = false;
    [HideInInspector] public bool IsSpeeched = false;
    [HideInInspector] public NPCState CurrentStateName { get; private set; }
    private float lastBotTalkTime = -999f;
    private float botTalkCooldown = 35f; // wait this long before another bot-talk

    [SerializeField]private enum SelectableState
    {
        Idle,
        Walk,
        Run,
        Talk,
        Swim,
        BotTalk,
        AvoidObstacle
    }

    [SerializeField] private SelectableState whichState = SelectableState.Walk;
    //[Header("Debug")]
    //public DebugState debugState = DebugState.None;
    //private DebugState lastDebugState = DebugState.None;


    private INPCState currentState;
    private Dictionary<NPCState, INPCState> stateMap;

    private int _animIDSpeed;
    private int _animIDMotionSpeed;


    private void Start()
    {
        animator = GetComponent<Animator>();
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

        targetWaypoint = waypoints[0];

        stateMap = new Dictionary<NPCState, INPCState>
        {
            { NPCState.Idle, new IdleState() },
            { NPCState.Walk, new WalkState() },
            { NPCState.Run, new RunState() },
            { NPCState.Swim, new SwimState() },
            { NPCState.BotTalk, new BotTalkState() }
            //{ NPCState.Talk, new TalkState() }
        };
        // Convert SelectableState to actual NPCState enum
        NPCState initialState = (NPCState)whichState;
        ChangeState(initialState);
        //ChangeState(NPCState.Swim);
    }

    private void Update()
    {
        currentState?.Update(this);
    }

    public void ChangeState(NPCState newState)
    {
        currentState?.Exit(this);
        currentState = stateMap[newState];
        CurrentStateName = newState;
        currentState.Enter(this);
    }

    public void MoveTowards(Vector3 destination)
    {
        Vector3 direction = (destination - transform.position).normalized;
        transform.position += direction * MoveSpeed * Time.deltaTime;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        inputMagnitude = direction.magnitude;
        animationBlend = Mathf.Lerp(animationBlend, MoveSpeed, Time.deltaTime * 5f);
        animator.SetFloat(_animIDSpeed, animationBlend);
        animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
    }

    public void StopMovement()
    {
        animator.SetFloat(_animIDSpeed, 0f);
        animator.SetFloat(_animIDMotionSpeed, 0f);
    }
    private void CheckNearbyBots()
    {
        if (Time.time - lastBotTalkTime < botTalkCooldown) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, 2f);

        foreach (var hit in hits)
        {
            if (hit.gameObject == this.gameObject) continue;

            NPCController otherNPC = hit.GetComponent<NPCController>();
            if (otherNPC != null && otherNPC.CurrentStateName != NPCState.BotTalk && this.CurrentStateName != NPCState.BotTalk)
            {
                this.ChangeState(NPCState.BotTalk);
                otherNPC.ChangeState(NPCState.BotTalk);
                this.lastBotTalkTime = Time.time;
                otherNPC.lastBotTalkTime = Time.time;
            }
        }
    }
    public void StartSpeech()
    {
        ttsrust_say("Hey I Am " + NPCname + " What are you looking for");
        IsSpeeched = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerInRange = true;
            if (!IsSpeeched)
            {
                //ChangeState(NPCState.Talk);
            }
        }
        if (other.CompareTag("Bot"))
        {
            CheckNearbyBots();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerInRange = false;
            ChangeState(NPCState.Walk);
        }
    }

    #if !UNITY_EDITOR && (UNITY_IOS || UNITY_WEBGL)
    const string _dll = "__Internal";
    #else
    const string _dll = "ttsrust";
    #endif

    [DllImport(_dll)]
    private static extern void ttsrust_say(string text);
}