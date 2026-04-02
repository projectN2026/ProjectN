using UnityEngine;

public class PlayerSpawner : BaseBehaviour
{
    protected override void Start()
    {
        base.Start();

        ObjectManager.SummonPlayer(new Vector3(0f, 0f));
    }
}
