using UnityEngine;

public class RoutesUI : BaseBehaviour
{
    [DescendantsField] 
    public RouteUI[] Routes;
    public float EffectSec;

    protected override void Update()
    {
        base.Update();

        UpdateRoutes(Managers.GameDataManager.RouteNumber);
    }

    private void UpdateRoutes(int fillRouteNumber)
    {
        if (Routes == null || Routes.Length == 0)
            return;

        int siblingIndex = 0;

        foreach (RouteUI route in Routes)
        {
            route.EffectSec = EffectSec;

            if (route.RouteNumber == fillRouteNumber)
            {
                route.IsFill = true;
                route.transform.SetSiblingIndex(Routes.Length - 1);
            }
            else
            {
                route.IsFill = false;
                route.transform.SetSiblingIndex(siblingIndex);
                siblingIndex++;
            }
        }
    }
}
