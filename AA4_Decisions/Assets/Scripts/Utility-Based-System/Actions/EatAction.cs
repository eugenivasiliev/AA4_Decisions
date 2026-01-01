using UnityEngine;

public class EatAction : UtilityAction
{
    public EatAction()
    {
        actionName = "Eat";
    }

    public override float CalculateUtility(UtilityAgent agent)
    {
        float hungerScore = agent.hunger / 100f;
        float distanceToFood = Vector3.Distance(agent.transform.position, agent.food.position);

        float foodProximity = Mathf.Clamp01(1f - distanceToFood / 10f);
        return hungerScore * foodProximity;
    }

    public override void Execute(UtilityAgent agent)
    {
        agent.transform.position =
            Vector3.MoveTowards(agent.transform.position, agent.food.position, Time.deltaTime * 2f);

        if (Vector3.Distance(agent.transform.position, agent.food.position) < 1f)
        {
            agent.hunger = 0f;
        }
    }
}