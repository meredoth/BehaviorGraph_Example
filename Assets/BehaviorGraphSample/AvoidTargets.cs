using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Avoid Targets", 
    story: "[Agent] Avoids [Targets]", 
    category: "Action", 
    id: "2b5ddf0e1c97016716b6bbf881b394e2")]
public partial class AvoidTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Targets;
    [SerializeReference] public BlackboardVariable<float> Speed = new(3.0f);
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new(5f);
    
    private Vector2 _closestTargetPosition;
    private float _colliderOffset;
    private Rigidbody2D _agentRb;
    
    protected override Status OnStart()
    {
        if (ReferenceEquals(Agent?.Value, null) || ReferenceEquals(Targets?.Value, null))
            return Status.Failure;
      
        return Initialize();
    }

    protected override Status OnUpdate()
    {
        if (ReferenceEquals(Agent?.Value, null) || ReferenceEquals(Targets, null))
            return Status.Failure;
        
        _closestTargetPosition = Vector2.positiveInfinity;
        
        foreach (var target in Targets.Value)
        {
            if(!target.activeSelf)
                continue;
            
            var targetPosition = GetPositionColliderAdjusted(target);

            if (GetDistanceXY(targetPosition) <= GetDistanceXY(_closestTargetPosition))
            {
                _closestTargetPosition = targetPosition;
            }
        }
        
        if (GetDistanceXY(_closestTargetPosition) > DistanceThreshold + _colliderOffset)
            return Status.Success;
        
        Vector2 agentPosition = Agent.Value.transform.position;
        Vector2 toDestination = agentPosition - _closestTargetPosition;

        float speed = Speed;
        toDestination.Normalize();
        agentPosition = toDestination * speed;

        _agentRb.linearVelocity = agentPosition;
            
        return Status.Success;
    }

    protected override void OnDeserialize() => Initialize();
    
    private Status Initialize()
    {
        if (!Agent.Value.TryGetComponent(out _agentRb))
        {
            Debug.LogError("Agent Doesn't have a RigidBody!");
            return Status.Failure;
        }
        
        _colliderOffset = 0.0f;
        Collider2D agentCollider = Agent.Value.GetComponentInChildren<Collider2D>();
        if (agentCollider != null)
        {
            Vector2 colliderExtents = agentCollider.bounds.extents;
            _colliderOffset += Mathf.Max(colliderExtents.x, colliderExtents.y);
        }
        _closestTargetPosition = Vector2.positiveInfinity;
        
        foreach (var target in Targets.Value)
        {
            if(!target.activeSelf)
                continue;
            
            var targetPosition = GetPositionColliderAdjusted(target);

            if (GetDistanceXY(targetPosition) <= GetDistanceXY(_closestTargetPosition))
            {
                _closestTargetPosition = targetPosition;
            }
        }

        return GetDistanceXY(_closestTargetPosition) > DistanceThreshold + _colliderOffset ? Status.Success : Status.Running;
    }
    
    private Vector2 GetPositionColliderAdjusted(GameObject target)
    {
        Collider2D targetCollider = target.GetComponentInChildren<Collider2D>();
      
        if (targetCollider != null)
            return targetCollider.ClosestPoint(Agent.Value.transform.position);
      
        return target.transform.position;
    }

    private float GetDistanceXY(Vector2 colliderAdjustedTargetPosition) => 
        Vector2.Distance(Agent.Value.transform.position, colliderAdjustedTargetPosition);
}

