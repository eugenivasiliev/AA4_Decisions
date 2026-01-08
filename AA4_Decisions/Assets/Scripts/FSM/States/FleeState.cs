using AI.StateMachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FleeState<T> : BaseState<T>
{
    private readonly float escapeRadius = 5f;
    private readonly float speed = .01f;
    private Transform enemy;

    public override void OnEnter()
    {
        enemy = (Blackboard as FSMAgent).enemy;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        Vector2 position = (Blackboard as MonoBehaviour).transform.position;

        (Blackboard as MonoBehaviour).transform.position -= speed * (enemy.position - (Vector3)position).normalized;

        if (Vector2.Distance(position, enemy.position) > escapeRadius)
        {
            (Blackboard as FSMAgent).enemy = null;
            Owner.Switch<PatrolState<T>>();
            return;
        }
    }
}