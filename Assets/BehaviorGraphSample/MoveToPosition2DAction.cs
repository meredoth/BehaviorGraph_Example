using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Move To Position 2D", story: "Move [Agent] to [Position]", category: "Action", id: "cecbbedb517a59104c71116d63c62f33")]
public partial class MoveToPosition2DAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Vector2> Position;
    [SerializeReference] public BlackboardVariable<float> Speed = new(1.0f);
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new(0.2f);
    [SerializeReference] public BlackboardVariable<float> SlowDownDistance = new(1.0f);

    private Rigidbody2D _agentRb;
    
    protected override Status OnStart()
    {
        if (ReferenceEquals(Agent?.Value, null))
            return Status.Failure;

        if (!Agent.Value.TryGetComponent(out _agentRb))
        {
            Debug.LogError("Agent Doesn't have a RigidBody!");
            return Status.Failure;
        }

        return Initialize();
    }

    protected override Status OnUpdate()
    {
        if (ReferenceEquals(Agent?.Value, null))
            return Status.Failure;

        float distance = GetDistanceXY();
      
        if (distance <= DistanceThreshold)
            return Status.Success;
            
        float speed = Speed;

        if (SlowDownDistance > 0.0f && distance < SlowDownDistance)
        {
            float ratio = distance / SlowDownDistance;
            speed = Mathf.Max(0.1f, Speed * ratio);
        }
        
        Vector2 agentPosition = Agent.Value.transform.position;
        Vector2 toDestination = Position - agentPosition;
        toDestination.Normalize();
        agentPosition = toDestination * speed;

        _agentRb.linearVelocity = agentPosition; 
        
        Agent.Value.transform.right = toDestination;
            
        return Status.Success;
    }

    private Status Initialize()
    {
        if (!Agent.Value.TryGetComponent(out _agentRb))
        {
            Debug.LogError("Agent Doesn't have a RigidBody!");
            return Status.Failure;
        }

        return GetDistanceXY() <= DistanceThreshold ? Status.Success : Status.Running;
    }

    private float GetDistanceXY()=> Vector2.Distance(Agent.Value.transform.position, Position);
}

