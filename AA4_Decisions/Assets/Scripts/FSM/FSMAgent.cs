using UnityEngine;
using AI;
using AI.StateMachine;

public class FSMAgent : MonoBehaviour
{
    [SerializeField] private StateMachine<FSMAgent> stateMachine;
    public Transform food;
    public Transform enemy;

    void Start()
    {
        stateMachine = new StateMachine<FSMAgent>(this);
        stateMachine.Add<PatrolState<FSMAgent>>();
        stateMachine.Add<FollowState<FSMAgent>>();
        stateMachine.Add<FleeState<FSMAgent>>();
        stateMachine.Switch<PatrolState<FSMAgent>>();
    }

    void Update()
    {
        stateMachine.OnUpdate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if(other.tag == "Food")
        {
            food = other.transform;
        }
        else if(other.TryGetComponent<UtilityAgent>(out UtilityAgent agent))
        {
            enemy = other.transform;
        }
    }
}