using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get List Of Targets", 
    story: "Find All [Targets] With [Tag]", 
    category: "Action", 
    id: "226c2fbf2b3fabbd8a7faae711b627dc")]

public partial class GetListOfTargetsAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> Targets;
    [SerializeReference] public BlackboardVariable<string> Tag;

    protected override Status OnUpdate()
    {
        List<GameObject> tagged = GameObject.FindGameObjectsWithTag(Tag).ToList();
        
        if (tagged.Count == 0)
            return Status.Failure;
        
        Targets.Value = tagged;
        return Status.Success;
    }
}

