public abstract class UtilityAction
{
    public string actionName;

    public abstract float CalculateUtility(UtilityAgent agent);
    public abstract void Execute(UtilityAgent agent);
}
