using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Agent runs away from Target", story: "[Agent] runs away from [Target]",
    category: "Action", id: "0cf7645aa52eb064699992c3888d06d7")]
public partial class AgentRunsAwayFromTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Speed = new(3.0f);
    
    private Rigidbody2D _agentRb;

    protected override Status OnStart()
    {
        if (ReferenceEquals(Agent?.Value, null) || ReferenceEquals(Target?.Value, null))
            return Status.Failure;

        if (!Agent.Value.TryGetComponent(out _agentRb))
        {
            Debug.LogError("Agent Doesn't have a RigidBody!");
            return Status.Failure;
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (ReferenceEquals(Agent?.Value, null) || ReferenceEquals(Target, null))
            return Status.Failure;

        Vector2 agentPosition = Agent.Value.transform.position;
        Vector2 toDestination = agentPosition - (Vector2)Target.Value.transform.position;

        float speed = Speed;
        toDestination.Normalize();
        agentPosition = toDestination * speed;

        _agentRb.linearVelocity = agentPosition;

        return Status.Success;
    }
}

