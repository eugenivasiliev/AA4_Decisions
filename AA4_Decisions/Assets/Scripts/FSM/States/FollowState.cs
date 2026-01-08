using AI.StateMachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FollowState<T> : BaseState<T>
{
    private readonly float targetTolerance = 0.2f;
    private readonly float speed = .01f;
    private Vector2 target;

    public override void OnEnter()
    {
        target = (Blackboard as FSMAgent).food.position;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        Vector2 position = (Blackboard as MonoBehaviour).transform.position;
        if (Vector2.Distance(position, target) < targetTolerance)
        {
            (Blackboard as FSMAgent).food = null;
            Owner.Switch<PatrolState<T>>();
            return;
        }

        (Blackboard as MonoBehaviour).transform.position += speed * (Vector3)(target - position).normalized;

        if ((Blackboard as FSMAgent).enemy) Owner.Switch<FleeState<T>>();
    }
}