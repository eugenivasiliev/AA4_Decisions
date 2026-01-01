using UnityEngine;

public class WanderAction : UtilityAction
{
    private Vector3 wanderTarget;
    private float changeTargetTime = 0f;
    public WanderAction()
    {
        actionName = "Wander";
    }

    public override float CalculateUtility(UtilityAgent agent)
    {
        float hungerScore = 1f - (agent.hunger / 100f);
        float safetyScore = agent.safety / 100f;
        float energyScore = agent.energy / 100f;
        float curiosity = agent.curiosity / 100f;

        return hungerScore * safetyScore * energyScore * curiosity;
    }

    public override void Execute(UtilityAgent agent)
    {
        changeTargetTime -= Time.deltaTime;

        if (changeTargetTime <= 0f)
        {
            Vector2 randomCircle = Random.insideUnitCircle * 2f;
            wanderTarget = agent.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);
            changeTargetTime = 3f;
        }

        agent.transform.position = Vector3.MoveTowards(
            agent.transform.position,
            wanderTarget,
            Time.deltaTime * 1.5f
        );
    }
}
