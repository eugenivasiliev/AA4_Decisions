using System.Collections.Generic;
using UnityEngine;

public class UtilityAgent : MonoBehaviour
{
    [Header("Needs")]
    public float hunger = 0.0f;
    public float safety = 100f;
    public float energy = 100f;
    public float curiosity = 0f;

    [Header("Movement Bounds")]
    public Vector2 areaCenter;
    public float areaRadius = 2f;

    public Transform player;
    public Transform food;

    public bool isPlayer = false;

    private List<UtilityAction> actions = new List<UtilityAction>();
    private UtilityAction currentAction;

    void Start()
    {
        areaCenter = transform.position;

        actions.Add(new EatAction());
        actions.Add(new FleeAction());
        actions.Add(new WanderAction());
        actions.Add(new RestAction());
    }

    void Update()
    {
        if (isPlayer)
        {
            if (hunger < 100)
                hunger += Time.deltaTime * 5f;
            curiosity = 50f;
            //currentAction = actions[2];
            DecideAction();
            currentAction.Execute(this);
            return;
        }
        UpdateNeeds();
        DecideAction();
        currentAction?.Execute(this);

        Debug.Log($"Current Action: {currentAction?.actionName}");
    }

    void UpdateNeeds()
    {
        if(hunger < 100)
            hunger += Time.deltaTime * 5f;
        if(energy > 0)
            energy -= Time.deltaTime * 2f;

        float distance = Vector3.Distance(transform.position, player.position);
        safety = Mathf.Clamp(distance * 15f, 0f, 100f);

        float essentials = Mathf.Clamp(safety * 0.5f + (100f - hunger) * 0.25f + energy * 0.25f, 0f, 100f);
        if(essentials > 50f && curiosity < 100)
            curiosity += Time.deltaTime * 3f;
        else
            if(curiosity > 0)
                curiosity -= Time.deltaTime * 5f;
    }

    void DecideAction()
    {
        float bestScore = -Mathf.Infinity;

        foreach (var action in actions)
        {
            float score = action.CalculateUtility(this);
            if (score > bestScore)
            {
                bestScore = score;
                currentAction = action;
            }
        }
    }

    public Vector3 ClampToArea(Vector3 target)
    {
        Vector2 offset = new Vector2(
            target.x - areaCenter.x,
            target.y - areaCenter.y
        );

        if (offset.magnitude > areaRadius)
        {
            offset = offset.normalized * areaRadius;
            target = new Vector3(
                areaCenter.x + offset.x,
                areaCenter.y + offset.y,
                target.z
            );
        }

        return target;
    }
}