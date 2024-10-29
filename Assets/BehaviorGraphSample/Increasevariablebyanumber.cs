using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Increase variable by a number", story: "[Variable] is increased by [Number]", category: "Action", id: "b37877d845a109199f5ee0099a10967d")]
public partial class IncreaseVariableBy1Action : Action
{
    [SerializeReference] public BlackboardVariable<float> Variable;
    [SerializeReference] public BlackboardVariable<float> Number;
    protected override Status OnStart()
    {
        Variable.Value += Number.Value;
        return Status.Success;
    }


}

