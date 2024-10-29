using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get Object Position", 
    story: "Get from current [Object] its last [Position]", 
    category: "Action", 
    id: "ebdab427dc17cf31410d928103c1af6c")]
public partial class GetObjectPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Object;
    [SerializeReference] public BlackboardVariable<Vector2> Position;

    protected override Status OnStart()
    {
        Position.Value = Object.Value.transform.position;
        return Status.Success;
    }
}

