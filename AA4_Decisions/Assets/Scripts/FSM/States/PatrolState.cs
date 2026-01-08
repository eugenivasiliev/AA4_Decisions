using AI.StateMachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PatrolState<T> : BaseState<T>
{
    private readonly float patrolRadius = 3f;
    private readonly float targetTolerance = 0.2f;
    private readonly float speed = .01f;
    private Vector2 target;
    private Vector2 patrolCenter;

    public override void OnEnter()
    {
        patrolCenter = (Blackboard as MonoBehaviour).transform.position;
        target = patrolRadius * Random.insideUnitCircle + patrolCenter;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        Vector2 position = (Blackboard as MonoBehaviour).transform.position;
        if (Vector2.Distance(position, target) < targetTolerance) target = patrolRadius * Random.insideUnitCircle + patrolCenter;

        (Blackboard as MonoBehaviour).transform.position += speed * (Vector3)(target - position).normalized;

        if ((Blackboard as FSMAgent).food) Owner.Switch<FollowState<T>>();
        if ((Blackboard as FSMAgent).enemy) Owner.Switch<FleeState<T>>();
    }
}