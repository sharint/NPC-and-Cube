using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Entity
{
    public enum NPCState { None, Fighting, Staying }
    private NPCState npcState;

    private GameObject npcGameObject;

    private Target target;
    
    private float speed;
    private Vector3 currentPointToMove;
    
    public NPC(string name, float speed,Vector3 spawnPosition, PrimitiveType objectType, NPCState npcState, Target target) : base(name, spawnPosition, objectType)
    {
        this.target = target;
        this.speed = speed;
        this.npcState = npcState;
        
    }

    public NPCState GetState()
    {
        return npcState;
    }

    private void SetCurrentPointToMove(Vector3 currentPointToMove)
    {
        this.currentPointToMove = currentPointToMove;
    }

    public override GameObject Spawn()
    {
        GameObject npcGameObject = base.Spawn();
        this.npcGameObject = npcGameObject;
        return npcGameObject;
    }

    public void Move()
    {
        switch (npcState)
        {
            case NPCState.Fighting:
                Vector3 stayPosition = Controller.stayPosition;
                MoveTowardsNPC(stayPosition);
                if(IsReachedPoint())
                {
                    npcState = NPCState.Staying;
                }
                break;
            case NPCState.Staying:
                Vector3 fightPosition = Controller.fightPosition;
                MoveTowardsNPC(fightPosition);
                if (IsReachedPoint())
                {
                    npcState = NPCState.Fighting;
                }
                break;
            default:
                Debug.LogError("Unknown npc state");
                break;
        }
    }

    private void MoveTowardsNPC(Vector3 targetPosition)
    {
        SetCurrentPointToMove(targetPosition);
        Vector3 currentPosition = npcGameObject.transform.position;
        npcGameObject.transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed);        
    }

    public bool IsReachedPoint()
    {
        if (npcGameObject.transform.position == currentPointToMove)
        {
            return true;
        }
        return false;
    }

    public void Fight()
    {
        target.Hit();
    }
}
