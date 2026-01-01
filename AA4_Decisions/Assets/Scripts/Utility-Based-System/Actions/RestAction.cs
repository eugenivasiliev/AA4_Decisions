using UnityEngine;

public class RestAction : UtilityAction
{
    public RestAction()
    {
        actionName = "Rest";
    }

    public override float CalculateUtility(UtilityAgent agent)
    {
        float energyNeed = 1f - (agent.energy / 100f);
        float safetyScore = agent.safety / 100f;

        return energyNeed * safetyScore;
    }

    public override void Execute(UtilityAgent agent)
    {
        agent.energy += Time.deltaTime * 20f;
        agent.energy = Mathf.Clamp(agent.energy, 0f, 100f);
    }
}
