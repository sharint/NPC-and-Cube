using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Vector3 targetPosition;
    private List<Vector3> npcsPosition;
    public static Vector3 stayPosition;
    public static Vector3 fightPosition;

    private List<NPC.NPCState> npcsState;

    [SerializeField] private GameObject healthBarPrefab;

    [SerializeField] private List<string> npcsName;
    [SerializeField] private string targetName;

    [SerializeField] private int damagePerSecond;
    [SerializeField] private int hitDelaySeconds;
    
    [SerializeField] private float npcsSpeed;
    [SerializeField] private int targetHealth;
    private int maxCountNPC;

    private List<NPC> _NPCs;
    private Target cube;

    private bool isHitWaitDelay;
    private bool movingNPC;

    void Start()
    {
        maxCountNPC = 2;

        InitializeCollections();

        AssignObjectPosition();
        AssignObjectStates();

        SpawnTarget();
        SpawnNPSs();

        isHitWaitDelay = true;
    }

    private void InitializeCollections()
    {
        npcsPosition = new List<Vector3>();
        npcsState = new List<NPC.NPCState>();
        _NPCs = new List<NPC>();
    }

    private void AssignObjectStates()
    {
        npcsState.Add(NPC.NPCState.Fighting);
        npcsState.Add(NPC.NPCState.Staying);
    }

    private void AssignObjectPosition()
    {
        targetPosition = new Vector3(0, 0, 0);

        Vector3 npc1Position = new Vector3(-2, 0, 0);
        Vector3 npc2Position = new Vector3(0, 0, 5);
        npcsPosition.Add(npc1Position);
        npcsPosition.Add(npc2Position);

        fightPosition = npc1Position;
        stayPosition = npc2Position;
    }

    private void SpawnNPSs()
    {
        PrimitiveType npcType = PrimitiveType.Capsule;
        for (int i = 0; i < maxCountNPC; i++)
        {
            NPC npc = new NPC(npcsName[i], npcsSpeed * Time.deltaTime, npcsPosition[i], npcType, npcsState[i], cube);
            npc.Spawn();
            _NPCs.Add(npc);
        }
    }

    private void SpawnTarget()
    {
        PrimitiveType targetType = PrimitiveType.Cube;
        cube = new Target(targetName, targetHealth, targetPosition, targetType, healthBarPrefab, damagePerSecond);
        cube.Spawn();
    }

    private void Update()
    {
        if (isHitWaitDelay && !movingNPC)
        {
            isHitWaitDelay = false;
            NPC currentFighter = GetCurrentFighter();
            StartCoroutine(Fight(currentFighter));
        }
        if (movingNPC)
        {
            MoveNPC();
        }
    }

    private NPC GetCurrentFighter()
    {
        foreach(NPC npc in _NPCs)
        {
            NPC.NPCState npcState = npc.GetState();
            if (npcState == NPC.NPCState.Fighting)
            {
                return npc;
            }
        }
        Debug.LogError("NPC fighter not found");
        return null;
    }

    private IEnumerator Fight(NPC currentFighter)
    {
        currentFighter.Fight();
        yield return new WaitForSeconds(hitDelaySeconds);
        isHitWaitDelay = true;
    }

    private void MoveNPC()
    {
        foreach(NPC npc in _NPCs)
        {
            npc.Move();
            if (npc.IsReachedPoint())
            {
                movingNPC = false;
            }
        }
    }

    public void Swap()
    {
        movingNPC = true;
    }
}
