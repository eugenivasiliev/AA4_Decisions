using UnityEngine;

public class FleeAction : UtilityAction
{
    public FleeAction()
    {
        actionName = "Flee";
    }

    public override float CalculateUtility(UtilityAgent agent)
    {
        float safetyScore = 1f - (agent.safety / 100f);
        float distanceToPlayer = Vector3.Distance(agent.transform.position, agent.player.position);
        float proximity = Mathf.Clamp01(1f - distanceToPlayer / 10f);

        return safetyScore * proximity;
    }

    public override void Execute(UtilityAgent agent)
    {
        Vector3 directionAway = (agent.transform.position - agent.player.position).normalized;

        Vector3 desiredTarget = agent.transform.position + directionAway * 3f;

        Vector3 clampedTarget = agent.ClampToArea(desiredTarget);

        agent.transform.position = Vector3.MoveTowards(
            agent.transform.position,
            clampedTarget,
            Time.deltaTime * 3f
        );
    }
}
