using UnityEngine;

public class PlayerSpawner : BaseBehaviour
{
    protected override void Start()
    {
        base.Start();

        Managers.ObjectManager.SummonPlayer(new Vector3(0f, 0f));
    }
}
