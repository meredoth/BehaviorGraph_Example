using System;
using BehaviorGraphSample;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Debug = UnityEngine.Debug;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get HasEatenPill Boolean From Hero ", 
    story: "Get [Agent] has [Boolean]", 
    category: "Action", 
    id: "b6a77d0c81222ac18f8385fb7075771a")]
public partial class GetBooleanFromGamObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<bool> Boolean;
    
    private Hero _hero;
    
    protected override Status OnStart()
    {
        if(Agent == null || Boolean == null)
        {
            return Status.Failure;
        }

        if (!Agent.Value.TryGetComponent(out _hero))
        {
            Debug.LogError("Agent needs a Hero type script!");
            return Status.Failure;
        }

        Boolean.Value = _hero.HasEatenPill;
        return Status.Success;
    }
}

