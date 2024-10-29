using System;
using Unity.Behavior.GraphFramework;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/Enemy Collision")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "Enemy Collision", message: "Agent has collided with [Enemy]", category: "Events", id: "fd221eb99f1aaf6366d252c9bde789cf")]
public partial class EnemyCollision : EventChannelBase
{
    public delegate void EnemyCollisionEventHandler(GameObject Enemy);
    public event EnemyCollisionEventHandler Event; 

    public void SendEventMessage(GameObject Enemy)
    {
        Event?.Invoke(Enemy);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<GameObject> EnemyBlackboardVariable = messageData[0] as BlackboardVariable<GameObject>;
        var Enemy = EnemyBlackboardVariable != null ? EnemyBlackboardVariable.Value : default(GameObject);

        Event?.Invoke(Enemy);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        EnemyCollisionEventHandler del = (Enemy) =>
        {
            BlackboardVariable<GameObject> var0 = vars[0] as BlackboardVariable<GameObject>;
            if(var0 != null)
                var0.Value = Enemy;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as EnemyCollisionEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as EnemyCollisionEventHandler;
    }
}

