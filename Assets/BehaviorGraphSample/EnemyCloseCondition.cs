using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Enemy Close", 
    story: "[Agent] is [DistanceThreshold] or less from any of the [Enemies]", 
    category: "Variable Conditions", 
    id: "dedb746231cedb555ffad0a87f5af742")]
public partial class EnemyCloseCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Enemies;
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new(5f);

    private Vector2 _closestTargetPosition;
    private float _colliderOffset;

    public override void OnStart()
    {
        _colliderOffset = 0.0f;
        Collider2D agentCollider = Agent.Value.GetComponentInChildren<Collider2D>();
        if (agentCollider != null)
        {
            Vector2 colliderExtents = agentCollider.bounds.extents;
            _colliderOffset += Mathf.Max(colliderExtents.x, colliderExtents.y);
        }
    }

    public override bool IsTrue()
    {
        _closestTargetPosition = Vector2.positiveInfinity;
        
        foreach (var target in Enemies.Value)
        {
            if(!target.activeSelf)
                continue;
            
            var targetPosition = GetPositionColliderAdjusted(target);
            if (GetDistanceXY(targetPosition) <= GetDistanceXY(_closestTargetPosition))
            {
                _closestTargetPosition = targetPosition;
            }
        }

        return GetDistanceXY(_closestTargetPosition) <= DistanceThreshold + _colliderOffset;
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
