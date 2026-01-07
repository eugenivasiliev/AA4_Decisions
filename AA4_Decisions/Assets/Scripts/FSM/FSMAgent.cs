using UnityEngine;
using AI;
using AI.StateMachine;

public class FSMAgent : MonoBehaviour
{
    [SerializeField] private StateMachine<FSMAgent> stateMachine;
    public Transform food;

    void Start()
    {
        stateMachine = new StateMachine<FSMAgent>(this);
        stateMachine.Add<PatrolState<FSMAgent>>();
        stateMachine.Add<FollowState<FSMAgent>>();
        stateMachine.Switch<PatrolState<FSMAgent>>();
    }

    void Update()
    {
        stateMachine.OnUpdate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if(other.tag == "Food")
        {
            food = other.transform;
        }
    }
}

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
        if(Vector2.Distance(position, target) < targetTolerance) target = patrolRadius * Random.insideUnitCircle + patrolCenter;

        (Blackboard as MonoBehaviour).transform.position += speed * (Vector3)(target - position).normalized;

        if((Blackboard as FSMAgent).food) Owner.Switch<FollowState<T>>();
    }
}

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
        if (Vector2.Distance(position, target) < targetTolerance) {
            (Blackboard as FSMAgent).food = null;
            Owner.Switch<PatrolState<T>>();
            return;
        }

        (Blackboard as MonoBehaviour).transform.position += speed * (Vector3)(target - position).normalized;

        //TODO: Check state change conditions
    }
}
